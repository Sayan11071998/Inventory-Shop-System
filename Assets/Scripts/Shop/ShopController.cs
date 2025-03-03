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

    public void EnableShopVisibility()
    {
        shopView.EnableShopVisibility();
    }

    public void DisableShopVisibility()
    {
        shopView.DisableShopVisibility();
    }

    public void LoadShopItems()
    {
        List<ItemProperty> items = shopModel.GetItemDatabase();
        shopView.DisplayItems(items);
    }

    public void StoreItem(ItemView itemDisplay, FilterController shopFilterController)
    {
        shopFilterController.AddItemDisplay(itemDisplay);
    }

    public void SetItemQuantities(int itemID,int quantity )
    {
        shopModel.SetItemQuantities(itemID, quantity);
    }

    public void SetCurrentSelectedItem(ItemView itemView)
    {
        shopModel.currentItem = itemView;
    }

    public ItemView GetCurrentItem()
    {
        if (shopModel.currentItem == null)
        {
            Debug.LogError("GetCurrentItem: currentItem is NULL!");
            return null;
        }

        return shopModel.currentItem;
    }

    public int GetItemQuantity(int itemID)
    {
        return shopModel.GetQuantity(itemID);
    }

    public bool ISShopOn()
    {
        return shopView.isShopOn;
    }

    public float GetPlayerBagWeight()
    {
        return GameManager.Instance.playerController.GetBagWeight();
    }

    public float GetPlayerBagCapacity()
    {
        return GameManager.Instance.playerController.GetBagCapacity();
    }

    public int GetPlayerCoin()
    {
        return GameManager.Instance.playerController.GetPlayerCoinCount();
    }

    public void DisplayBroughtItems(ItemView itemView, int quantity)
    { 
        GameManager.Instance.inventoryController.DisplayBroughtItems(GetCurrentItem(), quantity);
        PlayBroughtSound();
    }

    public void PlayBroughtSound()
    {
        SoundManager.Instance.PlaySound(Sounds.MoneySound);
    }

    public void SetItemWeight(int itemID, float newWeight)
    {
        shopModel.SetItemWeight(itemID, newWeight);
    }

    public float GetItemWeight(int itemID)
    {
        return shopModel.GetItemWeight(itemID);
    }

    public void PlayQuantityChangedSound()
    {
        SoundManager.Instance.PlaySound(Sounds.QuantityChanged);
    }
    public void PlayPopUpSound()
    {
        SoundManager.Instance.PlaySound(Sounds.ErrorSound);
    }
    public void PlayNonClickableSound()
    {
        SoundManager.Instance.PlaySound(Sounds.NonClickable);
    }
}
