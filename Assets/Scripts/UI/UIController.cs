public class UIController
{
    private UIView uiView;

    public UIController(UIView uiView)
    {
        this.uiView = uiView;
        uiView.SetUIController(this);
    }

    public void OnShopToggleChanged(bool isOn)
    {
        SoundManager.Instance.PlaySound(Sounds.ShopInventorySwitchButton);
        if (isOn == false)
        {
            EventService.Instance.OnShopToggledOnEvent.InvokeEvent();
        }
        else
        { 
            EventService.Instance.OnInventoryToggledOnEvent.InvokeEvent();
        }
        uiView.UpdateShopORInventoryText(!isOn);
    }

    public void SetItemDetailsPanel(bool isOn, ItemView itemDisplay)
    {
        uiView.SetItemDetailPanelView(isOn, itemDisplay);
    }

    public int GetQuantity()
    {
       if(GameManager.Instance.inventoryController.ISInventoryOn())
        {
            int itemID = GameManager.Instance.inventoryController.GetCurrentItem().itemProperty.itemID;
            int quantity = GameManager.Instance.inventoryController.GetItemQuantity(itemID);

            return quantity;
        }

       if(GameManager.Instance.shopController.ISShopOn())
        {
            int itemID = GameManager.Instance.shopController.GetCurrentItem().itemProperty.itemID;
            int quantity = GameManager.Instance.shopController.GetItemQuantity(itemID);

            return quantity;
        }
        return 0;
    }

    public void DisableItemDetailsPanel()
    {
        uiView.DisableItemDetailsPanel();
    }
}
