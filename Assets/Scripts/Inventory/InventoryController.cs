using System.Collections.Generic;
using System.Linq;

public class InventoryController
{
    private InventoryView inventoryView;
    private InventoryModel inventoryModel;
    public InventoryController(InventoryView inventoryView, InventoryModel inventoryModel)
    {
        this.inventoryView = inventoryView;
        this.inventoryView.SetInventoryController(this);

        this.inventoryModel = inventoryModel;
    }

    public void GatherResource()
    {
        PlayGatherResourceSound();
        for (int i = 0; i < inventoryModel.numberOfResource; i++)
        {
            int index = GetRandomIndex();
            inventoryView.DisplayGatheredItem(index);
        }
    }

    private void PlayGatherResourceSound()
    {
        SoundManager.Instance.PlaySound(Sounds.GatherResource);
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
        } while (!isItemForValue);

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
        if (inventoryModel.IsRarityAvailable(itemRarity))
        {
            return true;
        }

        return false;
    }

    public void EnableInventoryVisibility()
    {
        inventoryView.EnableInventoryVisibility();
    }

    public void DisableInventoryVisibility()
    {
        inventoryView.DisableInventoryVisibility();
    }

    public List<ItemProperty> GetItemDatabase()
    {
        return inventoryModel.getItemDatabase();
    }

    public void ApplyFilter(FilterController inventoryFilterController)
    {
        inventoryFilterController.ApplyFilter();
    }

    public void StoreItem(ItemView itemDisplay, FilterController inventoryFilterController)
    {
        inventoryFilterController.AddItemDisplay(itemDisplay);
    }

    public int GenerateRandomQuantity()
    {
        int quantity = UnityEngine.Random.Range(1, 3);
        return quantity;
    }

    public void SetQuantity(int itemId, int quantity)
    {
        inventoryModel.SetItemQuantities(itemId, quantity);
    }

    public int GetItemQuantity(int itemID)
    {
        return inventoryModel.GetQuantity(itemID).Sum();
    }

    public bool IsItemAlreadyInstantiated(int itemID)
    {
        return inventoryModel.GetInstatiatedItems().ContainsKey(itemID);
    }

    public ItemView GetInstantiatedItem(int itemID)
    {
        return inventoryModel.GetInstatiatedItems().TryGetValue(itemID, out ItemView itemView) ? itemView : null;
    }

    public void StoreInstantiatedItem(int itemID, ItemView itemView)
    {
        inventoryModel.StoreInstantiatedItems(itemID, itemView);
    }

    public void SetCurrentItem(ItemView itemView)
    {
        inventoryModel.currentItem = itemView;
    }

    public ItemView GetCurrentItem()
    {
        return inventoryModel.currentItem;
    }

    public bool ISInventoryOn()
    {
        return inventoryView.isInventoryOn;
    }

    public void ResetQuantities(int itemID)
    {
        inventoryModel.ResetQuantities(itemID);
    }

    public void SetPanelViews()
    {
        GameManager.Instance.uiController.SetItemDetailsPanel(true, GetCurrentItem());
    }

    public void RemoveWeight(int itemID, int quantity)
    {
        inventoryModel.RemoveWeight(itemID, quantity);
    }
    public void RemoveItem(int itemID)
    {
        inventoryModel.SetRarityAvailable(GetCurrentItem().itemProperty.rarity, false);
        inventoryModel.RemoveInstatiatedItem(itemID);

        if (inventoryModel.GetInstatiatedItems().Count <= 0)
        {
            DisablePanel();
        }
    }

    public void DisablePanel()
    {
        GameManager.Instance.uiController.DisableItemDetailsPanel();
    }

    public void SetItemWeight(int itemID, float newWeight)
    {
        inventoryModel.SetItemWeight(itemID, newWeight);
    }

    public float GetItemWeight(int itemID)
    {
        return inventoryModel.GetItemWeight(itemID).Sum();
    }

    public void SetBagWeight(float weight)
    {
        GameManager.Instance.playerController.SetBagWeight(weight);
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

    public float GetPlayerBagWeight()
    {
        return GameManager.Instance.playerController.GetBagWeight();
    }

    public float GetPlayerBagCapacity()
    {
        return GameManager.Instance.playerController.GetBagCapacity();
    }

    public void DisplayBroughtItems(ItemView itemView, int newQuantity)
    {
        inventoryView.DisplayBroughtItem(itemView, newQuantity);
    }


    public void PlaySoldSound()
    {
        SoundManager.Instance.PlaySound(Sounds.MoneySound);
    }

    public void PlayQuantityChangedSound()
    {
        SoundManager.Instance.PlaySound(Sounds.QuantityChanged);
    }

    public void PlayPopSound()
    {
        SoundManager.Instance.PlaySound(Sounds.ErrorSound);
    }

    public void PlayNonClickableSound()
    {
        SoundManager.Instance.PlaySound(Sounds.NonClickable);
    }
}
