using System.Collections.Generic;

using UnityEngine;

public class BoardHintManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _hintLayerPrefab;
    [SerializeField]
    private GameObject _warningLayer;

    private GameObject[,] _hintLayerGameObjects;
    private int _currentDraggedBlockCoordinateX;
    private int _currentDraggedBlockColumnsCount;
    private int _currentDraggedBlockDraggingSquaresCount;
    private bool[,] _boardSquareStatuses;

    private void Awake()
    {
        EventManager.AddListener<int, int, int>(GameEventType.ShowBoardHint, OnShowBoardHint);
        EventManager.AddListener(GameEventType.HideBoardHint, OnHideBoardHint);
        EventManager.AddListener(GameEventType.ShowWarningLayer, OnShowWarningLayer);
        EventManager.AddListener(GameEventType.HideWarningLayer, OnHideWarningLayer);
    }

    private void Start()
    {
        Init();
        Restore();
    }

    private void Init()
    {
        _boardSquareStatuses = new bool[BoardDefinition.Height, BoardDefinition.Width];
        _hintLayerGameObjects = new GameObject[BoardDefinition.Height, BoardDefinition.Width];

        for (int y = 0; y < BoardDefinition.Height; y++)
        {
            for (int x = 0; x < BoardDefinition.Width; x++)
            {
                GameObject hintLayerGameObject = Instantiate(_hintLayerPrefab);
                hintLayerGameObject.transform.position = BoardDefinition.SquarePositions[y, x];
                hintLayerGameObject.transform.SetParent(transform);
                hintLayerGameObject.SetActive(false);
                _hintLayerGameObjects[y, x] = hintLayerGameObject;
            }
        }
    }

    public void UpdateStatus(Board gameBoard, Board spawningBoard)
    {
        List<Block> allBlocks = BoardUtils.GetBlocks(gameBoard, spawningBoard);
        Board hintBoard = new BoardBuilder(allBlocks).Build();
        _boardSquareStatuses = hintBoard.GetSquareStatuses();
    }

    private void OnShowBoardHint(int blockCoordinateX, int blockColumnsCount, int draggingSquaresCount)
    {
        if (_currentDraggedBlockCoordinateX == blockCoordinateX
        && _currentDraggedBlockColumnsCount == blockColumnsCount
        && _currentDraggedBlockDraggingSquaresCount == draggingSquaresCount)
            return;

        HideHints();

        _currentDraggedBlockCoordinateX = blockCoordinateX;
        _currentDraggedBlockColumnsCount = blockColumnsCount;
        _currentDraggedBlockDraggingSquaresCount = draggingSquaresCount;

        for (int x = blockCoordinateX + draggingSquaresCount; x < blockCoordinateX + blockColumnsCount + draggingSquaresCount; x++)
            for (int y = 0; y < BoardDefinition.Height; y++)
                if (!_boardSquareStatuses[y, x])
                    _hintLayerGameObjects[y, x].SetActive(true);
    }

    private void OnHideBoardHint()
    {
        Restore();
        HideHints();
    }

    private void OnShowWarningLayer()
    {
        _warningLayer.GetComponent<Animator>().ResetTrigger(BoardDefinition.StopWarningTriggerParameter);
        _warningLayer.GetComponent<Animator>().SetTrigger(BoardDefinition.StartWarningTriggerParameter);
    }

    private void OnHideWarningLayer()
    {
        _warningLayer.GetComponent<Animator>().ResetTrigger(BoardDefinition.StartWarningTriggerParameter);
        _warningLayer.GetComponent<Animator>().SetTrigger(BoardDefinition.StopWarningTriggerParameter);
    }

    private void HideHints()
    {
        for (int y = 0; y < BoardDefinition.Height; y++)
            for (int x = 0; x < BoardDefinition.Width; x++)
                _hintLayerGameObjects[y, x].gameObject.SetActive(false);
    }

    private void Restore()
    {
        _currentDraggedBlockCoordinateX = -1;
        _currentDraggedBlockColumnsCount = -1;
        _currentDraggedBlockDraggingSquaresCount = -1;
    }
}