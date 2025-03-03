using System.Collections.Generic;
using UnityEngine;

public class ShopController
{
    private ShopView shopView;
    private ShopModel shopModel;
    FilterController filter;

    public ShopController(ShopView shopView, ShopModel shopModel)
    {
        this.shopModel = shopModel;
        this.shopView = shopView;

        this.shopView.SetShopController(this);
        LoadShopItems();
    }

    public void EnableShopVisibility() => shopView.EnableShopVisibility();
    public void DisableShopVisibility() => shopView.DisableShopVisibility();

    public void LoadShopItems()
    {
        List<ItemProperty> items = shopModel.GetItemDatabase();
        shopView.DisplayItems(items);
    }

    public void StoreItem(ItemView itemDisplay, FilterController shopFilterController) => shopFilterController.AddItemDisplay(itemDisplay);

    public ItemView GetCurrentItem()
    {
        if (shopModel.currentItem == null)
        {
            Debug.LogError("GetCurrentItem: currentItem is NULL!");
            return null;
        }

        return shopModel.currentItem;
    }

    public bool ISShopOn() => shopView.isShopOn;

    public void DisplayBroughtItems(ItemView itemView, int quantity)
    { 
        GameManager.Instance.inventoryController.DisplayBroughtItems(GetCurrentItem(), quantity);
        PlayBroughtSound();
    }

    public void PlayBroughtSound() => SoundManager.Instance.PlaySound(Sounds.MoneySound);
    public void PlayQuantityChangedSound() => SoundManager.Instance.PlaySound(Sounds.QuantityChanged);
    public void PlayPopUpSound() => SoundManager.Instance.PlaySound(Sounds.ErrorSound);
    public void PlayNonClickableSound() => SoundManager.Instance.PlaySound(Sounds.NonClickable);

    public int GetItemQuantity(int itemID) => shopModel.GetQuantity(itemID);
    public float GetPlayerBagWeight() => GameManager.Instance.playerController.GetBagWeight();
    public float GetPlayerBagCapacity() => GameManager.Instance.playerController.GetBagCapacity();
    public int GetPlayerCoin() => GameManager.Instance.playerController.GetPlayerCoinCount();
    public float GetItemWeight(int itemID) => shopModel.GetItemWeight(itemID);

    public void SetItemQuantities(int itemID,int quantity ) => shopModel.SetItemQuantities(itemID, quantity);
    public void SetCurrentSelectedItem(ItemView itemView) => shopModel.currentItem = itemView;
    public void SetItemWeight(int itemID, float newWeight) => shopModel.SetItemWeight(itemID, newWeight);
}