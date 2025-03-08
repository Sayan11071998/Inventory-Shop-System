using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryView : BaseView
{
    private InventoryController inventoryController;
    [SerializeField] private Transform parentPanel;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private FilterController inventoryFilterController;
    [SerializeField] private CanvasGroup weightExceededPopup;

    [Header("Sell Section")]
    [SerializeField] private CanvasGroup sellSection;
    // Instead of separate TMP fields, we now use the TransactionSectionController.
    [SerializeField] private TransactionSectionController sellSectionController;

    public bool isInventoryOn = false;

    private void OnEnable()
    {
        EventService.Instance.OnInventoryToggledOnEvent.AddListener(EnableInventoryVisibility);
        EventService.Instance.OnShopToggledOnEvent.AddListener(DisableInventoryVisibility);
        EventService.Instance.OnShopToggledOnEvent.AddListener(DisableSellSection);
        EventService.Instance.OnItemSelectedEvent.AddListener(EnableSellSection);
        EventService.Instance.OnItemSelectedEventWithParams.AddListener(SetSelectedItem);
    }

    private void OnDisable()
    {
        EventService.Instance.OnInventoryToggledOnEvent.RemoveListener(EnableInventoryVisibility);
        EventService.Instance.OnShopToggledOnEvent.RemoveListener(DisableInventoryVisibility);
        EventService.Instance.OnShopToggledOnEvent.RemoveListener(DisableSellSection);
        EventService.Instance.OnItemSelectedEvent.RemoveListener(EnableSellSection);
        EventService.Instance.onItemChanged.RemoveListener(inventoryController.SetPanelViews);
    }

    public void SetInventoryController(InventoryController controller)
    {
        inventoryController = controller;
        EventService.Instance.onItemChanged.AddListener(inventoryController.SetPanelViews);

        // Set up the TransactionSectionController delegates.
        sellSectionController.GetAvailableQuantity = () => inventoryController.GetItemQuantity(inventoryController.GetCurrentItem().itemProperty.itemID);
        sellSectionController.GetUnitPrice = () => inventoryController.GetCurrentItem().itemProperty.sellingPrice;
        sellSectionController.PlayQuantityChangedSound = () => EventService.Instance.OnQuantityChanged.InvokeEvent();
        sellSectionController.PlayNonClickableSound = () => EventService.Instance.OnNonClickableButtonPressed.InvokeEvent();
    }

    public void EnableInventoryVisibility()
    {
        isInventoryOn = true;
        EnableVisibility();
    }

    public void DisableInventoryVisibility()
    {
        isInventoryOn = false;
        DisableVisibility();
    }

    public void GatherResource()
    {
        if (inventoryController.GetPlayerBagWeight() < inventoryController.GetPlayerBagCapacity())
        {
            inventoryController.DisablePanel();
            inventoryController.GatherResource();
            inventoryController.SetBagWeight(inventoryController.GetTotalWeight());
        }
        else
        {
            EventService.Instance.OnMaximumWeightExceed.InvokeEvent();
            weightExceededPopup.alpha = 1;
            weightExceededPopup.blocksRaycasts = true;
            weightExceededPopup.interactable = true;
        }
    }

    public void DisplayGatheredItem(int index)
    {
        int itemID = inventoryController.GetItemDatabase()[index].itemID;
        int newQuantity = inventoryController.GenerateRandomQuantity();
        ItemProperty itemProperty = inventoryController.GetItemDatabase()[index];
        InstantiateItems(itemID, newQuantity, itemProperty);
    }

    public void DisplayBroughtItem(ItemView itemView, int newQuantity)
    {
        int itemID = itemView.itemProperty.itemID;
        if (itemView != null)
        {
            InstantiateItems(itemID, newQuantity, itemView.itemProperty);
            inventoryController.SetBagWeight(inventoryController.GetTotalWeight());
        }
    }

    private void InstantiateItems(int itemID, int newQuantity, ItemProperty itemProperty)
    {
        if (inventoryController.IsItemAlreadyInstantiated(itemID))
        {
            inventoryController.SetQuantity(itemID, newQuantity);
            ItemView existingItem = inventoryController.GetInstantiatedItem(itemID);
            inventoryController.SetItemWeight(itemID, existingItem.itemProperty.weight);
            if (existingItem != null)
            {
                int totalQuantity = inventoryController.GetItemQuantity(existingItem.itemProperty.itemID);
                existingItem.InventoryDisplayUI(totalQuantity);
            }
        }
        else
        {
            GameObject newItem = Instantiate(itemPrefab, parentPanel);
            ItemView itemView = newItem.GetComponent<ItemView>();
            if (itemView != null)
            {
                itemView.itemProperty = itemProperty;
                inventoryController.StoreItem(itemView, inventoryFilterController);
                inventoryController.SetQuantity(itemView.itemProperty.itemID, newQuantity);
                inventoryController.SetItemWeight(itemID, itemView.itemProperty.weight);
                inventoryController.StoreInstantiatedItem(itemView.itemProperty.itemID, itemView);
                itemView.InventoryDisplayUI(inventoryController.GetItemQuantity(itemView.itemProperty.itemID));
            }
        }
        inventoryController.ApplyFilter(inventoryFilterController);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void EnableSellSection()
    {
        if (isInventoryOn)
        {
            sellSection.alpha = 1;
            sellSection.interactable = true;
            sellSection.blocksRaycasts = true;
            sellSectionController.ResetSection();
        }
    }

    public void DisableSellSection()
    {
        if (!isInventoryOn)
        {
            sellSection.alpha = 0;
            sellSection.interactable = false;
            sellSection.blocksRaycasts = false;
        }
    }

    public void SetSelectedItem(bool isOn, ItemView itemView)
    {
        inventoryController.SetCurrentItem(itemView);
        sellSectionController.ResetSection();
    }

    // Updated Sell method that uses values from TransactionSectionController.
    public void Sell()
    {
        int amount = int.Parse(sellSectionController.GetPriceText());
        int quantity = int.Parse(sellSectionController.GetQuantityText());
        int itemID = inventoryController.GetCurrentItem().itemProperty.itemID;
        if (amount > 0 && quantity > 0)
        {
            sellSectionController.ResetSection();
            inventoryController.RemoveWeight(itemID, quantity);
            quantity = inventoryController.GetItemQuantity(itemID) - quantity;
            inventoryController.ResetQuantities(itemID);
            inventoryController.SetQuantity(itemID, quantity);
            inventoryController.GetCurrentItem().SetQuantityText(quantity);
            EventService.Instance.onItemChanged.InvokeEvent();
            EventService.Instance.onItemSoldWithIntParams.InvokeEvent(amount);
            EventService.Instance.onItemSoldWithFloatParams.InvokeEvent(inventoryController.GetTotalWeight());
            if (quantity <= 0)
                RemoveItem(itemID);
        }
        else
        {
            EventService.Instance.OnNonClickableButtonPressed.InvokeEvent();
        }
    }

    private void RemoveItem(int itemID)
    {
        ItemView itemToRemove = inventoryController.GetCurrentItem();
        if (itemToRemove != null)
        {
            inventoryController.RemoveItem(itemID);
            Destroy(itemToRemove.gameObject);
        }
    }

    public void DisableweightExceededPopup()
    {
        weightExceededPopup.alpha = 0;
        weightExceededPopup.blocksRaycasts = false;
        weightExceededPopup.interactable = false;
    }
}