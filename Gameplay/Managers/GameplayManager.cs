using System.Collections.Generic;

using UnityEngine;

using IEnumerator = System.Collections.IEnumerator;

public class GameplayManager : MonoBehaviour
{
    private Board _gameBoard;
    private Board _spawningBoard;
    private BlockCreator _bottomBlockCreator;
    private BlockCreator _topBlockCreator;
    private BlockSolver _blockSolver;
    private GameInput _humanInput;
    private GameInput _AIInput;

    private BoardGUI _boardGUI;
    private BoardHintManager _boardHint;
    private BoardSuggestionManager _boardSuggestionManager;

    private void Awake()
    {
        _gameBoard = new BoardBuilder(new List<Block>()).Build();
        _bottomBlockCreator = new BottomBlockCreator();
        _topBlockCreator = new TopBlockCreator();
        _blockSolver = new BlockSolver();
        _humanInput = new HumanInput();
        _AIInput = new AIInput();

        _boardGUI = gameObject.GetComponentInChildren<BoardGUI>();
        _boardHint = gameObject.GetComponentInChildren<BoardHintManager>();
        _boardSuggestionManager = gameObject.GetComponentInChildren<BoardSuggestionManager>();

        EventManager.AddListener(GameEventType.WatchedAds, OnWatchedAds);
    }

    private void Start()
    {
        _spawningBoard = _bottomBlockCreator.CreateBlocks(_gameBoard);
        _boardGUI.CreateBlocks(_spawningBoard.GetBlocks());

        _gameBoard = _blockSolver.MoveBlocksDown(BoardUtils.GetBlocks(_gameBoard, _spawningBoard));
        _boardGUI.MoveBlocksDown(_gameBoard.GetBlocks(), false);

        _spawningBoard = _topBlockCreator.CreateBlocks(_gameBoard);
        _boardGUI.CreateBlocks(_spawningBoard.GetBlocks());
        _boardHint.UpdateStatus(_gameBoard, _spawningBoard);

        StartCoroutine(RunGameFlow());
    }

    private IEnumerator RunGameFlow()
    {
        while (!_gameBoard.IsFull())
        {
            Block draggedAIBlock = _AIInput.GetDraggedBlock(_gameBoard, _spawningBoard);
            while (true)
            {
                if (draggedAIBlock != null)
                {
                    if (_boardSuggestionManager.CanShowSuggestion())
                    {
                        EventManager.Invoke(GameEventType.ShowBlockSuggestion, draggedAIBlock);
                        _boardGUI.StartSlidingABlock(draggedAIBlock);
                    }
                    else if (_boardSuggestionManager.HasHiddenSuggestion())
                    {
                        _boardSuggestionManager.Restore();
                        _boardGUI.StopSlidingABlock(draggedAIBlock);
                    }
                }

                if ((GameScene.Instance as GameScene).IsPlaying)
                {
                    Block draggedBlock = _humanInput.GetDraggedBlock(_gameBoard, _spawningBoard);
                    if (draggedBlock != null)
                    {
                        EventManager.Invoke(GameEventType.HideWarningLayer);

                        _gameBoard = _blockSolver.MoveABlockHorizontal(_gameBoard, draggedBlock);
                        _boardGUI.MoveABlockHorizontal(_gameBoard.GetBlocks(), draggedBlock);

                        yield return null;

                        _gameBoard = _blockSolver.MoveBlocksDown(BoardUtils.GetBlocks(_gameBoard, _spawningBoard));
                        _boardGUI.MoveBlocksDown(_gameBoard.GetBlocks(), true);

                        break;
                    }
                }

                yield return null;
            }

            yield return new WaitForSeconds(BlockDefinition.MovingSpeedWaitBeforePlayingSound);
            SoundManager.Instance.PlaySound(SoundManager.Instance.blocksFallSFXClip);
            yield return new WaitForSeconds(BlockDefinition.MovingSpeedWaitAfterPlayingSound);

            int combosCount = 0;
            while (_gameBoard.HasAComboInGame())
            {
                int clearRows = BoardUtils.GetClearRows(_gameBoard.GetSquareStatuses());

                ComboPreSolver comboPreSolver = _blockSolver.PreSolveCombos(_gameBoard);
                if (comboPreSolver.hasRainbowCombo)
                {
                    _boardGUI.PreSolveCombos(comboPreSolver);
                    yield return new WaitForSeconds(BoardDefinition.BlocksFlashingWaitingTime);
                }

                ComboSolver comboSolver = _blockSolver.SolveCombos(comboPreSolver);
                _gameBoard = comboSolver.returnedBoard;
                _boardGUI.SolveCombos(comboSolver);

                yield return new WaitForEndOfFrame();
                _boardGUI.CreateBlockExplosionEffects();
                yield return new WaitForSeconds(BoardDefinition.BlocksExplodingWaitingTime);

                _gameBoard = _blockSolver.MoveBlocksDown(BoardUtils.GetBlocks(_gameBoard));
                _boardGUI.MoveBlocksDown(_gameBoard.GetBlocks(), true);

                yield return new WaitForSeconds(BlockDefinition.MovingSpeedWaitBeforePlayingSound);
                if (comboPreSolver.hasRainbowCombo)
                {
                    SoundManager.Instance.PlaySound(SoundManager.Instance.blockHasRainbowComboSFXClip);
                    combosCount++;
                }
                else SoundManager.Instance.PlaySound(SoundManager.Instance.blocksCombosSFXClips, combosCount++);
                yield return new WaitForSeconds(BlockDefinition.MovingSpeedWaitAfterPlayingSound);

                EventManager.Invoke(GameEventType.ClearRows, clearRows, Mathf.Clamp(combosCount, 1, 6));

                if (!_gameBoard.HasAComboInGame())
                {
                    StopGameFlow();
                    yield break;
                }
                else yield return new WaitForSeconds(BoardDefinition.WaitForNextComboDelayTime);
            }

            if (_gameBoard.IsAtWarningState())
                EventManager.Invoke(GameEventType.ShowWarningLayer);

            if (_gameBoard.IsFull())
            {
                EventManager.Invoke(GameEventType.HideWarningLayer);

                yield return new WaitForSeconds(BoardDefinition.WaitForAskingContinueGameTime);
                EventManager.Invoke(GameEventType.GameFail);

                yield break;
            }
            else
            {
                _spawningBoard = _topBlockCreator.CreateBlocks(_gameBoard);
                _boardGUI.CreateBlocks(_spawningBoard.GetBlocks());
                _boardHint.UpdateStatus(_gameBoard, _spawningBoard);

                if (_gameBoard.IsEmpty())
                {
                    yield return new WaitForSeconds(BoardDefinition.AutoDropTopBlocksDelayTime);

                    _gameBoard = _blockSolver.MoveBlocksDown(BoardUtils.GetBlocks(_gameBoard, _spawningBoard));
                    _boardGUI.MoveBlocksDown(_gameBoard.GetBlocks(), true);

                    yield return new WaitForSeconds(BlockDefinition.MovingSpeedWaitBeforePlayingSound);
                    SoundManager.Instance.PlaySound(SoundManager.Instance.blocksFallSFXClip);
                    yield return new WaitForSeconds(BlockDefinition.MovingSpeedWaitAfterPlayingSound);

                    _spawningBoard = _topBlockCreator.CreateBlocks(_gameBoard);
                    _boardGUI.CreateBlocks(_spawningBoard.GetBlocks());
                    _boardHint.UpdateStatus(_gameBoard, _spawningBoard);
                }
            }

            yield return null;
        }
    }

    private void OnWatchedAds() => StartCoroutine(ContinueToPlayGame());

    private IEnumerator ContinueToPlayGame()
    {
        yield return new WaitForSeconds(BoardDefinition.AdsWatchedDelayTime);
        _boardGUI.Delete6BottomRows();
        yield return new WaitForSeconds(BoardDefinition.BlocksFallingAfterWatchingAdsTime);

        _gameBoard = new BoardBuilder(_boardGUI.GetBlocks()).Build();
        _gameBoard = _blockSolver.MoveBlocksDown(BoardUtils.GetBlocks(_gameBoard));
        _boardGUI.MoveBlocksDown(_gameBoard.GetBlocks(), true);

        yield return new WaitForSeconds(1.0f / BlockDefinition.MovingSpeed);

        int combosCount = 0;
        while (_gameBoard.HasAComboInGame())
        {
            ComboPreSolver comboPreSolver = _blockSolver.PreSolveCombos(_gameBoard);
            if (comboPreSolver.hasRainbowCombo)
            {
                _boardGUI.PreSolveCombos(comboPreSolver);
                yield return new WaitForSeconds(BoardDefinition.BlocksFlashingWaitingTime);
            }

            ComboSolver comboSolver = _blockSolver.SolveCombos(comboPreSolver);
            _gameBoard = comboSolver.returnedBoard;
            _boardGUI.SolveCombos(comboSolver);

            yield return new WaitForEndOfFrame();
            _boardGUI.CreateBlockExplosionEffects();
            yield return new WaitForSeconds(BoardDefinition.BlocksExplodingWaitingTime);

            _gameBoard = _blockSolver.MoveBlocksDown(BoardUtils.GetBlocks(_gameBoard));
            _boardGUI.MoveBlocksDown(_gameBoard.GetBlocks(), true);

            yield return new WaitForSeconds(BlockDefinition.MovingSpeedWaitBeforePlayingSound);
            if (comboPreSolver.hasRainbowCombo)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.blockHasRainbowComboSFXClip);
                combosCount++;
            }
            else SoundManager.Instance.PlaySound(SoundManager.Instance.blocksCombosSFXClips, combosCount++);
            yield return new WaitForSeconds(BlockDefinition.MovingSpeedWaitAfterPlayingSound);

            if (_gameBoard.HasAComboInGame())
                yield return new WaitForSeconds(BoardDefinition.WaitForNextComboDelayTime);
        }

        _spawningBoard = _topBlockCreator.CreateBlocks(_gameBoard);
        _boardGUI.CreateBlocks(_spawningBoard.GetBlocks());
        _boardHint.UpdateStatus(_gameBoard, _spawningBoard);

        yield return new WaitForEndOfFrame();

        StartCoroutine(RunGameFlow());
    }

    private void StopGameFlow() => LeanTween.delayedCall(BoardDefinition.WaitForNextComboDelayTime, PickUpWhereLeftOff);
    private void PickUpWhereLeftOff()
    {
        _spawningBoard = _topBlockCreator.CreateBlocks(_gameBoard);
        _boardGUI.CreateBlocks(_spawningBoard.GetBlocks());
        _boardHint.UpdateStatus(_gameBoard, _spawningBoard);

        StartCoroutine(RunGameFlow());
    }
}