using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] public TextMeshProUGUI quantity;
    [SerializeField] private Toggle itemToggle;

    public ItemProperty itemProperty;

    private ToggleGroup itemToggleGroup;
    public int quantityValue = 0;

    // public ItemProperty.ItemTypes itemType { get; private set; }
    // public ItemProperty.Rarity rarity { get; private set; }

    public ItemTypes itemType { get; private set; }
    public ItemRarity rarity { get; private set; }
    
    public void ShopDisplayUI()
    {
        SetValues();
        SetQuantityText(itemProperty.quantity);
    }

    public void InventoryDisplayUI(int quantityValue)
    {
        SetValues();
        SetQuantityText(quantityValue);
    }

    public void SetValues()
    {
        itemToggleGroup = GetComponentInParent<ToggleGroup>();
        itemToggle.group = itemToggleGroup;
        itemImage.sprite = itemProperty.itemIcon;

        itemType = itemProperty.item;
        rarity = itemProperty.rarity;
    }

    public void SetQuantityText(int quantity)
    {
        quantityValue = quantity;
        this.quantity.text = quantity.ToString();
    }


    public void SetItemDetailPanel(bool isOn)
    {
        if (isOn)
        {
            SoundManager.Instance.PlaySound(Sounds.ItemSelected);
            EventService.Instance.OnItemSelectedEventWithParams.InvokeEvent(isOn, this);
        }
    }

    public void disableItem()
    {
        if (this.gameObject != null)
            this.gameObject.SetActive(false);
    }

    public void EnabaleItem()
    {
        if (this.gameObject != null)
            this.gameObject.SetActive(true);
    }
}