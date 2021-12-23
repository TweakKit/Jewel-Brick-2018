public struct BlockComboCheck
{
    public readonly BlockExplodedType blockExplodedType;
    public readonly BlockColorAndType blockColorAndType;
    public readonly SquareCoordinate leftoverBlockCoordinate;

    public BlockComboCheck(BlockExplodedType blockExplodedType)
    {
        this.blockExplodedType = blockExplodedType;
        this.blockColorAndType = new BlockColorAndType();
        this.leftoverBlockCoordinate = null;
    }

    public BlockComboCheck(BlockExplodedType blockExplodedType, int blockColor, BlockType blockType, SquareCoordinate leftoverBlockCoordinate)
    {
        this.blockExplodedType = blockExplodedType;
        this.blockColorAndType = new BlockColorAndType(blockColor, blockType, BlockTypeConcrete.GetSpecificBlockType(blockColor, blockType));
        this.leftoverBlockCoordinate = leftoverBlockCoordinate;
    }
}