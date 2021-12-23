using System;
using System.Collections.Generic;

using UnityEngine;

public class BlockExplosion
{
    public readonly Enum explosionType;
    public readonly List<SquareCoordinate> explosionCoordinates;
    public readonly Vector2 explosionOffset;

    public BlockExplosion(Enum explosionType, List<SquareCoordinate> explosionCoordinates, Vector2 explosionOffset)
    {
        this.explosionType = explosionType;
        this.explosionCoordinates = explosionCoordinates;
        this.explosionOffset = explosionOffset;
    }
}