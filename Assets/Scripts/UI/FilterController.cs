using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterController : MonoBehaviour
{
    [SerializeField] private List<Toggle> filterToggle;

    private ItemProperty.ItemTypes currentFilterState;
    private Dictionary<Toggle, ItemProperty.ItemTypes> toggleFilterMap;
    private List<ItemView> itemDisplay = new List<ItemView>();
    bool showAll = true;

    private void Start() => Initialize();

    private void Initialize()
    {
        currentFilterState = ItemProperty.ItemTypes.Materials;

        toggleFilterMap = new Dictionary<Toggle, ItemProperty.ItemTypes>
        {
            {filterToggle[0], ItemProperty.ItemTypes.Materials},
            {filterToggle[1], ItemProperty.ItemTypes.Weapons},
            {filterToggle[2], ItemProperty.ItemTypes.Consumables},
            {filterToggle[3], ItemProperty.ItemTypes.Treasure},
        };

        foreach (Toggle toggle in filterToggle)
            toggle.onValueChanged.AddListener((isOn) => SetFilterState(toggle, isOn));
    }

    void SetFilterState(Toggle changedToggle, bool isToggleOn)
    {
        PlayFilterButtonSound();

        if (isToggleOn == true && toggleFilterMap.ContainsKey(changedToggle))
        {
            currentFilterState = toggleFilterMap[changedToggle];
            ApplyFilter();
        }
    }

    public void ShowAll(bool isOn)
    {
        PlayFilterButtonSound();
        if (isOn)
        {
            showAll = true;
        }
        else
        {
            showAll = false;
            return;
        }

        foreach (ItemView item in itemDisplay)
        {
            if (item != null)
                item.EnabaleItem();
        }
    }

    private static void PlayFilterButtonSound() => SoundManager.Instance.PlaySound(Sounds.FilterButtonSound);

    public void ApplyFilter()
    {
        if (showAll == true) return;

        foreach (ItemView item in itemDisplay)
        {
            if (currentFilterState == item.itemType)
            {
                if (item != null)
                    item.EnabaleItem();
            }
            else
            {
                if (item != null)
                    item.disableItem();
            }
        }
    }

    public void AddItemDisplay(ItemView newItemDisplay) => itemDisplay.Add(newItemDisplay);
}