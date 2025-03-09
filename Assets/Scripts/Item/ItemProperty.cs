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
    public ItemRarity rarity;
    public int quantity;
}