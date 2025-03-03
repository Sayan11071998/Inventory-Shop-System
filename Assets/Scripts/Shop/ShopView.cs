using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    private ShopController shopController;
    private CanvasGroup shopCanvas;
    [SerializeField] private FilterController shopFilterController;

    [SerializeField] private GameObject itemPrefab;

    [SerializeField] private Transform parentPanel;

    [Header("Buy Section")]
    [SerializeField] private CanvasGroup buySection;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TextMeshProUGUI buyingPriceText;
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

    public void SetShopController(ShopController shopController)
    {
        this.shopController = shopController;
        shopCanvas = this.GetComponent<CanvasGroup>();
    }

    public void EnableShopVisibility()
    {
        isShopOn = true;
        shopCanvas.alpha = 1;
        shopCanvas.interactable = true;
        shopCanvas.blocksRaycasts = true;
    }

    public void DisableShopVisibility()
    {
        isShopOn = false;
        shopCanvas.alpha = 0;
        shopCanvas.interactable = false;
        shopCanvas.blocksRaycasts = false;
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
        if (isShopOn == true)
        {
            buySection.alpha = 1;
            buySection.interactable = true;
            buySection.blocksRaycasts = true;
        }
    }

    public void SetCurrentSelected(bool isOn, ItemView itemView)
    {
        shopController.SetCurrentSelectedItem(itemView);
        SetBuySectionValues(isOn);
    }

    private void SetBuySectionValues(bool isOn)
    {
        if (isOn)
        {
            quantityText.text = 0.ToString();
            buyingPriceText.text = 0.ToString();
        }
    }

    public void AddBuySectionValues()
    {
        int itemID = shopController.GetCurrentItem().itemProperty.itemID;
        int AvailableQuantity = shopController.GetItemQuantity(itemID);
        int quantity = int.Parse(quantityText.text);
        int buyingPrice = int.Parse(buyingPriceText.text);

        if (quantity < AvailableQuantity)
        {
            shopController.PlayQuantityChangedSound();
            quantityText.text = (quantity + 1).ToString();
            buyingPriceText.text = (buyingPrice + shopController.GetCurrentItem().itemProperty.buyingPrice).ToString();
        }
        else
        {
            shopController.PlayNonClickableSound();
        }
    }

    public void ReduceBuySectionValues()
    {
        int itemID = shopController.GetCurrentItem().itemProperty.itemID;
        int AvailableQuantity = shopController.GetItemQuantity(itemID);
        int quantity = int.Parse(quantityText.text);
        int buyingPrice = int.Parse(buyingPriceText.text);

        if (quantity > 0)
        {
            shopController.PlayQuantityChangedSound();
            quantityText.text = (quantity - 1).ToString();
            buyingPriceText.text = (buyingPrice - shopController.GetCurrentItem().itemProperty.buyingPrice).ToString();
        }
        else
        {
            shopController.PlayNonClickableSound();
        }
    }

    public void DisableBuyingSection()
    {
        if (isShopOn == false)
        {
            buySection.alpha = 0;
            buySection.interactable = false;
            buySection.blocksRaycasts = false;
        }
    }
    public void Buy()
    {
        int amount = int.Parse(buyingPriceText.text);
        int seletcedQuantity = int.Parse(quantityText.text);

        int itemID = shopController.GetCurrentItem().itemProperty.itemID;

        if (amount > 0 && seletcedQuantity > 0)
        {
            if (shopController.GetPlayerCoin() >= amount)
            {
                if (shopController.GetPlayerBagWeight() < shopController.GetPlayerBagCapacity())
                {
                    SetBuySectionValues(true);
                    int playerCoin = shopController.GetPlayerCoin();
                    int newAmount = playerCoin - amount;
                    int newQuantity = shopController.GetItemQuantity(itemID) - seletcedQuantity;

                    shopController.DisplayBroughtItems(shopController.GetCurrentItem(), seletcedQuantity);
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
