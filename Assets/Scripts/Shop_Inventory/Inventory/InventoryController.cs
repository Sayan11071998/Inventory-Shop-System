using System.Collections.Generic;
using System.Linq;

public class InventoryController : BaseController<InventoryView, InventoryModel>, IInventoryController
{
    public InventoryController(InventoryView _inventoryView, InventoryModel _inventoryModel)
        : base(_inventoryView, _inventoryModel)
    {
        view.SetInventoryController(this);
    }

    public void GatherResource()
    {
        EventService.Instance.OnGatherResourceButtonPressed?.InvokeEvent();

        for (int i = 0; i < model.numberOfResource; i++)
        {
            int index = GetRandomIndex();
            ItemProperty resource = model.GetItemDatabase()[index];
            int quantityToAdd = GenerateRandomQuantity();
            float additionalWeight = quantityToAdd * resource.weight;
            float currentInventoryWeight = GetTotalWeight();

            if (currentInventoryWeight + additionalWeight <= GetPlayerBagCapacity())
            {
                view.DisplayGatheredItem(index, quantityToAdd);
                SetBagWeight(GetTotalWeight());
            }
            else
            {
                EventService.Instance.OnMaximumWeightExceed?.InvokeEvent();
                view.ShowWeightExceededPopup();
                break;
            }
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
                    case ItemRarity.VeryCommon:
                        model.SetRarityAvailable(ItemRarity.VeryCommon, true);
                        break;
                    case ItemRarity.Common:
                        model.SetRarityAvailable(ItemRarity.Common, true);
                        break;
                    case ItemRarity.Rare:
                        model.SetRarityAvailable(ItemRarity.Rare, true);
                        break;
                    case ItemRarity.Legendary:
                        model.SetRarityAvailable(ItemRarity.Legendary, true);
                        break;
                    case ItemRarity.Epic:
                        model.SetRarityAvailable(ItemRarity.Epic, true);
                        break;
                }
            }
        }
        else
        {
            model.SetRarityAvailable(ItemRarity.VeryCommon, true);
        }
    }

    private bool IsItemForValue(int index)
    {
        ItemRarity itemRarity = model.GetItemDatabase()[index].rarity;
        return model.IsRarityAvailable(itemRarity);
    }

    public int GenerateRandomQuantity() => UnityEngine.Random.Range(1, 3);

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
            int quantity = model.GetQuantity(itemID).Sum();
            ItemView itemView = kvp.Value;
            float unitWeight = itemView.itemProperty.weight;
            totalWeight += quantity * unitWeight;
        }
        return totalWeight;
    }

    public void EnableInventoryVisibility() => view.EnableInventoryVisibility();
    public void DisableInventoryVisibility() => view.DisableInventoryVisibility();

    public void ApplyFilter(FilterController inventoryFilterController) => inventoryFilterController.ApplyFilter();
    public void StoreInstantiatedItem(int itemID, ItemView itemView) => model.StoreInstantiatedItems(itemID, itemView);
    public bool IsItemAlreadyInstantiated(int itemID) => model.GetInstatiatedItems().ContainsKey(itemID);
    public void ResetQuantities(int itemID) => model.ResetQuantities(itemID);
    public void RemoveWeight(int itemID, int quantity) => model.RemoveWeight(itemID, quantity);
    public void DisablePanel() => GameManager.Instance.uiController.DisableItemDetailsPanel();
    public void DisplayBroughtItems(ItemView itemView, int newQuantity) => view.DisplayBroughtItem(itemView, newQuantity);
    public bool ISInventoryOn() => view.isInventoryOn;
    public void StoreItem(ItemView itemDisplay, FilterController filterController) => filterController.AddItemDisplay(itemDisplay);

    public List<ItemProperty> GetItemDatabase() => model.GetItemDatabase();
    public int GetItemQuantity(int itemID) => model.GetQuantity(itemID).Sum();
    public ItemView GetCurrentItem() => model.currentItem;
    public ItemView GetInstantiatedItem(int itemID) => model.GetInstatiatedItems().TryGetValue(itemID, out ItemView itemView) ? itemView : null;
    public float GetItemWeight(int itemID) => model.GetItemWeight(itemID).Sum();
    public float GetPlayerBagWeight() => GameManager.Instance.playerController.GetBagWeight();
    public float GetPlayerBagCapacity() => GameManager.Instance.playerController.GetBagCapacity();

    public void SetQuantity(int itemID, int quantity) => model.SetItemQuantities(itemID, quantity);
    public void SetCurrentItem(ItemView itemView) => model.currentItem = itemView;
    public void SetItemWeight(int itemID, float newWeight) => model.SetItemWeight(itemID, newWeight);
    public void SetBagWeight(float weight) => GameManager.Instance.playerController.SetBagWeight(weight);
    public void SetPanelViews() => GameManager.Instance.uiController.SetItemDetailsPanel(true, GetCurrentItem());
}