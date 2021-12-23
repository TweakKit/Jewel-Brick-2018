public static class BlockDefinition
{
    public const float NormalColorAlpha = 1;
    public const float BlurredColorAlpha = 0.6f;
    public const string ShadowGraphicName = "Shadow";
    public const string OverLayerName = "OverLayer";
    public const string HintArrowGraphicName = "HintArrowBackground";
    public const string EllectricGraphicName = "Ellectric";
    public const string LayerMaskName = "Block";

    public static readonly float MovingSpeed = 5.0f;
    public static readonly float FlashingSpeed = 1.0f;
    public static readonly float SlidingDeltaDistance = 0.2f;
    public static readonly float SlidingSpeed = 2.0f;
    public static readonly float TransformingSpeed = 5.0f;
    public static readonly float DisappearingSpeed = 2.0f;

    public static readonly float MovingSpeedWaitBeforePlayingSound = (1.0f / MovingSpeed) * 0.4f;
    public static readonly float MovingSpeedWaitAfterPlayingSound = (1.0f / MovingSpeed) * 0.6f;
}