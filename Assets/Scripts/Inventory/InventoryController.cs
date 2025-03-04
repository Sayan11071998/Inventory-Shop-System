using System.Collections.Generic;
using System.Linq;

public class InventoryController
{
    private InventoryView inventoryView;
    private InventoryModel inventoryModel;
    private SoundService soundService;

    public InventoryController(InventoryView _inventoryView, InventoryModel _inventoryModel, SoundService _soundService)
    {
        inventoryView = _inventoryView;
        inventoryModel = _inventoryModel;
        soundService = _soundService;

        inventoryView.SetInventoryController(this);
    }

    public void GatherResource()
    {
        EventService.Instance.OnGatherResourceButtonPressed.InvokeEvent();

        for (int i = 0; i < inventoryModel.numberOfResource; i++)
        {
            int index = GetRandomIndex();
            inventoryView.DisplayGatheredItem(index);
        }
    }

    private int GetRandomIndex()
    {
        SetAvailableRarity();

        bool isItemForValue;
        int index;

        do
        {
            index = UnityEngine.Random.Range(0, GetItemDatabase().Count);
            isItemForValue = IsItemForValue(index);
        }
        while (!isItemForValue);

        return index;
    }

    private void SetAvailableRarity()
    {
        Dictionary<int, ItemView> instatiatedItems = inventoryModel.GetInstatiatedItems();

        if (instatiatedItems.Count() > 0)
        {
            foreach (var entry in instatiatedItems)
            {
                int itemID = entry.Key;
                ItemView item = GetInstantiatedItem(itemID);

                switch (item.itemProperty.rarity)
                {
                    case ItemProperty.Rarity.VeryCommon:
                        inventoryModel.SetRarityAvailable(ItemProperty.Rarity.VeryCommon, true);
                        break;

                    case ItemProperty.Rarity.Common:
                        inventoryModel.SetRarityAvailable(ItemProperty.Rarity.Common, true);
                        break;

                    case ItemProperty.Rarity.Rare:
                        inventoryModel.SetRarityAvailable(ItemProperty.Rarity.Rare, true);
                        break;

                    case ItemProperty.Rarity.Legendary:
                        inventoryModel.SetRarityAvailable(ItemProperty.Rarity.Legendary, true);
                        break;

                    case ItemProperty.Rarity.Epic:
                        inventoryModel.SetRarityAvailable(ItemProperty.Rarity.Epic, true);
                        break;
                }
            }
        }
        else
        {
            inventoryModel.SetRarityAvailable(ItemProperty.Rarity.VeryCommon, true);
        }
    }

    private bool IsItemForValue(int index)
    {
        ItemProperty.Rarity itemRarity = GetItemDatabase()[index].rarity;

        if (inventoryModel.IsRarityAvailable(itemRarity)) return true;

        return false;
    }

    public int GenerateRandomQuantity()
    {
        int quantity = UnityEngine.Random.Range(1, 3);
        return quantity;
    }

    public void RemoveItem(int itemID)
    {
        inventoryModel.SetRarityAvailable(GetCurrentItem().itemProperty.rarity, false);
        inventoryModel.RemoveInstatiatedItem(itemID);

        if (inventoryModel.GetInstatiatedItems().Count <= 0)
            DisablePanel();
    }

    public float GetTotalWeight()
    {
        float totalWeight = 0;

        foreach (var totalItemWeight in inventoryModel.GetInstatiatedItems())
        {
            int itemID = totalItemWeight.Key;
            totalWeight += GetItemWeight(itemID);
        }

        return totalWeight;
    }

    public void EnableInventoryVisibility() => inventoryView.EnableInventoryVisibility();
    public void DisableInventoryVisibility() => inventoryView.DisableInventoryVisibility();
    public void ApplyFilter(FilterController inventoryFilterController) => inventoryFilterController.ApplyFilter();
    public void StoreItem(ItemView itemDisplay, FilterController inventoryFilterController) => inventoryFilterController.AddItemDisplay(itemDisplay);

    public bool IsItemAlreadyInstantiated(int itemID) => inventoryModel.GetInstatiatedItems().ContainsKey(itemID);
    public void StoreInstantiatedItem(int itemID, ItemView itemView) => inventoryModel.StoreInstantiatedItems(itemID, itemView);
    
    public bool ISInventoryOn() => inventoryView.isInventoryOn;
    public void ResetQuantities(int itemID) => inventoryModel.ResetQuantities(itemID);
    public void RemoveWeight(int itemID, int quantity) => inventoryModel.RemoveWeight(itemID, quantity);
    public void DisablePanel() => GameManager.Instance.uiController.DisableItemDetailsPanel();
    public void DisplayBroughtItems(ItemView itemView, int newQuantity) => inventoryView.DisplayBroughtItem(itemView, newQuantity);

    public List<ItemProperty> GetItemDatabase() => inventoryModel.getItemDatabase();
    public int GetItemQuantity(int itemID) => inventoryModel.GetQuantity(itemID).Sum();
    public ItemView GetCurrentItem() => inventoryModel.currentItem;
    public ItemView GetInstantiatedItem(int itemID) => inventoryModel.GetInstatiatedItems().TryGetValue(itemID, out ItemView itemView) ? itemView : null;
    public float GetItemWeight(int itemID) => inventoryModel.GetItemWeight(itemID).Sum();
    public float GetPlayerBagWeight() => GameManager.Instance.playerController.GetBagWeight();
    public float GetPlayerBagCapacity() => GameManager.Instance.playerController.GetBagCapacity();
    
    public void SetQuantity(int itemId, int quantity) => inventoryModel.SetItemQuantities(itemId, quantity);
    public void SetCurrentItem(ItemView itemView) => inventoryModel.currentItem = itemView;
    public void SetPanelViews() => GameManager.Instance.uiController.SetItemDetailsPanel(true, GetCurrentItem());
    public void SetItemWeight(int itemID, float newWeight) => inventoryModel.SetItemWeight(itemID, newWeight);
    public void SetBagWeight(float weight) => GameManager.Instance.playerController.SetBagWeight(weight);
}