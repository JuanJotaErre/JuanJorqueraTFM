using UnityEngine;

public class TerrainSprite : MonoBehaviour
{
    [SerializeField]
    public Sprite grassSprite;
    [SerializeField]
    public Sprite sandSprite;
    

    public Sprite GetSprite(TerrainType type)
    {
        return type switch
        {
            TerrainType.GRASS => grassSprite,
            TerrainType.SAND => sandSprite,
            _ => grassSprite,
        };
    }

}
