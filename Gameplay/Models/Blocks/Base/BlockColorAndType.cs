using System;

public struct BlockColorAndType
{
    public readonly int blockColor;
    public readonly BlockType blockType;
    public readonly Enum specificBlockType;

    public BlockColorAndType(int blockColor, BlockType blockType, Enum specificBlockType)
    {
        this.blockColor = blockColor;
        this.blockType = blockType;
        this.specificBlockType = specificBlockType;
    }
}