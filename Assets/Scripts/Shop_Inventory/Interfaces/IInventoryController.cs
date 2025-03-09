using System.Collections.Generic;

public interface IInventoryController
{
    void GatherResource();

    void EnableInventoryVisibility();
    void DisableInventoryVisibility();

    void DisablePanel();
    int GenerateRandomQuantity();
    void RemoveWeight(int itemID, int quantity);
    void ResetQuantities(int itemID);

    void StoreInstantiatedItem(int itemID, ItemView itemView);
    bool IsItemAlreadyInstantiated(int itemID);

    void ApplyFilter(FilterController filterController);
    void DisplayBroughtItems(ItemView itemView, int newQuantity);
    bool ISInventoryOn();
    void StoreItem(ItemView itemDisplay, FilterController filterController);

    int GetItemQuantity(int itemID);
    ItemView GetInstantiatedItem(int itemID);
    List<ItemProperty> GetItemDatabase();
    ItemView GetCurrentItem();
    float GetTotalWeight();
    float GetPlayerBagWeight();
    float GetPlayerBagCapacity();

    void SetPanelViews();
    void SetQuantity(int itemID, int quantity);
    void SetItemWeight(int itemID, float newWeight);
    void SetCurrentItem(ItemView itemView);
    void SetBagWeight(float weight);
}