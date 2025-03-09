using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryController : BaseController<InventoryView, InventoryModel>, IInventoryController
{
    private SoundService soundService;

    public InventoryController(InventoryView _inventoryView, InventoryModel _inventoryModel, SoundService _soundService)
        : base(_inventoryView, _inventoryModel)
    {
        soundService = _soundService;
        view.SetInventoryController(this);
    }

    // public void GatherResource()
    // {
    //     EventService.Instance.OnGatherResourceButtonPressed.InvokeEvent();
    //     for (int i = 0; i < model.numberOfResource; i++)
    //     {
    //         int index = GetRandomIndex();
    //         view.DisplayGatheredItem(index);
    //     }
    // }

    // public void GatherResource()
    // {
    //     // Notify that the gather button was pressed.
    //     EventService.Instance.OnGatherResourceButtonPressed.InvokeEvent();

    //     // Gather up to 'numberOfResource' items.
    //     for (int i = 0; i < model.numberOfResource; i++)
    //     {
    //         int index = GetRandomIndex();
    //         // Get the resource from the database.
    //         ItemProperty resource = model.GetItemDatabase()[index];
    //         // Generate a random quantity for this resource.
    //         int quantityToAdd = GenerateRandomQuantity();
    //         // Calculate additional weight needed.
    //         float additionalWeight = quantityToAdd * resource.weight;

    //         // Check if adding this resource would exceed the player's bag capacity.
    //         if (GetPlayerBagWeight() + additionalWeight <= GetPlayerBagCapacity())
    //         {
    //             // Use the new overload that passes the quantity.
    //             view.DisplayGatheredItem(index, quantityToAdd);
    //         }
    //         else
    //         {
    //             // If capacity would be exceeded, invoke the weight exceeded event and stop gathering.
    //             EventService.Instance.OnMaximumWeightExceed.InvokeEvent();
    //             break;
    //         }
    //     }
    //     // Update the player's bag weight based on the new total.
    //     SetBagWeight(GetTotalWeight());
    // }


    public void GatherResource()
    {
        // Fire the gather event.
        EventService.Instance.OnGatherResourceButtonPressed.InvokeEvent();

        // Loop to gather up to 'numberOfResource' items.
        for (int i = 0; i < model.numberOfResource; i++)
        {
            int index = GetRandomIndex();
            ItemProperty resource = model.GetItemDatabase()[index];
            int quantityToAdd = GenerateRandomQuantity();
            float additionalWeight = quantityToAdd * resource.weight;

            // Use the inventory's total weight rather than the player's bag weight.
            float currentInventoryWeight = GetTotalWeight();

            // Check if adding this resource would exceed the bag capacity.
            if (currentInventoryWeight + additionalWeight <= GetPlayerBagCapacity())
            {
                // Call the overload that passes quantity.
                view.DisplayGatheredItem(index, quantityToAdd);

                // Update the player's bag weight immediately.
                SetBagWeight(GetTotalWeight());
            }
            else
            {
                // If capacity is exceeded, notify and stop gathering.
                EventService.Instance.OnMaximumWeightExceed.InvokeEvent();
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

    // public float GetTotalWeight()
    // {
    //     float totalWeight = 0;
    //     foreach (var kvp in model.GetInstatiatedItems())
    //     {
    //         int itemID = kvp.Key;
    //         totalWeight += GetItemWeight(itemID);
    //     }
    //     return totalWeight;
    // }

    public float GetTotalWeight()
    {
        float totalWeight = 0;
        // For each unique item in inventory:
        foreach (var kvp in model.GetInstatiatedItems())
        {
            int itemID = kvp.Key;
            // Get the current quantity for that item.
            int quantity = model.GetQuantity(itemID).Sum();
            // Get the unit weight from the instantiated ItemView.
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

    public void DisplayBroughtItems(ItemView itemView, int newQuantity)
    {
        view.DisplayBroughtItem(itemView, newQuantity);
    }

    public void SetPanelViews()
    {
        GameManager.Instance.uiController.SetItemDetailsPanel(true, GetCurrentItem());
    }

    public bool ISInventoryOn() => view.isInventoryOn;

    // New method added per interface contract:
    public void StoreItem(ItemView itemDisplay, FilterController filterController)
    {
        filterController.AddItemDisplay(itemDisplay);
    }
}