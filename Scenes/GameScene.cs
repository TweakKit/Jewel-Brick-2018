using UnityEngine;

public enum GameState
{
    None,
    Paused,
    Playing,
    GameOver
}

public class GameScene : BaseScene
{
    private GameState _gameState;

    public bool IsPlaying => _gameState == GameState.Playing;
    public bool IsPaused => _gameState == GameState.Paused;

    public SceneTransition sceneTransition;

    private int dieNumberOfTimes;

    protected override void Awake()
    {
        base.Awake();
        EventManager.AddListener(GameEventType.GameOver, OpenGameAgain);
    }

    private void Start()
    {
        dieNumberOfTimes = 0;
        _gameState = GameState.Playing;
        _onOpen?.Invoke();

        RankPopUp.HasLoadedLeaderboardOnce = false;
        AdmobController.Instance.RequestAds();
    }

    public void PauseGame()
    {
        _gameState = GameState.Paused;
    }

    public void ResumeGame()
    {
        _gameState = GameState.Playing;
    }

    public void OpenGameAgain()
    {
        sceneTransition.PerformTransition();
    }
}