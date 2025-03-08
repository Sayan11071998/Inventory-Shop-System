using UnityEngine;

public abstract class BaseItemListView : BaseView
{
    [SerializeField] protected GameObject itemPrefab;
    [SerializeField] protected Transform parentPanel;

    /// <summary>
    /// Instantiates an item prefab under the parentPanel and sets its ItemProperty.
    /// </summary>
    /// <param name="itemProperty">The item data to assign</param>
    /// <returns>The instantiated ItemView component</returns>
    protected ItemView CreateItemView(ItemProperty itemProperty)
    {
        GameObject newItem = Instantiate(itemPrefab, parentPanel);
        ItemView itemView = newItem.GetComponent<ItemView>();
        if (itemView != null)
        {
            itemView.itemProperty = itemProperty;
        }
        return itemView;
    }
}