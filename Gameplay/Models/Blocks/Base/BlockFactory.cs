public static class BlockFactory
{
    public static Block GetBlock(Block block)
    {
        if (block != null)
            return GetBlock(block.ID, block.blockColorAndType, block.lastWorldCoordinate, block.worldCoordinate);

        return null;
    }

    public static Block GetBlock(int ID, BlockColorAndType blockColorAndType, SquareCoordinate lastWorldCoordinate, SquareCoordinate worldCoordinate)
    {
        switch (blockColorAndType.blockType)
        {
            case BlockType.One_Square:
                return new OneSquareBlock(ID, blockColorAndType, lastWorldCoordinate, worldCoordinate);

            case BlockType.Two_Square_Horizontal:
                return new TwoSquareHorizontalBlock(ID, blockColorAndType, lastWorldCoordinate, worldCoordinate);

            case BlockType.Two_Square_Vertical:
                return new TwoSquareVerticalBlock(ID, blockColorAndType, lastWorldCoordinate, worldCoordinate);

            case BlockType.Three_Square_Horizontal:
                return new ThreeSquareHorizontalBlock(ID, blockColorAndType, lastWorldCoordinate, worldCoordinate);

            case BlockType.Three_Square_L_1:
                return new ThreeSquareL1Block(ID, blockColorAndType, lastWorldCoordinate, worldCoordinate);

            case BlockType.Three_Square_L_2:
                return new ThreeSquareL2Block(ID, blockColorAndType, lastWorldCoordinate, worldCoordinate);

            case BlockType.Three_Square_L_3:
                return new ThreeSquareL3Block(ID, blockColorAndType, lastWorldCoordinate, worldCoordinate);

            case BlockType.Three_Square_L_4:
                return new ThreeSquareL4Block(ID, blockColorAndType, lastWorldCoordinate, worldCoordinate);

            case BlockType.Four_Square:
                return new FourSquareBlock(ID, blockColorAndType, lastWorldCoordinate, worldCoordinate);
        }

        return null;
    }
}