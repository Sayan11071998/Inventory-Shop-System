using System.Collections.Generic;

public interface IInventoryController
{
    void GatherResource();
    void EnableInventoryVisibility();
    void DisableInventoryVisibility();
    void SetCurrentItem(ItemView itemView);
    int GetItemQuantity(int itemID);
    int GenerateRandomQuantity();
    float GetTotalWeight();
    void RemoveWeight(int itemID, int quantity);
    void ResetQuantities(int itemID);
    void SetQuantity(int itemID, int quantity);
    void SetItemWeight(int itemID, float newWeight);
    void StoreInstantiatedItem(int itemID, ItemView itemView);
    bool IsItemAlreadyInstantiated(int itemID);
    ItemView GetInstantiatedItem(int itemID);
    List<ItemProperty> GetItemDatabase();
    ItemView GetCurrentItem();
    void DisablePanel();
    void SetBagWeight(float weight);
    float GetPlayerBagWeight();
    float GetPlayerBagCapacity();
    void ApplyFilter(FilterController filterController);
    void DisplayBroughtItems(ItemView itemView, int newQuantity);
    bool ISInventoryOn();
    void SetPanelViews();
    void StoreItem(ItemView itemDisplay, FilterController filterController);
}