using UnityEngine;

public class BaseView : MonoBehaviour
{
    [SerializeField] protected GameObject itemPrefab;
    [SerializeField] protected Transform parentPanel;

    protected CanvasGroup canvasGroup;

    protected virtual void Awake() => canvasGroup = GetComponent<CanvasGroup>();

    public virtual void EnableVisibility()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public virtual void DisableVisibility()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    protected ItemView CreateItemView(ItemProperty itemProperty)
    {
        if (itemPrefab == null || parentPanel == null)
        {
            Debug.LogError("itemPrefab or parentPanel not assigned in BaseView.");
            return null;
        }

        GameObject newItem = Instantiate(itemPrefab, parentPanel);
        ItemView itemView = newItem.GetComponent<ItemView>();

        if (itemView != null)
            itemView.itemProperty = itemProperty;

        return itemView;
    }
}