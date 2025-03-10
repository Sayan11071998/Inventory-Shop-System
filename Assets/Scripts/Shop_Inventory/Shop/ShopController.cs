using System.Collections.Generic;
using UnityEngine;

public class ShopController : BaseController<ShopView, ShopModel>
{
    public ShopController(ShopView _shopView, ShopModel _shopModel)
        : base(_shopView, _shopModel)
    {
        view.SetShopController(this);
        LoadShopItems();
    }

    public void LoadShopItems()
    {
        List<ItemProperty> items = model.GetItemDatabase();
        view.DisplayItems(items);
    }

    public void EnableShopVisibility() => view.EnableShopVisibility();
    public void DisableShopVisibility() => view.DisableShopVisibility();
    public void StoreItem(ItemView itemDisplay, FilterController shopFilterController) => shopFilterController.AddItemDisplay(itemDisplay);

    public ItemView GetCurrentItem()
    {
        if (model.currentItem == null)
        {
            Debug.LogError("GetCurrentItem: currentItem is NULL!");
            return null;
        }

        return model.currentItem;
    }

    public bool ISShopOn() => view.isShopOn;

    public void DisplayBroughtItems(ItemView itemView, int quantity)
    {
        GameManager.Instance.inventoryController.DisplayBroughtItems(GetCurrentItem(), quantity);
        EventService.Instance.OnItemSoldWithIntParams?.InvokeEvent(quantity);
    }

    public void UpdateItemQuantityUI(int itemID)
    {
        int updatedQuantity = model.GetQuantity(itemID);
        view.UpdateItemQuantityUI(itemID, updatedQuantity);
    }

    public void IncreaseItemQuantity(int itemID, int soldQuantity) => model.IncreaseItemQuantity(itemID, soldQuantity);

    public void SetCurrentSelectedItem(ItemView itemView) => model.currentItem = itemView;
}