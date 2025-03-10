using System.Collections.Generic;

public abstract class BaseModel
{
    public ItemView currentItem;

    protected ItemDatabase itemDatabase;
    protected List<ItemProperty> items;

    protected Dictionary<int, int> itemQuantities;
    protected Dictionary<int, float> itemWeight;

    public BaseModel(ItemDatabase _itemDatabase)
    {
        itemDatabase = _itemDatabase;
        items = new List<ItemProperty>();

        itemQuantities = new Dictionary<int, int>();
        itemWeight = new Dictionary<int, float>();

        foreach (ItemProperty item in itemDatabase.items)
        {
            itemQuantities[item.itemID] = 0;
            itemWeight[item.itemID] = 0f;
        }
    }

    public List<ItemProperty> GetItemDatabase()
    {
        if (items.Count == 0)
            items.AddRange(itemDatabase.items);
        return items;
    }

    public virtual int GetQuantity(int itemID)
    {
        if (itemQuantities.ContainsKey(itemID))
            return itemQuantities[itemID];
        return 0;
    }

    public virtual float GetItemWeight(int itemID)
    {
        if (itemWeight.ContainsKey(itemID))
            return itemWeight[itemID];
        return 0f;
    }

    public virtual void SetItemQuantities(int itemID, int newQuantity)
    {
        if (itemQuantities.ContainsKey(itemID))
            itemQuantities[itemID] = newQuantity;
    }

    public virtual void SetItemWeight(int itemID, float newWeight)
    {
        if (itemWeight.ContainsKey(itemID))
            itemWeight[itemID] = newWeight;
    }
}