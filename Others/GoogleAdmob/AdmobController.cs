using System;

public class AdmobController : PersistentSingleton<AdmobController>
{
    private bool _hasWatchedRewardedAd;

    protected override void Awake()
    {
        base.Awake();
    }

    public void RequestAds()
    {
        RequestAndShowBannerAd();
        RequestInterstitialAd();
        CreateAndLoadRewardedAd();
    }

    public void ShowInterstialAd()
    {
    }

    public void ShowRewardVideoAd()
    {
        EventManager.Invoke(GameEventType.GameFail);
    }

    public void HideBannerAd()
    {
    }

    public bool CanShowRewardedVideoAd() => false;

    private void RequestAndShowBannerAd()
    {
    }

    private void RequestInterstitialAd()
    {
        
    }

    private void CreateAndLoadRewardedAd()
    {
        
    }
}