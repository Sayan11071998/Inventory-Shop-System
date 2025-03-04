public class SoundService
{
    public SoundService() => EventService.Instance.onItemSoldWithIntParams.AddListener((amount) => PlaySoldSound());
    
    public void PlayGatherResourceSound() => SoundManager.Instance.PlaySound(Sounds.GatherResource);
    public void PlaySoldSound() => SoundManager.Instance.PlaySound(Sounds.MoneySound);
    public void PlayQuantityChangedSound() => SoundManager.Instance.PlaySound(Sounds.QuantityChanged);
    public void PlayPopSound() => SoundManager.Instance.PlaySound(Sounds.ErrorSound);
    public void PlayNonClickableSound() => SoundManager.Instance.PlaySound(Sounds.NonClickable);
}