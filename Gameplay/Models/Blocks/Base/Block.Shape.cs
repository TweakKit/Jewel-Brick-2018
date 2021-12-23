using System;
using System.Collections.Generic;

using UnityEngine;

using SystemObject = System.Object;

public abstract partial class Block : IEquatable<Block>
{
    private static int IDCounter = 0;

    public readonly int ID;
    public readonly BlockColorAndType blockColorAndType;
    public readonly SquareCoordinate lastWorldCoordinate;
    public readonly SquareCoordinate worldCoordinate;

    public abstract BlockType Type { get; }
    public abstract BlockType TopLeftoverType { get; }
    public abstract BlockType BottomLeftoverType { get; }
    public abstract int ColumnsCount { get; }
    public abstract Vector2 OffsetWorldPosition { get; }
    public abstract List<SquareCoordinate> LocalCoordinates { get; }
    public abstract List<SquareCoordinate> LocalBottomProjectionCoordinates { get; }
    public abstract SquareCoordinate TopLeftoverWorldCoordinate { get; }
    public abstract SquareCoordinate BottomLeftoverWorldCoordinate { get; }
    public abstract Vector2 CenterPosition { get; }

    public List<SquareCoordinate> ComboCoordinates { get; private set; }

    public virtual Vector2 WorldPosition 
    {
        get
        {
            Vector2 worldPosition = BoardDefinition.SquarePositions[worldCoordinate.y, worldCoordinate.x];
            worldPosition += OffsetWorldPosition;
            return worldPosition;
        }
    }

    public virtual List<SquareCoordinate> WorldCoordinates
    {
        get
        {
            List<SquareCoordinate> worldCoodinates = new List<SquareCoordinate>();
            LocalCoordinates.ForEach((localCoordinate) => worldCoodinates.Add(worldCoordinate + localCoordinate));
            return worldCoodinates;
        }
    }

    public Block(int ID, BlockColorAndType blockColorAndType, SquareCoordinate lastWorldCoordinate, SquareCoordinate worldCoordinate)
    {
        this.ID = ID;
        this.blockColorAndType = blockColorAndType;
        this.lastWorldCoordinate = lastWorldCoordinate;
        this.worldCoordinate = worldCoordinate;
    }

    public static int GetID() => IDCounter++;
    public static void ResetID() => IDCounter = 0;

    public virtual Block GetBlockAtNewPosition(Move move)
    {
        Block block = BlockFactory.GetBlock(this.ID, this.blockColorAndType, this.worldCoordinate, move.GetDestinationCoordinate());
        return block;
    }

    public virtual List<SquareCoordinate> GetSpawnedCoordinatesOnBottom(Board gameBoard, Board spawningBoard)
        => GetSpawnedCoordinates(spawningBoard, 0, BoardDefinition.SpawnedSquaresHeight);

    public virtual List<SquareCoordinate> GetSpawnedCoordinatesOnTop(Board gameBoard, Board spawningBoard)
    {
        if (gameBoard.IsAtThreshold())
            return GetSpawnedCoordinates(spawningBoard, BoardDefinition.Height - BoardDefinition.SpawnedSquaresHeight + 1, BoardDefinition.Height);
        else
            return GetSpawnedCoordinates(spawningBoard, BoardDefinition.Height - BoardDefinition.SpawnedSquaresHeight, BoardDefinition.Height);
    }

    public virtual SquareCoordinate GetProjectedCoordinate(Board board)
    {
        // Find the best fit projected coordinate.
        SquareCoordinate bestFitProjectedCoordinate = new SquareCoordinate(-1, -1);

        // For each local bottom projection coordinate of this block, project its respective world coordinate down.
        foreach (var bottomProjectionCoordinate in LocalBottomProjectionCoordinates)
        {
            // Calculate the projecting world coordinate.
            SquareCoordinate projectingCoordinate = worldCoordinate + bottomProjectionCoordinate;

            // Find out the projected world coordinate.
            SquareCoordinate projectedCoordinate = BoardUtils.GetProjectedCoordinate(projectingCoordinate, board.GetSquareStatuses());

            // Re-calculate the projected coordinate to fit the anchor world coordinate of this block.
            projectedCoordinate -= bottomProjectionCoordinate;

            // The best fit projected coordinate is the one that has the highest y (according to physics).
            if (bestFitProjectedCoordinate.y < projectedCoordinate.y)
                bestFitProjectedCoordinate = projectedCoordinate;
        }

        // Return this projected coordinate as the candidate that this block migt be able to move to.
        return bestFitProjectedCoordinate;
    }

    public virtual void Landed(Board board) => UpdateHorizontalDraggableCoordinates(board);
    public virtual void Flash() => ComboCoordinates = new List<SquareCoordinate>(WorldCoordinates);

    public virtual BlockComboCheck CheckCombo(List<SquareCoordinate> comboCoordinates)
    {
        ComboCoordinates = new List<SquareCoordinate>();

        foreach (var comboCoordinate in comboCoordinates)
            if (WorldCoordinates.Contains(comboCoordinate))
                ComboCoordinates.Add(comboCoordinate);

        if (ComboCoordinates.Count > 0)
        {
            if (ComboCoordinates.Count == WorldCoordinates.Count)
                return new BlockComboCheck(BlockExplodedType.Whole);
            else if (this.IsExplodedAtTheBottom(ComboCoordinates))
                return new BlockComboCheck(BlockExplodedType.Part, this.blockColorAndType.blockColor, TopLeftoverType, TopLeftoverWorldCoordinate);
            else
                return new BlockComboCheck(BlockExplodedType.Part, this.blockColorAndType.blockColor, BottomLeftoverType, BottomLeftoverWorldCoordinate);
        }
        else return new BlockComboCheck(BlockExplodedType.None);
    }

    public bool Equals(Block otherBlock)
    {
        if (otherBlock == null)
            return false;
        if (this.ID == otherBlock.ID)
            return true;
        else
            return false;
    }

    public override bool Equals(SystemObject other)
    {
        if (other == null)
            return false;

        Block otherBlock = other as Block;
        if (otherBlock == null)
            return false;
        else
            return Equals(otherBlock);
    }

    public override int GetHashCode() => this.ID.GetHashCode();

    protected virtual List<SquareCoordinate> GetSpawnedCoordinates(Board spawningBoard, int startRowIndex, int endRowIndex)
    {
        // This list collects a list of possible coordinates that this block can move to.
        List<SquareCoordinate> possibleCoordinates = new List<SquareCoordinate>();

        // Check through every square in every row.
        for (int y = startRowIndex; y < endRowIndex; y++)
            // Check through every square in every column.
            for (int x = 0; x < BoardDefinition.Width; x++)
                // If the checking square is empty, meaning it might be one of the possible candidates.
                if (!BoardUtils.IsSquareOccupied(spawningBoard.GetSquareStatuses(), new SquareCoordinate(y, x)))
                    // This is a double/real check to see if this block can be placed nicely in the board.
                    if (BoardUtils.CanPlaceBlock(this, new SquareCoordinate(y, x), spawningBoard.GetSquareStatuses(), endRowIndex))
                        // Put all of the real possible coordinates into the list.
                        possibleCoordinates.Add(new SquareCoordinate(y, x));

        // Return the list with data being shuffled for a random purpose.
        return possibleCoordinates.ShuffleAndReturn();
    }
}