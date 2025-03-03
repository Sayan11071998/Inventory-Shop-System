using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemProperty : ScriptableObject
{
    public int itemID;
    public string itemName;
    public ItemTypes item;
    public Sprite itemIcon;
    public string ItemDescription;
    public int buyingPrice;
    public int sellingPrice;
    public float weight;
    public Rarity rarity;
    public int quantity;

    [Serializable]
    public enum ItemTypes
    {
        Materials,
        Weapons,
        Consumables,
        Treasure
    }

    [Serializable]
    public enum Rarity
    {
        VeryCommon,
        Common,
        Rare,
        Epic,
        Legendary,
    }
}
