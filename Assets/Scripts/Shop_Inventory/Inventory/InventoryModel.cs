using System.Collections.Generic;
using UnityEngine;

public class InventoryModel : BaseItemModel
{
    public ItemView currentItem;
    public int numberOfResource { get; private set; }
    public Dictionary<ItemRarity, bool> isRarityAvailable;
    public Dictionary<int, float> itemWeight;
    private Dictionary<int, ItemView> instantiatedItems;
    public Dictionary<int, int> itemQuantities;

    public InventoryModel(ItemDatabase _itemDatabase) : base(_itemDatabase)
    {
        Initialize();
    }

    private void Initialize()
    {
        numberOfResource = 5;
        instantiatedItems = new Dictionary<int, ItemView>();
        itemQuantities = new Dictionary<int, int>();
        itemWeight = new Dictionary<int, float>();

        foreach (ItemProperty item in itemDatabase.items)
        {
            itemQuantities[item.itemID] = 0;
            itemWeight[item.itemID] = 0f;
        }

        isRarityAvailable = new Dictionary<ItemRarity, bool> {
            { ItemRarity.VeryCommon, true },
            { ItemRarity.Common, false },
            { ItemRarity.Rare, false },
            { ItemRarity.Epic, false },
            { ItemRarity.Legendary, false }
        };
    }

    public void SetItemQuantities(int itemID, int newQuantity)
    {
        if (itemQuantities.ContainsKey(itemID))
            itemQuantities[itemID] = newQuantity;
    }


    public void ResetQuantities(int itemID)
    {
        if (itemQuantities.ContainsKey(itemID))
            itemQuantities[itemID] = 0;
    }

    public int GetQuantity(int itemID)
    {
        if (itemQuantities.ContainsKey(itemID))
            return itemQuantities[itemID];
        return 0;
    }

    public void StoreInstantiatedItems(int itemID, ItemView itemView) => instantiatedItems[itemID] = itemView;
    public Dictionary<int, ItemView> GetInstatiatedItems() => instantiatedItems;

    public void SetRarityAvailable(ItemRarity rarity, bool value)
    {
        if (isRarityAvailable.ContainsKey(rarity))
            isRarityAvailable[rarity] = value;
    }

    public bool IsRarityAvailable(ItemRarity rarity) => isRarityAvailable[rarity];

    public void RemoveInstatiatedItem(int itemID)
    {
        if (instantiatedItems.ContainsKey(itemID))
            instantiatedItems.Remove(itemID);
    }

    public void SetItemWeight(int itemID, float newWeight)
    {
        if (!itemWeight.ContainsKey(itemID))
            itemWeight[itemID] = newWeight;
    }

    public float GetItemWeight(int itemID)
    {
        if (itemWeight.ContainsKey(itemID))
            return itemWeight[itemID];
        return 0f;
    }

    public void RemoveWeight(int itemID, int quantity)
    {
        if (itemWeight.ContainsKey(itemID))
        {
            float unitWeight = 0f;
            if (instantiatedItems.ContainsKey(itemID))
                unitWeight = instantiatedItems[itemID].itemProperty.weight;
            float removalAmount = quantity * unitWeight;
            itemWeight[itemID] = Mathf.Max(0f, itemWeight[itemID] - removalAmount);
        }
    }
}