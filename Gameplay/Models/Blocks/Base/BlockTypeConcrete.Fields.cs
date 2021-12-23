using System;

public static partial class BlockTypeConcrete
{
    public static readonly BlockType[] Group1BlockTypes = new BlockType[]
    {
        BlockType.One_Square,
        BlockType.Two_Square_Horizontal,
        BlockType.Two_Square_Vertical,
    };

    public static readonly BlockType[] Group2BlockTypes = new BlockType[]
    {
        BlockType.Three_Square_L_1,
        BlockType.Three_Square_L_2,
        BlockType.Three_Square_L_3,
        BlockType.Three_Square_L_4,
    };

    public static readonly BlockType[] Group3BlockTypes = new BlockType[]
    {
        BlockType.Three_Square_Horizontal,
        BlockType.Four_Square
    };

    public static readonly BlockType[] OneRowBlockTypes = new BlockType[]
    {
        BlockType.One_Square,
        BlockType.Two_Square_Horizontal,
        BlockType.Three_Square_Horizontal,
    };

    public static readonly Enum[] NormalBlockTypes = new Enum[]
    {
        NormalBlockPoolType.One_Square,
        NormalBlockPoolType.Two_Square_Horizontal,
        NormalBlockPoolType.Two_Square_Vertical,
        NormalBlockPoolType.Three_Square_Horizontal,
        NormalBlockPoolType.Three_Square_L_1,
        NormalBlockPoolType.Three_Square_L_2,
        NormalBlockPoolType.Three_Square_L_3,
        NormalBlockPoolType.Three_Square_L_4,
        NormalBlockPoolType.Four_Square
    };

    public static readonly Enum[] SpecialBlockTypes = new Enum[]
    {
        SpecialBlockPoolType.One_Square,
        SpecialBlockPoolType.Two_Square_Horizontal,
        SpecialBlockPoolType.Two_Square_Vertical,
        SpecialBlockPoolType.Three_Square_Horizontal,
        SpecialBlockPoolType.Three_Square_L_1,
        SpecialBlockPoolType.Three_Square_L_2,
        SpecialBlockPoolType.Three_Square_L_3,
        SpecialBlockPoolType.Three_Square_L_4,
        SpecialBlockPoolType.Four_Square
    };
}

public enum BlockType
{
    One_Square,
    Two_Square_Horizontal,
    Two_Square_Vertical,
    Three_Square_Horizontal,
    Three_Square_L_1,
    Three_Square_L_2,
    Three_Square_L_3,
    Three_Square_L_4,
    Four_Square,
    None
}