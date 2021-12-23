public class ScoreManager : Singleton<ScoreManager>
{
    private long _currentScore;
    public long CurrentScore => _currentScore;

    protected override void Awake()
    {
        base.Awake();
        EventManager.AddListener<int, int>(GameEventType.ClearRows, OnClearRows);
    }

    private void Start()
    {
        _currentScore = 0;
    }

    private void OnClearRows(int clearRows, int combosCount)
    {
        long addedScore = 0;
        switch (clearRows)
        {
            case 1:
                addedScore = 10 * (PlayerPrefsManager.Instance.savedData.Level + 1);
                break;
            case 2:
                addedScore = 25 * (PlayerPrefsManager.Instance.savedData.Level + 1);
                break;
            case 3:
                addedScore = 75 * (PlayerPrefsManager.Instance.savedData.Level + 1);
                break;
            case 4:
            default:
                addedScore = 300 * (PlayerPrefsManager.Instance.savedData.Level + 1);
                break;
        }

        addedScore *= combosCount;
        _currentScore += addedScore;

        EventManager.Invoke<long>(ScoreEventType.UpdateScore, _currentScore);
        EventManager.Invoke<long, int>(ScoreEventType.ShowPopUpScoreText, addedScore, combosCount);
        EventManager.Invoke<int, int>(ScoreEventType.ShowPopUpScoreEffect, clearRows, combosCount);
    }
}