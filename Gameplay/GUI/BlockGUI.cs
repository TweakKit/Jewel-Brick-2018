using UnityEngine;

public class BlockGUI : MonoBehaviour
{
    private GameObject _blockShapeGraphic;
    private GameObject _blockShadowGraphic;
    private GameObject _blockOverLayerGraphic;
    private GameObject _hintArrowGraphic;
    private GameObject _ellectricEffectGraphic;
    private Vector2 _currentVirtualCenterPosition;
    private Vector2 _selectedPosition;
    private Vector3 _hintArrowOriginalScale;

    public bool IsDraggable { get; private set; }
    public Block OwnerBlock { get; private set; }

    public delegate void ExplodeInBoardEventHandler(BlockGUI blockGUI);
    public event ExplodeInBoardEventHandler ExplodeInBoard;
    public delegate void DeleteFromBoardEventHandler(BlockGUI blockGUI);
    public event DeleteFromBoardEventHandler DeleteFromBoard;

    private void Awake()
    {
        _blockShapeGraphic = gameObject;
        _blockShadowGraphic = transform.FindInChildren(BlockDefinition.ShadowGraphicName).gameObject;
        _blockOverLayerGraphic = transform.FindInChildren(BlockDefinition.OverLayerName).gameObject;
        _hintArrowGraphic = transform.FindInChildren(BlockDefinition.HintArrowGraphicName).gameObject;
        _ellectricEffectGraphic = transform.FindInChildren(BlockDefinition.EllectricGraphicName).gameObject;
        _hintArrowOriginalScale = _hintArrowGraphic.transform.localScale;
    }

    public void SetModel(Block block) => OwnerBlock = block;

    public void Init(Sprite sprite, float alpha, ExplodeInBoardEventHandler explodeInBoardEventHandler, DeleteFromBoardEventHandler deleteFromBoardEventHandler)
    {
        IsDraggable = false;
        _blockShapeGraphic.GetComponent<SpriteRenderer>().sprite = sprite;
        _blockShadowGraphic.GetComponent<SpriteRenderer>().sprite = sprite;
        _blockShapeGraphic.GetComponent<SpriteRenderer>().SetAlpha(alpha);
        _blockShadowGraphic.GetComponent<SpriteRenderer>().SetAlpha(BlockDefinition.BlurredColorAlpha);
        _ellectricEffectGraphic.gameObject.SetActive(false);
        ExplodeInBoard += explodeInBoardEventHandler;
        DeleteFromBoard += deleteFromBoardEventHandler;
    }

    public void InitialMove(Vector2 toPosition)
    {
        transform.position = toPosition;
    }

    public void PreMove()
    {
        IsDraggable = true;
        _blockShapeGraphic.GetComponent<SpriteRenderer>().SetAlpha(BlockDefinition.NormalColorAlpha);
    }

    public void Move(Vector2 fromPosition, Vector2 toPosition, bool gradual)
    {
        if (fromPosition != toPosition)
        {
            if (gradual)
                StartCoroutine(Lerp.InterpolatePositions(transform, fromPosition, toPosition, BlockDefinition.MovingSpeed));
            else
                transform.position = toPosition;
        }
    }

    public void StartSliding(float direction)
    {
        _hintArrowGraphic.transform.SetLocalScaleX(direction * _hintArrowOriginalScale.x);
        _hintArrowGraphic.GetComponent<SpriteRenderer>().color = BlockColor.GetColorBlockColorWithAlpha(OwnerBlock.blockColorAndType.blockColor);
        _hintArrowGraphic.transform.GetChild(0).GetComponent<SpriteRenderer>().color = BlockColor.GetColorBlockColor(OwnerBlock.blockColorAndType.blockColor);
        _hintArrowGraphic.SetActive(true);

        Vector2 fromPosition = OwnerBlock.WorldPosition;
        Vector2 toPosition = OwnerBlock.WorldPosition + Vector2.right * direction * BlockDefinition.SlidingDeltaDistance;
        StartCoroutine(Lerp.InterpolatePositionsPingpong(transform, fromPosition, toPosition, BlockDefinition.SlidingSpeed));
    }

    public void StopSliding()
    {
        StopAllCoroutines();
        _hintArrowGraphic.SetActive(false);
        Move(transform.position, OwnerBlock.WorldPosition, false);
    }

    public void Selected()
    {
        _currentVirtualCenterPosition = OwnerBlock.CenterPosition;
        _selectedPosition = OwnerBlock.WorldPosition;

        _blockShadowGraphic.SetActive(true);
        _blockShadowGraphic.transform.localPosition = Vector2.zero;

        OwnerBlock.DraggingSquaresCount = 0;
    }

    public void MoveByMouse(Vector2 mousePosition)
    {
        // Move this block's shadow horizontally while getting it claimed in an appropriately moveable range.
        float min = BoardDefinition.SquarePositions[OwnerBlock.LeftmostDraggableCoordinate.y, OwnerBlock.LeftmostDraggableCoordinate.x].x + OwnerBlock.OffsetWorldPosition.x;
        float max = BoardDefinition.SquarePositions[OwnerBlock.RightmostDraggableCoordinate.y, OwnerBlock.RightmostDraggableCoordinate.x].x + OwnerBlock.OffsetWorldPosition.x;
        mousePosition.x = Mathf.Clamp(mousePosition.x, min, max);

        // Move this block's shape to the new position.
        _blockShapeGraphic.transform.position = new Vector2(mousePosition.x, transform.position.y);
        _blockShadowGraphic.transform.position = _selectedPosition;

        // Calculate the dragging squares count to see how many squares away from the block's current sitting origin.
        float draggingDelta = _blockShapeGraphic.transform.position.x + (OwnerBlock.CenterPosition.x - OwnerBlock.WorldPosition.x) - _currentVirtualCenterPosition.x;
        if (Mathf.Abs(draggingDelta) > BoardDefinition.HalfDistanceBetweenSquares)
        {
            OwnerBlock.DraggingSquaresCount += (int)Mathf.Sign(draggingDelta);
            _currentVirtualCenterPosition.x += Mathf.Sign(draggingDelta) * BoardDefinition.DistanceBetweenSquares;
        }

        // Show the board hint to help players know where they are dragging and placing this block.
        EventManager.Invoke(GameEventType.ShowBoardHint, OwnerBlock.worldCoordinate.x, OwnerBlock.ColumnsCount, OwnerBlock.DraggingSquaresCount);
    }

    public bool UnSelected()
    {
        EventManager.Invoke(GameEventType.HideBoardHint);

        _blockShadowGraphic.SetActive(false);
        _blockShapeGraphic.transform.position = _selectedPosition;

        return OwnerBlock.DraggingSquaresCount != 0;
    }

    public void Flash()
    {
        OwnerBlock.Flash();
        _ellectricEffectGraphic.gameObject.SetActive(true);
    }

    public void Explode()
    {
        ExplodeInBoard?.Invoke(this);
        PoolManager.ReturnObjectToPool(gameObject);
    }

    public void Delete()
    {
        DeleteFromBoard?.Invoke(this);
        StartCoroutine(Lerp.FadeToDisappear
        (
            _blockShapeGraphic,
            BlockDefinition.DisappearingSpeed,
            () => PoolManager.ReturnObjectToPool(gameObject))
        );
    }

    public void Transform()
    {
        _blockOverLayerGraphic.SetActive(true);
        StartCoroutine(Lerp.TransformColor(_blockShapeGraphic, _blockOverLayerGraphic, BlockDefinition.TransformingSpeed));
    }
}