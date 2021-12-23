using System;
using System.Collections.Generic;

using UnityEngine;

public static partial class BlockColor
{
    public static readonly int MaxNumberOfSameBlockColor = 2;
    public static readonly int IndexSpecialBlockColor = 5; // Rainbow

    public static readonly List<int> IndexNormalBlockColors = new List<int>()
    {
        0, // Orange
        1, // Blue
        2, // Green
        3, // Pink
        4, // Yellow
    };

    public static readonly List<Color> ColorBlockColors = new List<Color>
    {
        new Color(0.8509804f, 0.0f,       0.02352941f, 1.0f), // Orange
        new Color(0.0f,       0.7843137f, 1.0f,        1.0f), // Blue
        new Color(0.0f,       0.5647059f, 0.01154211f, 1.0f), // Green
        new Color(1.0f,       0.5137255f, 0.8784314f,  1.0f), // Pink
        new Color(0.7019608f, 0.4196078f, 0.0f,        1.0f), // Yellow
        new Color(0.0f,       0.0f,       0.0f,        1.0f), // Rainbow
    };

    public static readonly List<Color> ColorBlockColorsWithAlpha = new List<Color>
    {
        new Color(0.8509804f, 0.0f,       0.02352941f, 0.6f), // Orange
        new Color(0.0f,       0.7843137f, 1.0f,        0.6f), // Blue
        new Color(0.0f,       0.5647059f, 0.01154211f, 0.6f), // Green
        new Color(1.0f,       0.5137255f, 0.8784314f,  0.6f), // Pink
        new Color(0.7019608f, 0.4196078f, 0.0f,        0.6f), // Yellow
        new Color(0.0f,       0.0f,       0.0f,        0.6f), // Rainbow
    };

    public static readonly Enum[] BlockExplosionType = new Enum[]
    {
        BlockExplosionPoolType.OrangeBlockExplosion,
        BlockExplosionPoolType.BlueBlockExplosion,
        BlockExplosionPoolType.GreenBlockExplosion,
        BlockExplosionPoolType.PinkBlockExplosion,
        BlockExplosionPoolType.YellowBlockExplosion,
        BlockExplosionPoolType.RainbowBlockExplosion,
    };
}