using System;

using UnityEngine;

public static partial class BlockColor
{
    public static bool IsRainbowColor(int blockColor) => blockColor == IndexSpecialBlockColor;
    public static Color GetColorBlockColor(int blockColor) => ColorBlockColors[blockColor];
    public static Color GetColorBlockColorWithAlpha(int blockColor) => ColorBlockColorsWithAlpha[blockColor];
    public static Enum GetBlockExplosionType(int blockColor) => BlockExplosionType[blockColor];
}