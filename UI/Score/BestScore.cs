using UnityEngine;

using TMPro;

public class BestScore : MonoBehaviour, IGameScore
{
    private TextMeshProUGUI _bestScoreText;
    private long _previousHighScore;
    private bool _hasPlayedSound;

    private void Awake()
    {
        _bestScoreText = gameObject.GetComponent<TextMeshProUGUI>();
        EventManager.AddListener<long>(ScoreEventType.UpdateScore, OnUpdateScore);
        EventManager.AddListener(ScoreEventType.ResetScore, OnResetScore);
    }

    private void Start()
    {
        _bestScoreText.text = PlayerPrefsManager.Instance.savedData.BestScore.ToString();
        _previousHighScore = 0;
        _hasPlayedSound = false;
    }

    public void OnUpdateScore(long currentScore)
    {
        if (currentScore > PlayerPrefsManager.Instance.savedData.BestScore)
        {
            _previousHighScore = PlayerPrefsManager.Instance.savedData.BestScore;

            if (!_hasPlayedSound)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.reachHighScoreSFXClip);
                _hasPlayedSound = true;
            }

            StartCoroutine(Lerp.TransformText
            (
                _bestScoreText,
                _previousHighScore,
                currentScore,
                "",
                UIDefinition.TextTransformingSpeed,
                () => PlayerPrefsManager.Instance.savedData.BestScore = currentScore

            ));
        }
    }

    public void OnResetScore() => Start();
}