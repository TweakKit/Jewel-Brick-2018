using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BoardGUI : MonoBehaviour
{
    private List<BlockGUI> _blockGUIs;
    private List<BlockExplosion> _blockExplosions;
    private BoardBlockSpritesManager _boardBlockSpritesManager;

    private void Awake()
    {
        _blockGUIs = new List<BlockGUI>();
        _boardBlockSpritesManager = gameObject.GetComponent<BoardBlockSpritesManager>();
        EventManager.AddListener(GameEventType.GameFail, StartBreakinglocks);
    }

    public void CreateBlocks(List<Block> blocks, float alpha = BlockDefinition.BlurredColorAlpha)
    {
        foreach (var block in blocks)
        {
            GameObject blockGameObject = PoolManager.GetObjectFromPool(block.blockColorAndType.specificBlockType);
            BlockGUI blockGUI = blockGameObject.GetOrAddComponent<BlockGUI>();

            blockGUI.SetModel(block);
            blockGUI.Init(_boardBlockSpritesManager.GetBlockSprite(block.blockColorAndType), alpha, OnExplodeABlockGUIInBoard, OnDeleteABlockGUIFromBoard);
            blockGUI.InitialMove
            (
                BoardDefinition.SquarePositions[block.worldCoordinate.y, block.worldCoordinate.x] + block.OffsetWorldPosition
            );

            _blockGUIs.Add(blockGUI);
        }
    }

    public void MoveBlocksDown(List<Block> blocks, bool gradual)
    {
        UpdateBoard(blocks);

        foreach (var blockGUI in _blockGUIs)
        {
            blockGUI.PreMove();
            blockGUI.Move
            (
                BoardDefinition.SquarePositions[blockGUI.OwnerBlock.lastWorldCoordinate.y, blockGUI.OwnerBlock.lastWorldCoordinate.x] + blockGUI.OwnerBlock.OffsetWorldPosition,
                BoardDefinition.SquarePositions[blockGUI.OwnerBlock.worldCoordinate.y, blockGUI.OwnerBlock.worldCoordinate.x] + blockGUI.OwnerBlock.OffsetWorldPosition,
                gradual
            );
        }
    }

    public void MoveABlockHorizontal(List<Block> blocks, Block draggedBlock)
    {
        UpdateBoard(blocks);

        BlockGUI draggedBlockGUI = _blockGUIs.First((blockGUI) => blockGUI.OwnerBlock.ID == draggedBlock.ID);
        draggedBlockGUI.Move
        (
            BoardDefinition.SquarePositions[draggedBlockGUI.OwnerBlock.lastWorldCoordinate.y, draggedBlockGUI.OwnerBlock.lastWorldCoordinate.x] + draggedBlockGUI.OwnerBlock.OffsetWorldPosition,
            BoardDefinition.SquarePositions[draggedBlockGUI.OwnerBlock.worldCoordinate.y, draggedBlockGUI.OwnerBlock.worldCoordinate.x] + draggedBlockGUI.OwnerBlock.OffsetWorldPosition,
            false
        );
    }

    public void StartSlidingABlock(Block bestAIBlock)
    {
        BlockGUI slidedBlockGUI = _blockGUIs.First((blockGUI) => blockGUI.OwnerBlock.ID == bestAIBlock.ID);
        slidedBlockGUI.StartSliding(Mathf.Sign(bestAIBlock.DraggingSquaresCount));
    }

    public void StopSlidingABlock(Block bestAIBlock)
    {
        BlockGUI slidedBlockGUI = _blockGUIs.First((blockGUI) => blockGUI.OwnerBlock.ID == bestAIBlock.ID);
        slidedBlockGUI.StopSliding();
    }

    public void PreSolveCombos(ComboPreSolver comboPreSolver)
    {
        comboPreSolver.rainbowComboExplodedBlocks.ForEach((block) =>
        {
            BlockGUI flashBlockGUI = _blockGUIs.First((blockGUI) => blockGUI.OwnerBlock.ID == block.ID);
            flashBlockGUI.Flash();
        });
    }

    public void SolveCombos(ComboSolver comboSolver)
    {
        _blockExplosions = new List<BlockExplosion>();
        comboSolver.explodedBlocks.ForEach((block) =>
        {
            BlockGUI explodedBlockGUI = _blockGUIs.First((blockGUI) => blockGUI.OwnerBlock.ID == block.ID);
            explodedBlockGUI.Explode();
        });

        UpdateBoard(comboSolver.returnedBoard.GetBlocks());

        List<Block> onBoardBlocks = new List<Block>();
        _blockGUIs.ForEach((blockGUI) => onBoardBlocks.Add(blockGUI.OwnerBlock));

        List<Block> comboBlocks = comboSolver.returnedBoard.GetBlocks().Except(onBoardBlocks).ToList();
        CreateBlocks(comboBlocks, BlockDefinition.NormalColorAlpha);
    }

    public void CreateBlockExplosionEffects()
    {
        foreach (var blockExplosion in _blockExplosions)
        {
            blockExplosion.explosionCoordinates.ForEach((explosionCoordinate) =>
            {
                GameObject blockExplosionGameObject = PoolManager.GetObjectFromPool(blockExplosion.explosionType);
                blockExplosionGameObject.transform.position = BoardDefinition.SquarePositions[explosionCoordinate.y, explosionCoordinate.x] + blockExplosion.explosionOffset;
            });
        }

        _blockExplosions = null;
    }

    public void Delete6BottomRows()
    {
        List<Block> bottomBlocks = new List<Block>();
        _blockGUIs.ForEach((blockGUI) =>
        {
            if (blockGUI.OwnerBlock.IsDubbedBottomBlock())
                bottomBlocks.Add(blockGUI.OwnerBlock);
        });

        bottomBlocks.ForEach((block) =>
        {
            BlockGUI deletedBlockGUI = _blockGUIs.First((blockGUI) => blockGUI.OwnerBlock.ID == block.ID);
            deletedBlockGUI.Delete();
        });
    }

    public List<Block> GetBlocks()
    {
        List<Block> blocks = new List<Block>();
        _blockGUIs.ForEach((blockGUI) => blocks.Add(blockGUI.OwnerBlock));
        return blocks;
    }

    private void StartBreakinglocks() => StartCoroutine(BreakBlocks());

    private void UpdateBoard(List<Block> blocks)
    {
        foreach (var blockGUI in _blockGUIs)
        {
            foreach (var block in blocks)
            {
                if (blockGUI.OwnerBlock.ID == block.ID)
                {
                    blockGUI.SetModel(block);
                    break;
                }
            }
        }
    }

    private IEnumerator BreakBlocks()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.gameOverSFXClip);

        List<BlockGUI> brokenBlockGUIs = new List<BlockGUI>();
        while (_blockGUIs.Count > 0)
        {
            BlockGUI prioritizedBlockGUI = GetBlockGUIWithHighestPriority(_blockGUIs);
            brokenBlockGUIs.Add(prioritizedBlockGUI);
            prioritizedBlockGUI.Transform();
            yield return new WaitForSeconds(BoardDefinition.BlocksTransformingIntervalTime);
        }

        yield return new WaitForSeconds(BoardDefinition.WaitForOpeningGameOverPopUp);
        EventManager.Invoke(GameEventType.GameOver);
    }

    private BlockGUI GetBlockGUIWithHighestPriority(List<BlockGUI> blockGUIs)
    {
        int highestCoordinateY = blockGUIs.Max((blockGUI) => blockGUI.OwnerBlock.worldCoordinate.y);
        List<BlockGUI> highestCoordinateYBlockGUIs = blockGUIs.Where((blockGUI) => blockGUI.OwnerBlock.worldCoordinate.y == highestCoordinateY).ToList();
        int lowestCoordinateX = highestCoordinateYBlockGUIs.Min((blockGUI) => blockGUI.OwnerBlock.worldCoordinate.x);
        BlockGUI prioritizedBlockGUI = highestCoordinateYBlockGUIs.First((blockGUI) => blockGUI.OwnerBlock.worldCoordinate.x == lowestCoordinateX);
        blockGUIs.Remove(prioritizedBlockGUI);
        return prioritizedBlockGUI;
    }

    private void OnExplodeABlockGUIInBoard(BlockGUI blockGUI)
    {
        BlockExplosion blockExplosion = new BlockExplosion(BlockColor.GetBlockExplosionType(blockGUI.OwnerBlock.blockColorAndType.blockColor),
                                                           blockGUI.OwnerBlock.ComboCoordinates,
                                                           blockGUI.OwnerBlock.OffsetWorldPosition);
        _blockExplosions.Add(blockExplosion);
        _blockGUIs.Remove(blockGUI);
    }

    private void OnDeleteABlockGUIFromBoard(BlockGUI blockGUI)
    {
        _blockGUIs.Remove(blockGUI);
    }
}