using System.Collections.Generic;

public abstract class BaseController<TView, TModel>
    where TView : BaseView
    where TModel : BaseModel
{
    protected TView view;
    protected TModel model;

    public BaseController(TView _view, TModel _model)
    {
        view = _view;
        model = _model;
    }

    public List<ItemProperty> GetItemDatabase() => model.GetItemDatabase();
    public int GetItemQuantity(int itemID) => model.GetQuantity(itemID);
    public float GetItemWeight(int itemID) => model.GetItemWeight(itemID);
    public float GetPlayerBagWeight() => GameManager.Instance.playerController.GetBagWeight();
    public float GetPlayerBagCapacity() => GameManager.Instance.playerController.GetBagCapacity();
    public int GetPlayerCoin() => GameManager.Instance.playerController.GetPlayerCoinCount();

    public void SetItemQuantities(int itemID, int quantity) => model.SetItemQuantities(itemID, quantity);
    public void SetItemWeight(int itemID, float newWeight) => model.SetItemWeight(itemID, newWeight);
}