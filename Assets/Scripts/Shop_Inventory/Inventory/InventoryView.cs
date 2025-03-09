using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryView : BaseItemListView
{
    private InventoryController inventoryController;
    [SerializeField] private FilterController inventoryFilterController;
    [SerializeField] private CanvasGroup weightExceededPopup;

    [Header("Sell Section")]
    [SerializeField] private CanvasGroup sellSection;
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
        sellSectionController.GetAvailableQuantity = () =>
            inventoryController.GetItemQuantity(inventoryController.GetCurrentItem().itemProperty.itemID);
        sellSectionController.GetUnitPrice = () =>
            inventoryController.GetCurrentItem().itemProperty.sellingPrice;
        sellSectionController.PlayQuantityChangedSound = () =>
            EventService.Instance.OnQuantityChanged.InvokeEvent();
        sellSectionController.PlayNonClickableSound = () =>
            EventService.Instance.OnNonClickableButtonPressed.InvokeEvent();
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

    // public void GatherResource()
    // {
    //     if (inventoryController.GetPlayerBagWeight() < inventoryController.GetPlayerBagCapacity())
    //     {
    //         inventoryController.DisablePanel();
    //         inventoryController.GatherResource();
    //         inventoryController.SetBagWeight(inventoryController.GetTotalWeight());
    //     }
    //     else
    //     {
    //         EventService.Instance.OnMaximumWeightExceed.InvokeEvent();
    //         weightExceededPopup.alpha = 1;
    //         weightExceededPopup.blocksRaycasts = true;
    //         weightExceededPopup.interactable = true;
    //     }
    // }

    public void GatherResource()
    {
        // Use the inventory's total weight for checking capacity.
        if (inventoryController.GetTotalWeight() < inventoryController.GetPlayerBagCapacity())
        {
            inventoryController.DisablePanel();
            inventoryController.GatherResource();
            // Immediately update the player's bag weight based on the current total.
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
        // Here we assume that the item is already instantiated in inventory.
        InstantiateOrUpdateItem(itemView.itemProperty, newQuantity);
        inventoryController.SetBagWeight(inventoryController.GetTotalWeight());
    }

    /// <summary>
    /// Checks if the item already exists. If yes, update its UI; otherwise, instantiate a new one.
    /// </summary>
    // private void InstantiateOrUpdateItem(ItemProperty itemProperty, int newQuantity)
    // {
    //     int itemID = itemProperty.itemID;
    //     if (inventoryController.IsItemAlreadyInstantiated(itemID))
    //     {
    //         inventoryController.SetQuantity(itemID, newQuantity);
    //         ItemView existingItem = inventoryController.GetInstantiatedItem(itemID);
    //         inventoryController.SetItemWeight(itemID, existingItem.itemProperty.weight);
    //         if (existingItem != null)
    //         {
    //             int totalQuantity = inventoryController.GetItemQuantity(existingItem.itemProperty.itemID);
    //             existingItem.InventoryDisplayUI(totalQuantity);
    //         }
    //     }
    //     else
    //     {
    //         // Use the base method from BaseItemListView
    //         ItemView itemView = CreateItemView(itemProperty);
    //         if (itemView != null)
    //         {
    //             inventoryController.StoreItem(itemView, inventoryFilterController);
    //             inventoryController.SetQuantity(itemView.itemProperty.itemID, newQuantity);
    //             inventoryController.SetItemWeight(itemID, itemView.itemProperty.weight);
    //             inventoryController.StoreInstantiatedItem(itemView.itemProperty.itemID, itemView);
    //             itemView.InventoryDisplayUI(inventoryController.GetItemQuantity(itemView.itemProperty.itemID));
    //         }
    //     }
    //     inventoryController.ApplyFilter(inventoryFilterController);
    //     EventSystem.current.SetSelectedGameObject(null);
    // }

    private void InstantiateOrUpdateItem(ItemProperty itemProperty, int quantityToAdd)
    {
        int itemID = itemProperty.itemID;
        if (inventoryController.IsItemAlreadyInstantiated(itemID))
        {
            // Get existing quantity and calculate new total.
            int existingQuantity = inventoryController.GetItemQuantity(itemID);
            int newTotalQuantity = existingQuantity + quantityToAdd;
            inventoryController.SetQuantity(itemID, newTotalQuantity);

            // Add weight for each newly purchased unit.
            for (int i = 0; i < quantityToAdd; i++)
            {
                inventoryController.SetItemWeight(itemID, itemProperty.weight);
            }

            // Update the UI for the existing item.
            ItemView existingItem = inventoryController.GetInstantiatedItem(itemID);
            if (existingItem != null)
            {
                existingItem.InventoryDisplayUI(newTotalQuantity);
            }
        }
        else
        {
            // Instantiate a new item.
            ItemView itemView = CreateItemView(itemProperty);
            if (itemView != null)
            {
                inventoryController.StoreItem(itemView, inventoryFilterController);
                // Set quantity for the new item.
                inventoryController.SetQuantity(itemID, quantityToAdd);

                // Add weight entries for each unit purchased.
                for (int i = 0; i < quantityToAdd; i++)
                {
                    inventoryController.SetItemWeight(itemID, itemProperty.weight);
                }

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


    public void Sell()
    {
        int amount = int.Parse(sellSectionController.GetPriceText());
        int sellQuantity = int.Parse(sellSectionController.GetQuantityText()); // capture sell quantity
        int itemID = inventoryController.GetCurrentItem().itemProperty.itemID;

        if (amount > 0 && sellQuantity > 0)
        {
            sellSectionController.ResetSection();
            inventoryController.RemoveWeight(itemID, sellQuantity);

            // Subtract sold quantity from inventory.
            int newInventoryQuantity = inventoryController.GetItemQuantity(itemID) - sellQuantity;
            inventoryController.ResetQuantities(itemID);
            inventoryController.SetQuantity(itemID, newInventoryQuantity);
            inventoryController.GetCurrentItem().SetQuantityText(newInventoryQuantity);

            // Increase the shop's stock.
            GameManager.Instance.shopController.IncreaseItemQuantity(itemID, sellQuantity);
            // Update the shop UI.
            GameManager.Instance.shopController.UpdateItemQuantityUI(itemID);

            EventService.Instance.onItemChanged.InvokeEvent();
            EventService.Instance.onItemSoldWithIntParams.InvokeEvent(amount);
            EventService.Instance.onItemSoldWithFloatParams.InvokeEvent(inventoryController.GetTotalWeight());

            if (newInventoryQuantity <= 0)
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