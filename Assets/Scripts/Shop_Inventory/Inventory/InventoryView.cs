using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryView : BaseItemListView
{
    [SerializeField] private FilterController inventoryFilterController;
    [SerializeField] private CanvasGroup weightExceededPopup;

    [Header("Sell Section")]
    [SerializeField] private CanvasGroup sellSection;
    [SerializeField] private TransactionSectionController sellSectionController;

    private InventoryController inventoryController;

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
        EventService.Instance.OnItemChanged.RemoveListener(inventoryController.SetPanelViews);
    }

    public void SetInventoryController(InventoryController controller)
    {
        inventoryController = controller;
        EventService.Instance.OnItemChanged.AddListener(inventoryController.SetPanelViews);
        sellSectionController.GetAvailableQuantity = () => inventoryController.GetItemQuantity(inventoryController.GetCurrentItem().itemProperty.itemID);
        sellSectionController.GetUnitPrice = () => inventoryController.GetCurrentItem().itemProperty.sellingPrice;
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
        if (inventoryController.GetTotalWeight() < inventoryController.GetPlayerBagCapacity())
        {
            inventoryController.DisablePanel();
            inventoryController.GatherResource();
            inventoryController.SetBagWeight(inventoryController.GetTotalWeight());
        }
        else
        {
            EventService.Instance.OnMaximumWeightExceed?.InvokeEvent();
            weightExceededPopup.alpha = 1;
            weightExceededPopup.blocksRaycasts = true;
            weightExceededPopup.interactable = true;
        }
    }

    public void DisplayGatheredItem(int index)
    {
        ItemProperty itemProperty = inventoryController.GetItemDatabase()[index];
        int newQuantity = inventoryController.GenerateRandomQuantity();
        InstantiateOrUpdateItem(itemProperty, newQuantity);
    }

    public void DisplayGatheredItem(int index, int quantity)
    {
        ItemProperty itemProperty = inventoryController.GetItemDatabase()[index];
        InstantiateOrUpdateItem(itemProperty, quantity);
    }

    public void DisplayBroughtItem(ItemView itemView, int newQuantity)
    {
        InstantiateOrUpdateItem(itemView.itemProperty, newQuantity);
        inventoryController.SetBagWeight(inventoryController.GetTotalWeight());
    }

    private void InstantiateOrUpdateItem(ItemProperty itemProperty, int quantityToAdd)
    {
        int itemID = itemProperty.itemID;
        if (inventoryController.IsItemAlreadyInstantiated(itemID))
        {
            int existingQuantity = inventoryController.GetItemQuantity(itemID);
            int newTotalQuantity = existingQuantity + quantityToAdd;
            inventoryController.SetQuantity(itemID, newTotalQuantity);

            for (int i = 0; i < quantityToAdd; i++)
                inventoryController.SetItemWeight(itemID, itemProperty.weight);

            ItemView existingItem = inventoryController.GetInstantiatedItem(itemID);

            if (existingItem != null)
                existingItem.InventoryDisplayUI(newTotalQuantity);
        }
        else
        {
            ItemView itemView = CreateItemView(itemProperty);
            if (itemView != null)
            {
                inventoryController.StoreItem(itemView, inventoryFilterController);
                inventoryController.SetQuantity(itemID, quantityToAdd);

                for (int i = 0; i < quantityToAdd; i++)
                    inventoryController.SetItemWeight(itemID, itemProperty.weight);

                inventoryController.StoreInstantiatedItem(itemID, itemView);
                itemView.InventoryDisplayUI(quantityToAdd);
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

    public void ShowWeightExceededPopup()
    {
        EventService.Instance.OnMaximumWeightExceed?.InvokeEvent();
        weightExceededPopup.alpha = 1;
        weightExceededPopup.blocksRaycasts = true;
        weightExceededPopup.interactable = true;
    }

    public void Sell()
    {
        int amount = int.Parse(sellSectionController.GetPriceText());
        int sellQuantity = int.Parse(sellSectionController.GetQuantityText());
        int itemID = inventoryController.GetCurrentItem().itemProperty.itemID;

        if (amount > 0 && sellQuantity > 0)
        {
            sellSectionController.ResetSection();
            inventoryController.RemoveWeight(itemID, sellQuantity);

            int newInventoryQuantity = inventoryController.GetItemQuantity(itemID) - sellQuantity;

            inventoryController.ResetQuantities(itemID);
            inventoryController.SetQuantity(itemID, newInventoryQuantity);
            inventoryController.GetCurrentItem().SetQuantityText(newInventoryQuantity);

            GameManager.Instance.shopController.IncreaseItemQuantity(itemID, sellQuantity);
            GameManager.Instance.shopController.UpdateItemQuantityUI(itemID);

            EventService.Instance.OnItemChanged?.InvokeEvent();
            EventService.Instance.OnItemSoldWithIntParams?.InvokeEvent(amount);
            EventService.Instance.OnItemSoldWithFloatParams?.InvokeEvent(inventoryController.GetTotalWeight());

            if (newInventoryQuantity <= 0)
                RemoveItem(itemID);
        }
        else
        {
            EventService.Instance.OnNonClickableButtonPressed?.InvokeEvent();
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