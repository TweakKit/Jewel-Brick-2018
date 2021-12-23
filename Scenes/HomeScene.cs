using UnityEngine;

public class HomeScene : BaseScene
{
    [SerializeField]
    private AnimatedButton _backgroundMusicButton;
    [SerializeField]
    private AnimatedButton _sfxMusicButton;
    [SerializeField]
    private GameObject _rankPopUp;

    private void Start()
    {
        _backgroundMusicButton.GetComponent<SpriteSwapper>().SetEnabled
            (PlayerPrefsManager.Instance.savedData.SoundBGMOn);
        _sfxMusicButton.GetComponent<SpriteSwapper>().SetEnabled
            (PlayerPrefsManager.Instance.savedData.SoundSFXOn);

        RankPopUp.HasLoadedLeaderboardOnce = false;
    }

    public void OpenRankPopUp()
    {
        OpenPopup<RankPopUp>(true, _rankPopUp);
    }

    public void Rate()
    {
        Application.OpenURL("https://play.google.com/apps");
    }

    public void ChangeBGMMusicState()
    {
        SoundManager.Instance.SetSoundBGMStatus(!PlayerPrefsManager.Instance.savedData.SoundBGMOn);
    }

    public void ChangeSFXMusicState()
    {
        SoundManager.Instance.SetSoundSFXStatus(!PlayerPrefsManager.Instance.savedData.SoundSFXOn);
    }
}