using UnityEngine;

using TMPro;

public class Score : MonoBehaviour, IGameScore
{
    private TextMeshProUGUI _scoreText;
    private long _previousScore;

    private void Awake()
    {
        _scoreText = gameObject.GetComponent<TextMeshProUGUI>();
        EventManager.AddListener<long>(ScoreEventType.UpdateScore, OnUpdateScore);
        EventManager.AddListener(ScoreEventType.ResetScore, OnResetScore);
    }

    private void Start()
    {
        _scoreText.text = 0.ToString();
        _previousScore = 0;
    }

    public void OnUpdateScore(long currentScore)
    {
        StartCoroutine(Lerp.TransformText
       (
           _scoreText,
           _previousScore,
           currentScore,
           "",
           UIDefinition.TextTransformingSpeed,
           () => _previousScore = currentScore
       ));
    }

    public void OnResetScore() => Start();
}