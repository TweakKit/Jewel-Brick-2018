using UnityEngine;

using TMPro;

public class GameOverPopUp : PopUp
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _bestScoreText;
    [SerializeField]
    private GameObject _replayButton;
    [SerializeField]
    private string _appRateLink;
    [SerializeField]
    private float _scoreTextTransformingSpeed;
    [SerializeField]
    private float _replayButtonScalingUpTime;

    public void UpdateData(long score, long bestScore)
    {
        _replayButton.GetComponent<Animator>().enabled = false;
        _replayButton.transform.localScale = Vector3.zero;

        if (score != 0 || bestScore != 0)
        {
            StartCoroutine(Lerp.PlaySFXMusicInDuration
            (
                SoundManager.Instance.scoreUpdateSFXClip,
                1.0f / _scoreTextTransformingSpeed,
                0.1f)
            );
        }

        StartCoroutine(Lerp.TransformText(_scoreText,
                                             0,
                                             score,
                                             "",
                                             _scoreTextTransformingSpeed));

        StartCoroutine(Lerp.TransformText(_bestScoreText,
                                              0,
                                              bestScore,
                                              "",
                                              _scoreTextTransformingSpeed));

        LeanTween.delayedCall(1.0f / _scoreTextTransformingSpeed, InvokeOpenAds);
    }

    public void ReplayGame() => BlockTypeConcrete.Reset();
    public void Rate() => Application.OpenURL(_appRateLink.Trim());

    private void InvokeOpenAds()
    {
        LeanTween.scale(_replayButton, Vector3.one, _replayButtonScalingUpTime)
                 .setOnComplete(() => _replayButton.GetComponent<Animator>().enabled = true);
        AdmobController.Instance.ShowInterstialAd();
    }
}