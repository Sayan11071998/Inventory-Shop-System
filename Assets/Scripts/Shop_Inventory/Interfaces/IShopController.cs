using System.Collections.Generic;

public interface IShopController
{
    void LoadShopItems();
    void EnableShopVisibility();
    void DisableShopVisibility();
    void StoreItem(ItemView itemDisplay, FilterController filterController);
    ItemView GetCurrentItem();
    bool ISShopOn();
    void DisplayBroughtItems(ItemView itemView, int quantity);
    void PlayBroughtSound();
    void PlayQuantityChangedSound();
    void PlayPopUpSound();
    void PlayNonClickableSound();
    int GetItemQuantity(int itemID);
    float GetPlayerBagWeight();
    float GetPlayerBagCapacity();
    int GetPlayerCoin();
    float GetItemWeight(int itemID);
    void SetItemQuantities(int itemID, int quantity);
    void SetCurrentSelectedItem(ItemView itemView);
    void SetItemWeight(int itemID, float newWeight);

    // NEW: Increase the shop's available quantity when an item is sold.
    void IncreaseItemQuantity(int itemID, int soldQuantity);

    void UpdateItemQuantityUI(int itemID);
}