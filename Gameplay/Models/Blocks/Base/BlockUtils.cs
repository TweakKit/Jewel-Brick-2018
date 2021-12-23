using System.Linq;
using System.Collections.Generic;

public static class BlockUtils
{
    public static bool IsRainbowBlock(this Block block) => BlockColor.IsRainbowColor(block.blockColorAndType.blockColor);

    public static Block GetBlockWithHighMoveablePriority(List<Block> blocks, bool[,] squareStatuses)
    {
        // Get the list of blocks that have no obstacles below them as they are the ones that need to move first.        
        List<Block> freeBlocks = GetBlocksHasNoObstaclesBelow(blocks, squareStatuses);

        // Return the block that has no obstacles below them and has the lowest y coordinate (according to physics).
        return GetFreeBlockWithLowestCoordinateY(freeBlocks);
    }

    public static bool ContainComboCoordinate(this Block block, List<SquareCoordinate> comboCoordinates)
    {
        foreach (var comboCoordinate in comboCoordinates)
            if (block.ContainCoordinate(comboCoordinate))
                return true;

        return false;
    }

    public static bool ContainCoordinate(this Block block, SquareCoordinate coordinate)
    {
        foreach (var worldCoordinate in block.WorldCoordinates)
            if (worldCoordinate.Equals(coordinate))
                return true;

        return false;
    }

    public static List<Block> GetNeighbourBlocks(this Block block, Board gameBoard)
    {
        List<Block> neighbourBlocks = new List<Block>();

        foreach (var worldCoordinate in block.WorldCoordinates)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (y != 0)
                {
                    SquareCoordinate tempCoordinate = worldCoordinate + new SquareCoordinate(y, 0);
                    Block blockAtCoordinate = BoardUtils.GetBlockAtCoordinate(gameBoard, tempCoordinate);
                    if (blockAtCoordinate != null && blockAtCoordinate != block)
                        if (!neighbourBlocks.Contains(blockAtCoordinate))
                            neighbourBlocks.Add(blockAtCoordinate);
                }
            }

            for (int x = -1; x <= 1; x++)
            {
                if (x != 0)
                {
                    SquareCoordinate tempCoordinate = worldCoordinate + new SquareCoordinate(0, x);
                    Block blockAtCoordinate = BoardUtils.GetBlockAtCoordinate(gameBoard, tempCoordinate);
                    if (blockAtCoordinate != null && blockAtCoordinate != block)
                        if (!neighbourBlocks.Contains(blockAtCoordinate))
                            neighbourBlocks.Add(blockAtCoordinate);
                }
            }
        }

        return neighbourBlocks;
    }

    public static bool IsExplodedAtTheBottom(this Block block, List<SquareCoordinate> comboCoordinates)
    {
        int lowestComboCoordinateY = comboCoordinates.Min((comboCoordinate) => comboCoordinate.y);
        return lowestComboCoordinateY == block.worldCoordinate.y;
    }

    public static bool IsDubbedBottomBlock(this Block block) => block.worldCoordinate.y <= 6;

    private static List<Block> GetBlocksHasNoObstaclesBelow(List<Block> blocks, bool[,] squareStatuses)
    {
        List<Block> freeBlocks = new List<Block>();

        foreach (var block in blocks)
            if (!block.HasObstaclesBelow(squareStatuses))
                freeBlocks.Add(block);

        return freeBlocks;
    }

    private static Block GetFreeBlockWithLowestCoordinateY(List<Block> freeBlocks)
    {
        int lowestCoordinateY = freeBlocks.Min((freeBlock) => freeBlock.worldCoordinate.y);
        return freeBlocks.First((freeBlock) => freeBlock.worldCoordinate.y == lowestCoordinateY);
    }

    private static bool HasObstaclesBelow(this Block checkedBlock, bool[,] squareStatuses)
    {
        foreach (var localBottomProjectionCoordinate in checkedBlock.LocalBottomProjectionCoordinates)
        {
            SquareCoordinate tempCoordinate = checkedBlock.worldCoordinate + localBottomProjectionCoordinate;
            tempCoordinate = tempCoordinate - new SquareCoordinate(1, 0);

            if (tempCoordinate.y < 0)
                return false;

            if (BoardUtils.IsSquareOccupied(squareStatuses, tempCoordinate))
                return true;
        }

        return false;
    }
}