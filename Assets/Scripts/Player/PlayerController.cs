public class PlayerController
{
    private PlayerView playerView;
    private PlayerModel playerModel;

    public PlayerController(PlayerView _playerView, PlayerModel _playerModel)
    {
        playerView = _playerView;
        playerModel = _playerModel;

        playerView.SetPlayerController(this);
    }

    public void IncreasePlayerCoin(int coin)
    {
        playerModel.numberOfCoins += coin;
        playerView.SetCoinText();
    }

    public void SetBagWeight(float Weight)
    {
        playerModel.bagWeight = Weight;
        playerView.SetBagWeightText();
    }

    public void DecreasePlayerCoin(int coin)
    {
        playerModel.numberOfCoins -= coin;
        playerView.SetCoinText();
    }

    public int GetPlayerCoinCount() => playerModel.numberOfCoins;
    public float GetBagCapacity() => playerModel.bagCapacity;
    public float GetBagWeight() => playerModel.bagWeight;
}