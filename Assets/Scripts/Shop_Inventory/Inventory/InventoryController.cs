using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : BaseController<InventoryView, InventoryModel>
{
    private SoundService soundService;

    public InventoryController(InventoryView _inventoryView, InventoryModel _inventoryModel, SoundService _soundService)
        : base(_inventoryView, _inventoryModel)
    {
        soundService = _soundService;
        view.SetInventoryController(this);
    }

    public void GatherResource()
    {
        EventService.Instance.OnGatherResourceButtonPressed.InvokeEvent();
        for (int i = 0; i < model.numberOfResource; i++)
        {
            int index = GetRandomIndex();
            view.DisplayGatheredItem(index);
        }
    }

    private int GetRandomIndex()
    {
        SetAvailableRarity();
        bool isItemForValue;
        int index;
        do
        {
            index = UnityEngine.Random.Range(0, model.GetItemDatabase().Count);
            isItemForValue = IsItemForValue(index);
        } while (!isItemForValue);
        return index;
    }

    private void SetAvailableRarity()
    {
        Dictionary<int, ItemView> instantiatedItems = model.GetInstatiatedItems();
        if (instantiatedItems.Count > 0)
        {
            foreach (var entry in instantiatedItems)
            {
                int itemID = entry.Key;
                ItemView item = GetInstantiatedItem(itemID);
                switch (item.itemProperty.rarity)
                {
                    case ItemProperty.Rarity.VeryCommon:
                        model.SetRarityAvailable(ItemProperty.Rarity.VeryCommon, true);
                        break;
                    case ItemProperty.Rarity.Common:
                        model.SetRarityAvailable(ItemProperty.Rarity.Common, true);
                        break;
                    case ItemProperty.Rarity.Rare:
                        model.SetRarityAvailable(ItemProperty.Rarity.Rare, true);
                        break;
                    case ItemProperty.Rarity.Legendary:
                        model.SetRarityAvailable(ItemProperty.Rarity.Legendary, true);
                        break;
                    case ItemProperty.Rarity.Epic:
                        model.SetRarityAvailable(ItemProperty.Rarity.Epic, true);
                        break;
                }
            }
        }
        else
        {
            model.SetRarityAvailable(ItemProperty.Rarity.VeryCommon, true);
        }
    }

    private bool IsItemForValue(int index)
    {
        ItemProperty.Rarity itemRarity = model.GetItemDatabase()[index].rarity;
        return model.IsRarityAvailable(itemRarity);
    }

    public int GenerateRandomQuantity()
    {
        return UnityEngine.Random.Range(1, 3);
    }

    public void RemoveItem(int itemID)
    {
        model.SetRarityAvailable(model.currentItem.itemProperty.rarity, false);
        model.RemoveInstatiatedItem(itemID);
        if (model.GetInstatiatedItems().Count <= 0)
            DisablePanel();
    }

    public float GetTotalWeight()
    {
        float totalWeight = 0;
        foreach (var kvp in model.GetInstatiatedItems())
        {
            int itemID = kvp.Key;
            totalWeight += GetItemWeight(itemID);
        }
        return totalWeight;
    }

    public void EnableInventoryVisibility() => view.EnableInventoryVisibility();
    public void DisableInventoryVisibility() => view.DisableInventoryVisibility();
    public void ApplyFilter(FilterController inventoryFilterController) => inventoryFilterController.ApplyFilter();
    public void StoreItem(ItemView itemDisplay, FilterController inventoryFilterController) => inventoryFilterController.AddItemDisplay(itemDisplay);
    public bool IsItemAlreadyInstantiated(int itemID) => model.GetInstatiatedItems().ContainsKey(itemID);
    public void StoreInstantiatedItem(int itemID, ItemView itemView) => model.StoreInstantiatedItems(itemID, itemView);
    public bool ISInventoryOn() => view.isInventoryOn;
    public void ResetQuantities(int itemID) => model.ResetQuantities(itemID);
    public void RemoveWeight(int itemID, int quantity) => model.RemoveWeight(itemID, quantity);
    public void DisablePanel() => GameManager.Instance.uiController.DisableItemDetailsPanel();
    public void DisplayBroughtItems(ItemView itemView, int newQuantity) => view.DisplayBroughtItem(itemView, newQuantity);
    public List<ItemProperty> GetItemDatabase() => model.GetItemDatabase();
    public int GetItemQuantity(int itemID) => model.GetQuantity(itemID).Sum();
    public ItemView GetCurrentItem() => model.currentItem;
    public ItemView GetInstantiatedItem(int itemID) => model.GetInstatiatedItems().TryGetValue(itemID, out ItemView itemView) ? itemView : null;
    public float GetItemWeight(int itemID) => model.GetItemWeight(itemID).Sum();
    public float GetPlayerBagWeight() => GameManager.Instance.playerController.GetBagWeight();
    public float GetPlayerBagCapacity() => GameManager.Instance.playerController.GetBagCapacity();
    public void SetQuantity(int itemId, int quantity) => model.SetItemQuantities(itemId, quantity);
    public void SetCurrentItem(ItemView itemView) => model.currentItem = itemView;
    public void SetPanelViews() => GameManager.Instance.uiController.SetItemDetailsPanel(true, GetCurrentItem());
    public void SetItemWeight(int itemID, float newWeight) => model.SetItemWeight(itemID, newWeight);
    public void SetBagWeight(float weight) => GameManager.Instance.playerController.SetBagWeight(weight);
}