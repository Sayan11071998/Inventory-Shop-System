using System.Collections.Generic;

public class ShopModel : BaseItemModel
{
    public ItemView currentItem;
    public Dictionary<int, int> itemQuantities;
    public Dictionary<int, float> itemWeight;

    public ShopModel(ItemDatabase _itemDatabase) : base(_itemDatabase)
    {
        Initialize();
    }

    private void Initialize()
    {
        itemQuantities = new Dictionary<int, int>();
        itemWeight = new Dictionary<int, float>();

        foreach (ItemProperty item in itemDatabase.items)
        {
            itemQuantities[item.itemID] = 0;
            itemWeight[item.itemID] = 0f;
        }
    }

    public void SetItemQuantities(int itemID, int quantity)
    {
        if (itemQuantities.ContainsKey(itemID))
            itemQuantities[itemID] = quantity;
    }

    public int GetQuantity(int itemID)
    {
        if (itemQuantities.ContainsKey(itemID))
            return itemQuantities[itemID];
        return 0;
    }

    public void SetItemWeight(int itemID, float newWeight)
    {
        if (itemWeight.ContainsKey(itemID))
            itemWeight[itemID] = newWeight;
    }

    public float GetItemWeight(int itemID)
    {
        if (itemWeight.ContainsKey(itemID))
            return itemWeight[itemID];
        return 0f;
    }
}