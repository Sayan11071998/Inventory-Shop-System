using UnityEngine;

public class BaseView : MonoBehaviour
{
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
}