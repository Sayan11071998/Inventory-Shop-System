using System.Collections.Generic;
using System.Linq;

public class InventoryModel : BaseItemModel
{
    public ItemView currentItem;
    public int numberOfResource { get; private set; }
    public Dictionary<ItemProperty.Rarity, bool> isRarityAvailable;
    public Dictionary<int, List<float>> itemWeight;
    private Dictionary<int, ItemView> instantiatedItems;
    public Dictionary<int, List<int>> itemQuantities;

    public InventoryModel(ItemDatabase _itemDatabase) : base(_itemDatabase)
    {
        Initialize();
    }

    private void Initialize()
    {
        numberOfResource = 5;
        instantiatedItems = new Dictionary<int, ItemView>();
        itemQuantities = new Dictionary<int, List<int>>();
        itemWeight = new Dictionary<int, List<float>>();

        foreach (ItemProperty item in itemDatabase.items)
        {
            itemQuantities[item.itemID] = new List<int>();
            itemWeight[item.itemID] = new List<float>();
        }

        isRarityAvailable = new Dictionary<ItemProperty.Rarity, bool> {
            { ItemProperty.Rarity.VeryCommon, true },
            { ItemProperty.Rarity.Common, false },
            { ItemProperty.Rarity.Rare, false },
            { ItemProperty.Rarity.Epic, false },
            { ItemProperty.Rarity.Legendary, false }
        };
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

    public bool IsRarityAvailable(ItemProperty.Rarity rarity) => isRarityAvailable[rarity];

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
            // Remove as many weight entries as the sold quantity.
            for (int i = 0; i < quantity && itemWeight[itemID].Count > 0; i++)
            {
                itemWeight[itemID].RemoveAt(0);
            }
        }
    }
}