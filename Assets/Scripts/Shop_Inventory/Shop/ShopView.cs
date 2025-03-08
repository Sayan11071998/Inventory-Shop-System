using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopView : BaseView
{
    private ShopController shopController;
    [SerializeField] private FilterController shopFilterController;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform parentPanel;

    [Header("Buy Section")]
    [SerializeField] private CanvasGroup buySection;
    // Use TransactionSectionController for the buy section.
    [SerializeField] private TransactionSectionController buySectionController;
    [SerializeField] private CanvasGroup notEnoughMoneyPopup;
    [SerializeField] private CanvasGroup weightExceededPopUp;

    public bool isShopOn = true;

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

        // Set up the TransactionSectionController delegates.
        buySectionController.GetAvailableQuantity = () => shopController.GetItemQuantity(shopController.GetCurrentItem().itemProperty.itemID);
        buySectionController.GetUnitPrice = () => shopController.GetCurrentItem().itemProperty.buyingPrice;
        buySectionController.PlayQuantityChangedSound = () => shopController.PlayQuantityChangedSound();
        buySectionController.PlayNonClickableSound = () => shopController.PlayNonClickableSound();
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
            GameObject newItem = Instantiate(itemPrefab, parentPanel);
            ItemView itemDisplay = newItem.GetComponent<ItemView>();
            shopController.StoreItem(itemDisplay, shopFilterController);
            if (itemDisplay != null)
            {
                itemDisplay.itemProperty = item;
                shopController.SetItemQuantities(itemDisplay.itemProperty.itemID, itemDisplay.itemProperty.quantity);
                itemDisplay.ShopDisplayUI();
            }
        }
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
                if (shopController.GetPlayerBagWeight() < shopController.GetPlayerBagCapacity())
                {
                    buySectionController.ResetSection();
                    int playerCoin = shopController.GetPlayerCoin();
                    int newAmount = playerCoin - amount;
                    int newQuantity = shopController.GetItemQuantity(itemID) - selectedQuantity;
                    shopController.DisplayBroughtItems(shopController.GetCurrentItem(), selectedQuantity);
                    shopController.SetItemQuantities(itemID, newQuantity);
                    shopController.GetCurrentItem().SetQuantityText(newQuantity);
                    EventService.Instance.onItemChanged.InvokeEvent();
                    EventService.Instance.onItemBroughtWithIntParams.InvokeEvent(newAmount);
                }
                else
                {
                    shopController.PlayPopUpSound();
                    weightExceededPopUp.alpha = 1;
                    weightExceededPopUp.blocksRaycasts = true;
                    weightExceededPopUp.interactable = true;
                }
            }
            else
            {
                shopController.PlayPopUpSound();
                notEnoughMoneyPopup.alpha = 1;
                notEnoughMoneyPopup.blocksRaycasts = true;
                notEnoughMoneyPopup.interactable = true;
            }
        }
        else
        {
            shopController.PlayNonClickableSound();
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