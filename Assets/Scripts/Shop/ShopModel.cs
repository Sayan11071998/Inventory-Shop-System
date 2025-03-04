using System.Collections.Generic;

public class ShopModel
{
    private ItemDatabase itemDatabase;
    public ItemView currentItem;

    private List<ItemProperty> items;
    public Dictionary<int, int> itemQuantities;
    public Dictionary<int, float> itemWeight;

    public ShopModel(ItemDatabase itemDatabase) => Initilize(itemDatabase);

    private void Initilize(ItemDatabase _itemDatabase)
    {
        itemDatabase = _itemDatabase;
        items = new List<ItemProperty>();

        InitializeItemQuantities(itemDatabase);
        InitializeItemWeight(itemDatabase);
    }

    private void InitializeItemQuantities(ItemDatabase itemDatabase)
    {
        itemQuantities = new Dictionary<int, int>();

        foreach (ItemProperty item in itemDatabase.items)
            itemQuantities[item.itemID] = 0;
    }

    public List<ItemProperty> GetItemDatabase()
    {
        if (items.Count == 0)
            items.AddRange(itemDatabase.items);

        return items;
    }

    private void InitializeItemWeight(ItemDatabase itemDatabase)
    {
        itemWeight = new Dictionary<int, float>();

        foreach (ItemProperty item in itemDatabase.items)
            itemWeight[item.itemID] = 0;
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
        return 0;
    }
}