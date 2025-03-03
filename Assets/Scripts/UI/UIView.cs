using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{

    [SerializeField] private Toggle shopToggle;
    [SerializeField] private TextMeshProUGUI shoppOrInventoryText;
    [SerializeField] private CanvasGroup itemDeatilsPanelCanvasGroup;
    [SerializeField] private CanvasGroup sellSection;
    
    [Header("Item Properties")]
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemRarityText;
    [SerializeField] private TextMeshProUGUI itemWeightText;
    [SerializeField] private TextMeshProUGUI quantityAvailableText;
    [SerializeField] private TextMeshProUGUI itemBuyingPriceText;
    [SerializeField] private TextMeshProUGUI itemSellingPriceText;

    private UIController uiController;

    private void OnDisable() => EventService.Instance.OnItemSelectedEventWithParams.RemoveListener(uiController.SetItemDetailsPanel);

    public void OnShopToggleChanged(bool isOn)
    {
        uiController.OnShopToggleChanged(isOn);
        itemDeatilsPanelCanvasGroup.alpha = 0;
    }

    public void UpdateShopORInventoryText(bool isShopOpen)
    {
        shoppOrInventoryText.text = isShopOpen ? "Shop" : "Inventory";
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SetUIController(UIController uiController)
    {
        this.uiController = uiController;
        EventService.Instance.OnItemSelectedEventWithParams.AddListener(uiController.SetItemDetailsPanel);
    }

    public void SetItemDetailPanelView(bool isOn, ItemView itemView)
    {
        if (isOn == true)
        {
            itemDeatilsPanelCanvasGroup.alpha = 1;
            SetItemDetailPanelValues(itemView);
            EventService.Instance.OnItemSelectedEvent.InvokeEvent();
        }
    }

    public void SetItemDetailPanelValues(ItemView itemView)
    {
        this.itemName.text = itemView.itemProperty.name;
        this.itemImage.sprite = itemView.itemProperty.itemIcon;
        this.itemTypeText.text = FormatEnumText(itemView.itemProperty.item);
        this.itemRarityText.text = FormatEnumText(itemView.itemProperty.rarity);
        this.itemWeightText.text = itemView.itemProperty.weight.ToString();
        this.quantityAvailableText.text = uiController.GetQuantity().ToString();
        this.itemDescriptionText.text = itemView.itemProperty.ItemDescription;
        this.itemBuyingPriceText.text = itemView.itemProperty.buyingPrice.ToString();
        this.itemSellingPriceText.text = itemView.itemProperty.sellingPrice.ToString();
    }

    public void DisableItemDetailsPanel() => itemDeatilsPanelCanvasGroup.alpha = 0;
    private string FormatEnumText(Enum enumValue) => System.Text.RegularExpressions.Regex.Replace(enumValue.ToString(), "(\\B[A-Z])", " $1");
}