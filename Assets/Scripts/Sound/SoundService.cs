public class SoundService
{
    public SoundService()
    {
        EventService.Instance.OnGatherResourceButtonPressed.AddListener(PlayGatherResourceSound);
        EventService.Instance.onItemSoldWithIntParams.AddListener((amount) => PlaySoldSound());
        EventService.Instance.OnQuantityChanged.AddListener(PlayQuantityChangedSound);
        EventService.Instance.OnMaximumWeightExceed.AddListener(PlayPopSound);
        EventService.Instance.OnNonClickableButtonPressed.AddListener(PlayNonClickableSound);
    }
    
    public void PlayGatherResourceSound() => SoundManager.Instance.PlaySound(Sounds.GatherResource);
    public void PlaySoldSound() => SoundManager.Instance.PlaySound(Sounds.MoneySound);
    public void PlayQuantityChangedSound() => SoundManager.Instance.PlaySound(Sounds.QuantityChanged);
    public void PlayPopSound() => SoundManager.Instance.PlaySound(Sounds.ErrorSound);
    public void PlayNonClickableSound() => SoundManager.Instance.PlaySound(Sounds.NonClickable);
}