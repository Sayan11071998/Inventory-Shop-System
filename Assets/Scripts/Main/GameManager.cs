using UnityEngine;

public class GameManager : GenericMonoSingelton<GameManager>
{
    public ShopController shopController { get; private set; }
    public InventoryController inventoryController { get; private set; }
    public UIController uiController { get; private set; }
    public PlayerController playerController { get; private set; }

    private UIView uiView;
    private ShopView shopView;
    private InventoryView inventoryView;
    private PlayerView playerView;

    [SerializeField] private ItemDatabase itemDatabase;
    
    private void Start()
    {
        CreateUI();
        CreatePlayer();
        CreateShop();
        CreateInventory();
    }

    private void CreatePlayer()
    {
        PlayerModel playerModel = new PlayerModel();
        playerView = GameObject.FindFirstObjectByType<PlayerView>();
        playerController = new PlayerController(playerView, playerModel);
    }

    private void CreateInventory()
    {
        InventoryModel inventoryModel = new InventoryModel(itemDatabase);
        inventoryView = GameObject.FindFirstObjectByType<InventoryView>();
        inventoryController = new InventoryController(inventoryView, inventoryModel);
    }

    private void CreateUI()
    {
        uiView = GameObject.FindFirstObjectByType<UIView>();
        uiController = new UIController(uiView);
    }

    private void CreateShop()
    {
        ShopModel shopModel = new ShopModel(itemDatabase);
        shopView = GameObject.FindFirstObjectByType<ShopView>();
        shopController = new ShopController(shopView, shopModel);
    }
}
