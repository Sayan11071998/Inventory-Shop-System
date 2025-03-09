public interface IShopController
{
    void LoadShopItems();

    void EnableShopVisibility();
    void DisableShopVisibility();

    void StoreItem(ItemView itemDisplay, FilterController filterController);
    bool ISShopOn();
    void DisplayBroughtItems(ItemView itemView, int quantity);

    void IncreaseItemQuantity(int itemID, int soldQuantity);
    void UpdateItemQuantityUI(int itemID);

    void PlayBroughtSound();
    void PlayQuantityChangedSound();
    void PlayPopUpSound();
    void PlayNonClickableSound();

    ItemView GetCurrentItem();
    int GetItemQuantity(int itemID);
    float GetPlayerBagWeight();
    float GetPlayerBagCapacity();
    int GetPlayerCoin();
    float GetItemWeight(int itemID);

    void SetItemQuantities(int itemID, int quantity);
    void SetCurrentSelectedItem(ItemView itemView);
    void SetItemWeight(int itemID, float newWeight);
}