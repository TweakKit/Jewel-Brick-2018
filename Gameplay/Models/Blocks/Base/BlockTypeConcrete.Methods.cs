using System;
using System.Linq;
using System.Collections.Generic;

using UnityRandom = UnityEngine.Random;

public static partial class BlockTypeConcrete
{
    private static readonly int shapeGroup1FirstRate = 80;
    private static readonly int shapeGroup2FirstRate = 10;
    private static readonly int shapeGroup3FirstRate = 10;
    private static readonly int shapeGroup1SecondRate = 70;
    private static readonly int shapeGroup2SecondRate = 20;
    private static readonly int shapeGroup3SecondRate = 10;
    private static readonly int shapeGroup1FinalRate = 60;
    private static readonly int shapeGroup2FinalRate = 20;
    private static readonly int shapeGroup3FinalRate = 20;
    private static readonly int rainbowColorAppearFrequencyTurns = 5;
    private static readonly int changeNumberOfBlocksTurnsThreshold = 50;

    private static List<int> selectableBlockColors;
    private static List<int> randomBlockColors;
    private static List<BlockType> randomBlockTypes;
    private static int blocksSpawningTurns = 0;
    private static int rainbowColorWaitingTurns = 0;

    public static void Reset()
    {
        blocksSpawningTurns = 0;
        rainbowColorWaitingTurns = 0;
    }

    public static List<BlockColorAndType> GetRandomBlocksWithColorsAndTypesAtBottom()
    {
        // Randomize the number of blocks.
        int randomNumberOfBlocks = UnityRandom.Range(2, 3 + 1);

        // Randomize the colors of the blocks.
        RandomizeBlockColors(randomNumberOfBlocks);

        // Randomize the types of the blocks.
        RandomizeBlockTypes(randomNumberOfBlocks, false);

        // Return the final list of blocks with colors and types.
        return GetBlocksWithColorsAndTypes(randomNumberOfBlocks);
    }

    public static List<BlockColorAndType> GetRandomBlocksWithColorsAndTypesOnTop(bool isOneRowBlockTypes)
    {
        // Randomize the number of blocks.
        int randomNumberOfBlocks = (isOneRowBlockTypes || blocksSpawningTurns < changeNumberOfBlocksTurnsThreshold)
            ? UnityRandom.Range(2, 3 + 1) : UnityRandom.Range(2, 4 + 1);

        // Randomize the colors of the blocks.
        RandomizeBlockColors(randomNumberOfBlocks);

        // Randomize the types of the blocks.
        RandomizeBlockTypes(randomNumberOfBlocks, isOneRowBlockTypes);

        // Return the final list of blocks with colors and types.
        return GetBlocksWithColorsAndTypes(randomNumberOfBlocks);
    }

    private static void RandomizeBlockColors(int numberOfBlocks)
    {
        rainbowColorWaitingTurns++;

        Action<int> AddToList = (blockColorIndex) =>
        {
            randomBlockColors.Add(blockColorIndex);
            if (randomBlockColors.Where((index) => index == blockColorIndex).Count()
                                                == BlockColor.MaxNumberOfSameBlockColor)
                selectableBlockColors.Remove(blockColorIndex);
        };

        selectableBlockColors = new List<int>(BlockColor.IndexNormalBlockColors);
        randomBlockColors = new List<int>(numberOfBlocks);

        for (int i = 0; i < numberOfBlocks; i++)
            AddToList(selectableBlockColors[UnityRandom.Range(0, selectableBlockColors.Count)]);

        if (rainbowColorWaitingTurns >= rainbowColorAppearFrequencyTurns)
        {
            int removedBlockColorIndex = UnityRandom.Range(0, numberOfBlocks);
            randomBlockColors.RemoveAt(removedBlockColorIndex);
            randomBlockColors.Add(BlockColor.IndexSpecialBlockColor);
            rainbowColorWaitingTurns = 0;
        }
    }

    private static void RandomizeBlockTypes(int numberOfBlocks, bool isOneRowBlockTypes)
    {
        BlockType[] selectedBlockTypes = isOneRowBlockTypes ? OneRowBlockTypes : GetRandomBlockTypes(numberOfBlocks);
        randomBlockTypes = new List<BlockType>(selectedBlockTypes);
    }

    private static List<BlockColorAndType> GetBlocksWithColorsAndTypes(int numberOfBlocks)
    {
        List<BlockColorAndType> blocksWithColorsAndTypes = new List<BlockColorAndType>(numberOfBlocks);

        for (int i = 0; i < numberOfBlocks; i++)
        {
            Enum specificBlockType = GetSpecificBlockType(randomBlockColors[i], randomBlockTypes[i]);
            blocksWithColorsAndTypes.Add(new BlockColorAndType(randomBlockColors[i], randomBlockTypes[i], specificBlockType));
        }

        return blocksWithColorsAndTypes;
    }

    public static Enum GetSpecificBlockType(int blockColor, BlockType blockType)
    {
        if (BlockColor.IsRainbowColor(blockColor))
            return SpecialBlockTypes[(int)blockType];
        else
            return NormalBlockTypes[(int)blockType];
    }

    private static BlockType[] GetRandomBlockTypes(int numberOfBlocks)
    {
        blocksSpawningTurns++;

        List<BlockType> randomBlockTypes = new List<BlockType>();
        int shape2Shape3BlocksCount = 0;

        if (blocksSpawningTurns <= 10)
        {
            for (int i = 0; i < numberOfBlocks; i++)
            {
                if (shape2Shape3BlocksCount <= 2 && UnityRandom.Range(1, 101) <= shapeGroup3FirstRate)
                {
                    randomBlockTypes.Add(Group3BlockTypes.GetRandomFromArray());
                    shape2Shape3BlocksCount++;
                    continue;
                }

                if (shape2Shape3BlocksCount <= 2 && UnityRandom.Range(1, 101) <= shapeGroup2FirstRate)
                {
                    randomBlockTypes.Add(Group2BlockTypes.GetRandomFromArray());
                    shape2Shape3BlocksCount++;
                    continue;
                }

                randomBlockTypes.Add(Group1BlockTypes.GetRandomFromArray());
            }
        }
        else if (blocksSpawningTurns > 10 && blocksSpawningTurns <= 20)
        {
            for (int i = 0; i < numberOfBlocks; i++)
            {
                if (shape2Shape3BlocksCount <= 2 && UnityRandom.Range(1, 101) <= shapeGroup3SecondRate)
                {
                    randomBlockTypes.Add(Group3BlockTypes.GetRandomFromArray());
                    shape2Shape3BlocksCount++;
                    continue;
                }

                if (shape2Shape3BlocksCount <= 2 && UnityRandom.Range(1, 101) <= shapeGroup2SecondRate)
                {
                    randomBlockTypes.Add(Group2BlockTypes.GetRandomFromArray());
                    shape2Shape3BlocksCount++;
                    continue;
                }

                randomBlockTypes.Add(Group1BlockTypes.GetRandomFromArray());
            }
        }
        else
        {
            for (int i = 0; i < numberOfBlocks; i++)
            {
                if (shape2Shape3BlocksCount <= 2 && UnityRandom.Range(1, 101) <= shapeGroup3FinalRate)
                {
                    randomBlockTypes.Add(Group3BlockTypes.GetRandomFromArray());
                    shape2Shape3BlocksCount++;
                    continue;
                }

                if (shape2Shape3BlocksCount <= 2 && UnityRandom.Range(1, 101) <= shapeGroup2FinalRate)
                {
                    randomBlockTypes.Add(Group2BlockTypes.GetRandomFromArray());
                    shape2Shape3BlocksCount++;
                    continue;
                }

                randomBlockTypes.Add(Group1BlockTypes.GetRandomFromArray());
            }
        }

        return randomBlockTypes.ToArray();
    }
}