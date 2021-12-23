using System;

using UnityEngine;

public class BoardSuggestionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _blockSuggestionPrefab;
    [SerializeField]
    private BlockSuggestion[] _blockSuggestions;

    private bool _canUpdateStatus;
    private float _suggestionWaitingTime;
    private bool _isShowingSuggestion;
    private bool _hasHiddenSuggestion;
    private GameObject _blockSuggestionHolder;

    private void Awake()
    {
        EventManager.AddListener<Block>(GameEventType.ShowBlockSuggestion, OnShowBlockSuggestion);
        EventManager.AddListener(GameEventType.HideBlockSuggestion, OnHideBlockSuggestion);
    }

    private void Start() => Init();

    public bool CanShowSuggestion()
    {
        if (_canUpdateStatus)
        {
            _suggestionWaitingTime += Time.deltaTime;
            return _suggestionWaitingTime >= BoardDefinition.SuggestionTime;
        }

        return false;
    }

    public bool HasHiddenSuggestion() => _hasHiddenSuggestion;
    public void Restore() => _hasHiddenSuggestion = false;

    private void Init()
    {
        _canUpdateStatus = true;
        _suggestionWaitingTime = 0.0f;
        _isShowingSuggestion = false;
        _hasHiddenSuggestion = false;

        _blockSuggestionHolder = Instantiate(_blockSuggestionPrefab);
        _blockSuggestionHolder.transform.SetParent(transform);
        _blockSuggestionHolder.SetActive(false);
    }

    private void OnShowBlockSuggestion(Block block)
    {
        _canUpdateStatus = false;
        _isShowingSuggestion = true;

        Sprite suggestionSprite = _blockSuggestions[(int)block.blockColorAndType.blockType].sprite;
        _blockSuggestionHolder.GetComponent<SpriteRenderer>().sprite = suggestionSprite;
        SquareCoordinate suggestionCoordinate = block.worldCoordinate + new SquareCoordinate(0, block.DraggingSquaresCount);
        _blockSuggestionHolder.transform.position = BoardDefinition.SquarePositions[suggestionCoordinate.y, suggestionCoordinate.x]
            + block.OffsetWorldPosition;
        _blockSuggestionHolder.SetActive(true);
    }

    private void OnHideBlockSuggestion()
    {
        if (_isShowingSuggestion)
        {
            _hasHiddenSuggestion = true;
            _blockSuggestionHolder.SetActive(false);
        }

        _canUpdateStatus = true;
        _suggestionWaitingTime = 0.0f;
        _isShowingSuggestion = false;
    }
}

[Serializable]
public struct BlockSuggestion
{
    public BlockType blockType;
    public Sprite sprite;
}