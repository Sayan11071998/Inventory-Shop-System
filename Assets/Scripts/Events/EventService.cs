public class EventService
{
    private static EventService instance;
    public static EventService Instance
    {
        get
        {
            if (instance == null)
                instance = new EventService();
            return instance;
        }
    }

    public EventController OnShopToggledOnEvent { get; private set; }
    public EventController OnInventoryToggledOnEvent { get; private set; }
    public EventController OnItemSelectedEvent { get; private set; }
    public EventController<bool, ItemView> OnItemSelectedEventWithParams { get; private set; }
    public EventController<int> onItemBroughtWithIntParams { get; private set; }
    public EventController<int> onItemSoldWithIntParams { get; private set; }
    public EventController<float> onItemSoldWithFloatParams { get; private set; }
    public EventController onItemChanged { get; private set; }

    // public EventController OnItemSoldWithIntParams { get; private set; }

    public EventService()
    {
        OnShopToggledOnEvent = new EventController();
        OnInventoryToggledOnEvent = new EventController();

        OnItemSelectedEvent = new EventController();
        OnItemSelectedEventWithParams = new EventController<bool, ItemView>();

        onItemBroughtWithIntParams = new EventController<int>();
        onItemSoldWithFloatParams = new EventController<float>();
        onItemChanged = new EventController();

        onItemSoldWithIntParams = new EventController<int>();


        // OnItemSoldWithIntParams = new EventController();
    }
}