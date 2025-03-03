using System.Collections.Generic;

public class InventoryModel
{
    public ItemView currentItem;
    public int numberOfResource { get; private set; }

    public Dictionary<ItemProperty.Rarity, bool> isRarityAvailable;
    public Dictionary<int, List<float>> itemWeight;

    private ItemDatabase itemDatabase;
    private List<ItemProperty> Items;

    private Dictionary<int, ItemView> instantiatedItems;
    public Dictionary<int, List<int>> itemQuantities;

    public InventoryModel(ItemDatabase itemDatabase) => Initialize(itemDatabase);

    private void Initialize(ItemDatabase itemDatabase)
    {
        this.itemDatabase = itemDatabase;
        Items = new List<ItemProperty>();
        numberOfResource = 5;

        InitializeInstantiatedItems();
        InitializeItemQuantities(itemDatabase);
        InitializeItemWeight(itemDatabase);
        InitializeRairityValues();
    }

    private void InitializeInstantiatedItems() => instantiatedItems = new Dictionary<int, ItemView>();

    private void InitializeItemQuantities(ItemDatabase itemDatabase)
    {
        itemQuantities = new Dictionary<int, List<int>>();

        foreach (ItemProperty item in itemDatabase.items)
            itemQuantities[item.itemID] = new List<int>();
    }

    private void InitializeItemWeight(ItemDatabase itemDatabase)
    {
        itemWeight = new Dictionary<int, List<float>>();

        foreach (ItemProperty item in itemDatabase.items)
            itemWeight[item.itemID] = new List<float>();
    }

    private void InitializeRairityValues()
    {
        isRarityAvailable = new Dictionary<ItemProperty.Rarity, bool>();

        isRarityAvailable[ItemProperty.Rarity.VeryCommon] = true;
        isRarityAvailable[ItemProperty.Rarity.Common] = false;
        isRarityAvailable[ItemProperty.Rarity.Legendary] = false;
        isRarityAvailable[ItemProperty.Rarity.Epic] = false;
        isRarityAvailable[ItemProperty.Rarity.Rare] = false;
    }

    public List<ItemProperty> getItemDatabase()
    {
        if (Items.Count == 0)
            Items.AddRange(itemDatabase.items);

        return Items;
    }

    public void SetItemQuantities(int itemID, int newQuantity)
    {
        if (!itemQuantities.ContainsKey(itemID))
            itemQuantities[itemID] = new List<int>();
        itemQuantities[itemID].Add(newQuantity);
    }

    public void ResetQuantities(int itemID)
    {
        if (itemQuantities.ContainsKey(itemID))
            itemQuantities[itemID].Clear();
    }

    public List<int> GetQuantity(int itemID)
    {
        if (itemQuantities.ContainsKey(itemID))
            return itemQuantities[itemID];

        return new List<int>();
    }

    public void StoreInstantiatedItems(int itemID, ItemView itemView) => instantiatedItems[itemID] = itemView;
    public Dictionary<int, ItemView> GetInstatiatedItems() => instantiatedItems;

    public void SetRarityAvailable(ItemProperty.Rarity rarity, bool value)
    {
        if (isRarityAvailable.ContainsKey(rarity))
            isRarityAvailable[rarity] = value;
    }

    public bool IsRarityAvailable(ItemProperty.Rarity rairity) => isRarityAvailable[rairity];

    public void RemoveInstatiatedItem(int itemID)
    {
        if (instantiatedItems.ContainsKey(itemID))
            instantiatedItems.Remove(itemID);
    }

    public void SetItemWeight(int itemID, float newWeight)
    {
        if (!itemWeight.ContainsKey(itemID))
            itemWeight[itemID] = new List<float>();

        itemWeight[itemID].Add(newWeight);
    }

    public List<float> GetItemWeight(int itemID)
    {
        if (itemWeight.ContainsKey(itemID))
            return itemWeight[itemID];

        return new List<float>();
    }

    public void RemoveWeight(int itemID, int quantity)
    {
        if (itemWeight.ContainsKey(itemID))
        {
            for (int i = 1; i < quantity; i++)
                itemWeight.Remove(itemID);
        }
    }
}