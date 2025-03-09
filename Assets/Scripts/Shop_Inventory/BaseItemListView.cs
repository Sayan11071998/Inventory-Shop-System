using UnityEngine;

public abstract class BaseItemListView : BaseView
{
    [SerializeField] protected GameObject itemPrefab;
    [SerializeField] protected Transform parentPanel;

    protected ItemView CreateItemView(ItemProperty itemProperty)
    {
        GameObject newItem = Instantiate(itemPrefab, parentPanel);
        ItemView itemView = newItem.GetComponent<ItemView>();

        if (itemView != null)
            itemView.itemProperty = itemProperty;
        
        return itemView;
    }
}