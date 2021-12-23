using System.Linq;
using System.Collections.Generic;

public abstract partial class Block
{
    public int DraggingSquaresCount { get; set; }
    public List<SquareCoordinate> HorizontalDraggableCoordinates { get; private set; }
    public SquareCoordinate LeftmostDraggableCoordinate { get; private set; }
    public SquareCoordinate RightmostDraggableCoordinate { get; private set; }

    protected virtual void UpdateHorizontalDraggableCoordinates(Board board)
    {
        // First thing first, build a board that doesn't contain this block.
        List<Block> remainingBlocks = new List<Block>(board.GetBlocks()).RemoveAndReturn(this);
        Board checkingBoard = new BoardBuilder(remainingBlocks).Build();

        // Reset these lists before populating new data into it.
        List<SquareCoordinate> leftDraggableCoordinates = new List<SquareCoordinate>();
        List<SquareCoordinate> rightDraggableCoordinates = new List<SquareCoordinate>();

        // Update the list of left draggable coordinates for this block for every finished move.
        SquareCoordinate leftTempCoordinate = worldCoordinate;
        while ((leftTempCoordinate -= new SquareCoordinate(0, 1)).x >= 0)
        {
            if (BoardUtils.CanPlaceBlock(this, leftTempCoordinate, checkingBoard.GetSquareStatuses(), BoardDefinition.Height))
                leftDraggableCoordinates.Add(leftTempCoordinate);
            else
                break;
        }

        // Update the list of right draggable coordinates for this block for every finished move.
        SquareCoordinate rightTempCoordinate = worldCoordinate;
        while ((rightTempCoordinate += new SquareCoordinate(0, 1)).x < BoardDefinition.Width)
        {
            if (BoardUtils.CanPlaceBlock(this, rightTempCoordinate, checkingBoard.GetSquareStatuses(), BoardDefinition.Height))
                rightDraggableCoordinates.Add(rightTempCoordinate);
            else
                break;
        }

        // Update the leftmost draggable coordinate for this block.
        if (leftDraggableCoordinates.Count > 0)
        {
            int leftmostDraggableCoordinateX = leftDraggableCoordinates.Min((moveableCoordinate) => moveableCoordinate.x);
            LeftmostDraggableCoordinate = leftDraggableCoordinates.First((moveableCoordinate) => moveableCoordinate.x == leftmostDraggableCoordinateX);
        }
        else LeftmostDraggableCoordinate = worldCoordinate;

        // Update the rightmost draggable coordinate for this block.
        if (rightDraggableCoordinates.Count > 0)
        {
            int rightmostDraggableCoordinateX = rightDraggableCoordinates.Max((moveableCoordinate) => moveableCoordinate.x);
            RightmostDraggableCoordinate = rightDraggableCoordinates.First((moveableCoordinate) => moveableCoordinate.x == rightmostDraggableCoordinateX);
        }
        else RightmostDraggableCoordinate = worldCoordinate;

        // Update the list of horizontal draggable coordinates.
        HorizontalDraggableCoordinates = new List<SquareCoordinate>(leftDraggableCoordinates).AddRangeAndReturn(rightDraggableCoordinates);
    }
}