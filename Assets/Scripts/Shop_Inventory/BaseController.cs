public abstract class BaseController<TView, TModel>
{
    protected TView view;
    protected TModel model;

    public BaseController(TView _view, TModel _model)
    {
        view = _view;
        model = _model;
    }
}