using System.Collections.Generic;

public interface IItemListView
{
    void DisplayItems(List<ItemProperty> items);
    void EnableVisibility();
    void DisableVisibility();
}