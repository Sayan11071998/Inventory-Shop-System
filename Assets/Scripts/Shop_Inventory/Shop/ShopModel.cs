using System.Collections.Generic;

public class ShopModel : BaseModel
{
    public ShopModel(ItemDatabase _itemDatabase) : base(_itemDatabase) { }

    public void IncreaseItemQuantity(int itemID, int soldQuantity)
    {
        if (itemQuantities.ContainsKey(itemID))
            itemQuantities[itemID] += soldQuantity;
        else
            itemQuantities[itemID] = soldQuantity;
    }
}