using System.Collections.Generic;
using UnityEngine;

public class ShopView : BaseItemListView, IItemListView
{
    [SerializeField] private FilterController shopFilterController;

    [Header("Buy Section")]
    [SerializeField] private CanvasGroup buySection;
    [SerializeField] private TransactionSectionController buySectionController;
    [SerializeField] private CanvasGroup notEnoughMoneyPopup;
    [SerializeField] private CanvasGroup weightExceededPopUp;

    public bool isShopOn = true;

    private ShopController shopController;
    private Dictionary<int, ItemView> itemViews = new Dictionary<int, ItemView>();

    private void OnEnable()
    {
        EventService.Instance.OnInventoryToggledOnEvent.AddListener(DisableShopVisibility);
        EventService.Instance.OnInventoryToggledOnEvent.AddListener(DisableBuyingSection);
        EventService.Instance.OnShopToggledOnEvent.AddListener(EnableShopVisibility);
        EventService.Instance.OnItemSelectedEvent.AddListener(EnableBuyingSection);
        EventService.Instance.OnItemSelectedEventWithParams.AddListener(SetCurrentSelected);
    }

    private void OnDisable()
    {
        EventService.Instance.OnInventoryToggledOnEvent.RemoveListener(DisableShopVisibility);
        EventService.Instance.OnInventoryToggledOnEvent.RemoveListener(DisableBuyingSection);
        EventService.Instance.OnShopToggledOnEvent.RemoveListener(EnableShopVisibility);
        EventService.Instance.OnItemSelectedEvent.RemoveListener(EnableBuyingSection);
        EventService.Instance.OnItemSelectedEventWithParams.RemoveListener(SetCurrentSelected);
    }

    public void SetShopController(ShopController controller)
    {
        shopController = controller;
        buySectionController.GetAvailableQuantity = () => shopController.GetItemQuantity(shopController.GetCurrentItem().itemProperty.itemID);
        buySectionController.GetUnitPrice = () => shopController.GetCurrentItem().itemProperty.buyingPrice;
        buySectionController.PlayNonClickableSound = () => EventService.Instance.OnNonClickableButtonPressed.InvokeEvent();
    }

    public void EnableShopVisibility()
    {
        isShopOn = true;
        EnableVisibility();
    }

    public void DisableShopVisibility()
    {
        isShopOn = false;
        DisableVisibility();
    }

    public void DisplayItems(List<ItemProperty> items)
    {
        foreach (ItemProperty item in items)
        {
            ItemView itemDisplay = CreateItemView(item);
            if (itemDisplay != null)
            {
                itemViews[item.itemID] = itemDisplay;
                shopController.SetItemQuantities(itemDisplay.itemProperty.itemID, itemDisplay.itemProperty.quantity);
                itemDisplay.ShopDisplayUI();
            }
            shopController.StoreItem(itemDisplay, shopFilterController);
        }
    }

    public void UpdateItemQuantityUI(int itemID, int newQuantity)
    {
        if (itemViews.ContainsKey(itemID))
            itemViews[itemID].SetQuantityText(newQuantity);
    }

    public void EnableBuyingSection()
    {
        if (isShopOn)
        {
            buySection.alpha = 1;
            buySection.interactable = true;
            buySection.blocksRaycasts = true;
            buySectionController.ResetSection();
        }
    }

    public void SetCurrentSelected(bool isOn, ItemView itemView)
    {
        shopController.SetCurrentSelectedItem(itemView);
        buySectionController.ResetSection();
    }

    public void DisableBuyingSection()
    {
        if (!isShopOn)
        {
            buySection.alpha = 0;
            buySection.interactable = false;
            buySection.blocksRaycasts = false;
        }
    }

    public void Buy()
    {
        int amount = int.Parse(buySectionController.GetPriceText());
        int selectedQuantity = int.Parse(buySectionController.GetQuantityText());
        int itemID = shopController.GetCurrentItem().itemProperty.itemID;

        if (amount > 0 && selectedQuantity > 0)
        {
            if (shopController.GetPlayerCoin() >= amount)
            {
                float currentInventoryWeight = GameManager.Instance.inventoryController.GetTotalWeight();
                float additionalWeight = selectedQuantity * shopController.GetCurrentItem().itemProperty.weight;
                float newTotalWeight = currentInventoryWeight + additionalWeight;

                if (newTotalWeight <= GameManager.Instance.inventoryController.GetPlayerBagCapacity())
                {
                    buySectionController.ResetSection();
                    int newQuantity = shopController.GetItemQuantity(itemID) - selectedQuantity;

                    shopController.DisplayBroughtItems(shopController.GetCurrentItem(), selectedQuantity);
                    shopController.SetItemQuantities(itemID, newQuantity);
                    shopController.GetCurrentItem().SetQuantityText(newQuantity);

                    EventService.Instance.OnItemChanged.InvokeEvent();
                    EventService.Instance.OnItemBroughtWithIntParams.InvokeEvent(amount);
                }
                else
                {
                    EventService.Instance.OnMaximumWeightExceed.InvokeEvent();
                    weightExceededPopUp.alpha = 1;
                    weightExceededPopUp.blocksRaycasts = true;
                    weightExceededPopUp.interactable = true;
                }
            }
            else
            {
                EventService.Instance.OnMaximumWeightExceed.InvokeEvent();
                notEnoughMoneyPopup.alpha = 1;
                notEnoughMoneyPopup.blocksRaycasts = true;
                notEnoughMoneyPopup.interactable = true;
            }
        }
        else
        {
            EventService.Instance.OnNonClickableButtonPressed.InvokeEvent();
        }
    }


    public void DisableNotEnoughMoneyPopUp()
    {
        notEnoughMoneyPopup.alpha = 0;
        notEnoughMoneyPopup.blocksRaycasts = false;
        notEnoughMoneyPopup.interactable = false;
    }

    public void DisableWeightExceededPopUp()
    {
        weightExceededPopUp.alpha = 0;
        weightExceededPopUp.blocksRaycasts = false;
        weightExceededPopUp.interactable = false;
    }
}