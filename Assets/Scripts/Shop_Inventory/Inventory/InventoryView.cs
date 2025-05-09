using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryView : BaseView
{
    [SerializeField] private FilterController inventoryFilterController;
    [SerializeField] private CanvasGroup weightExceededPopup;
    [SerializeField] private Button gatherResourceButton;

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
        sellSectionController.GetAvailableQuantity = () => inventoryController.GetItemQuantity(inventoryController.GetCurrentItem().itemProperty.GetInstanceID());
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
        int key = itemProperty.GetInstanceID();
        if (inventoryController.IsItemAlreadyInstantiated(key))
        {
            int existingQuantity = inventoryController.GetItemQuantity(key);
            int newTotalQuantity = existingQuantity + quantityToAdd;
            inventoryController.SetQuantity(key, newTotalQuantity);

            for (int i = 0; i < quantityToAdd; i++)
                inventoryController.SetItemWeight(key, itemProperty.weight);

            ItemView existingItem = inventoryController.GetInstantiatedItem(key);

            if (existingItem != null)
                existingItem.InventoryDisplayUI(newTotalQuantity);
        }
        else
        {
            ItemView itemView = CreateItemView(itemProperty);
            if (itemView != null)
            {
                inventoryController.StoreItem(itemView, inventoryFilterController);
                inventoryController.SetQuantity(key, quantityToAdd);

                for (int i = 0; i < quantityToAdd; i++)
                    inventoryController.SetItemWeight(key, itemProperty.weight);

                inventoryController.StoreInstantiatedItem(key, itemView);
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

        if (gatherResourceButton != null)
            gatherResourceButton.interactable = false;
    }

    public void Sell()
    {
        int amount = int.Parse(sellSectionController.GetPriceText());
        int sellQuantity = int.Parse(sellSectionController.GetQuantityText());
        int itemID = inventoryController.GetCurrentItem().itemProperty.GetInstanceID();

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

        if (gatherResourceButton != null)
            gatherResourceButton.interactable = true;
    }
}