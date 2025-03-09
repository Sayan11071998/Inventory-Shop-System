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

    public EventController<int> OnItemBroughtWithIntParams { get; private set; }

    public EventController<int> OnItemSoldWithIntParams { get; private set; }
    public EventController<float> OnItemSoldWithFloatParams { get; private set; }

    public EventController OnItemChanged { get; private set; }
    public EventController OnQuantityChanged { get; private set; }

    public EventController OnItemSelectedSound { get; private set; }
    public EventController OnGatherResourceButtonPressed { get; private set; }
    public EventController OnMaximumWeightExceed { get; private set; }
    public EventController OnNonClickableButtonPressed { get; private set; }
    public EventController OnFilterButtonPressed { get; private set; }
    public EventController OnShopInventorySwitchButtonPressed { get; private set; }

    public EventService()
    {
        OnShopToggledOnEvent = new EventController();
        OnInventoryToggledOnEvent = new EventController();

        OnItemSelectedEvent = new EventController();
        OnItemSelectedEventWithParams = new EventController<bool, ItemView>();

        OnItemBroughtWithIntParams = new EventController<int>();

        OnItemSoldWithIntParams = new EventController<int>();
        OnItemSoldWithFloatParams = new EventController<float>();

        OnItemChanged = new EventController();
        OnQuantityChanged = new EventController();
        
        OnItemSelectedSound = new EventController();
        OnGatherResourceButtonPressed = new EventController();
        OnMaximumWeightExceed = new EventController();
        OnNonClickableButtonPressed = new EventController();
        OnFilterButtonPressed = new EventController();
        OnShopInventorySwitchButtonPressed = new EventController();
    }
}