using UnityEngine;

public class PausePopUp : PopUp
{
    [SerializeField]
    private AnimatedButton _backgroundMusicButton;
    [SerializeField]
    private AnimatedButton _sfxMusicButton;

    public override void Init(BaseScene parentScene)
    {
        base.Init(parentScene);

        _backgroundMusicButton.GetComponent<SpriteSwapper>().SetEnabled
            (PlayerPrefsManager.Instance.savedData.SoundBGMOn);
        _sfxMusicButton.GetComponent<SpriteSwapper>().SetEnabled
            (PlayerPrefsManager.Instance.savedData.SoundSFXOn);
    }

    public void ChangeBGMMusicState()
    {
        SoundManager.Instance.SetSoundBGMStatus(!PlayerPrefsManager.Instance.savedData.SoundBGMOn);
    }

    public void ChangeSFXMusicState()
    {
        SoundManager.Instance.SetSoundSFXStatus(!PlayerPrefsManager.Instance.savedData.SoundSFXOn);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        (parentScene as GameScene).ResumeGame();
        Close();
    }
}