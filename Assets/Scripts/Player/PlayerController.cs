public class PlayerController
{
    private PlayerView playerView;
    private PlayerModel playerModel;

    public PlayerController(PlayerView playerView, PlayerModel playerModel)
    {
        this.playerView = playerView;
        this.playerModel = playerModel;

        this.playerView.SetPlayerController(this);
    }

    public int GetPlayerCoinCount()
    {
        return playerModel.numberOfCoins;
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

    public float GetBagCapacity()
    {
        return playerModel.bagCapacity;
    }

    public float GetBagWeight()
    {
        return playerModel.bagWeight;
    }

    public void DecreasePlayerCoin(int coin)
    {
        playerModel.numberOfCoins -= coin;
        playerView.SetCoinText();
    }
}
