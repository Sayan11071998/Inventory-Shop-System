public class SoundService
{
    public SoundService()
    {
        EventService.Instance.OnItemSelectedSound.AddListener(PlayOnItemSelectedSound);
        EventService.Instance.OnQuantityChanged.AddListener(PlayQuantityChangedSound);
        EventService.Instance.OnItemSoldWithIntParams.AddListener((amount) => PlaySoldSound());
        EventService.Instance.OnFilterButtonPressed.AddListener(PlayFilterButtonSound);
        EventService.Instance.OnMaximumWeightExceed.AddListener(PlayPopSound);
        EventService.Instance.OnNonClickableButtonPressed.AddListener(PlayNonClickableSound);
        EventService.Instance.OnGatherResourceButtonPressed.AddListener(PlayGatherResourceSound);
    }
    
    public void PlayOnItemSelectedSound() => SoundManager.Instance.PlaySound(Sounds.ItemSelected);
    public void PlayQuantityChangedSound() => SoundManager.Instance.PlaySound(Sounds.QuantityChanged);
    public void PlaySoldSound() => SoundManager.Instance.PlaySound(Sounds.MoneySound);
    public void PlayFilterButtonSound() => SoundManager.Instance.PlaySound(Sounds.FilterButtonSound);
    public void PlayPopSound() => SoundManager.Instance.PlaySound(Sounds.ErrorSound);
    public void PlayNonClickableSound() => SoundManager.Instance.PlaySound(Sounds.NonClickable);
    public void PlayGatherResourceSound() => SoundManager.Instance.PlaySound(Sounds.GatherResource);
}