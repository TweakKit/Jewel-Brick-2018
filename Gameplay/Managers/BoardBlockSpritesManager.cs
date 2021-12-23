using System;

using UnityEngine;

public class BoardBlockSpritesManager : MonoBehaviour
{
    [SerializeField]
    private BlockSprite[] _blockSprites;

    public Sprite GetBlockSprite(BlockColorAndType blockColorAndType)
    {
        switch (blockColorAndType.blockColor)
        {
            case 0: // Orange color
                return _blockSprites[0].spriteTypes[(int)blockColorAndType.blockType].sprite;
            case 1: // Blue color
                return _blockSprites[1].spriteTypes[(int)blockColorAndType.blockType].sprite;
            case 2: // Green color
                return _blockSprites[2].spriteTypes[(int)blockColorAndType.blockType].sprite;
            case 3: // Pink color
                return _blockSprites[3].spriteTypes[(int)blockColorAndType.blockType].sprite;
            case 4: // Yellow color
                return _blockSprites[4].spriteTypes[(int)blockColorAndType.blockType].sprite;
            case 5: // Rainbow color
                return _blockSprites[5].spriteTypes[(int)blockColorAndType.blockType].sprite;
        }

        return default;
    }
}

[Serializable]
public class BlockSprite
{
    public string colorName;
    public SpriteType[] spriteTypes;
}

[Serializable]
public class SpriteType
{
    public BlockType blockType;
    public Sprite sprite;
}