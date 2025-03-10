using System.Collections.Generic;
using UnityEngine;

public class InventoryModel : BaseModel
{
    public Dictionary<ItemRarity, bool> isRarityAvailable;
    private Dictionary<int, ItemView> instantiatedItems;

    public InventoryModel(ItemDatabase _itemDatabase) : base(_itemDatabase)
    {
        Initialize();
    }

    private void Initialize()
    {
        instantiatedItems = new Dictionary<int, ItemView>();

        isRarityAvailable = new Dictionary<ItemRarity, bool> {
            { ItemRarity.VeryCommon, true },
            { ItemRarity.Common, true },
            { ItemRarity.Rare, true },
            { ItemRarity.Epic, false },
            { ItemRarity.Legendary, false }
        };
    }

    public void StoreInstantiatedItems(int itemID, ItemView itemView) => instantiatedItems[itemID] = itemView;
    public Dictionary<int, ItemView> GetInstatiatedItems() => instantiatedItems;

    public void ResetQuantities(int itemID)
    {
        if (itemQuantities.ContainsKey(itemID))
            itemQuantities[itemID] = 0;
    }

    public void RemoveInstatiatedItem(int itemID)
    {
        if (instantiatedItems.ContainsKey(itemID))
            instantiatedItems.Remove(itemID);
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

    public void SetRarityAvailable(ItemRarity rarity, bool value)
    {
        if (isRarityAvailable.ContainsKey(rarity))
            isRarityAvailable[rarity] = value;
    }

    public bool IsRarityAvailable(ItemRarity rarity) => isRarityAvailable[rarity];
}