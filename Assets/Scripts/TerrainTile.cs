using System;
using Unity.VisualScripting;
using UnityEngine;


public class TerrainTile : MonoBehaviour
{
    [SerializeField]
    public TerrainType type = TerrainType.GRASS;
    [SerializeField]
    public bool walkable = true;
    [SerializeField]
    public TerrainSprite sprites;

    public void Initialize(TerrainType type, bool walkable, TerrainSprite sprites)
    {
        this.type = type;
        this.walkable = walkable;
        this.sprites = sprites;
        Start();
    }

    void Start()
    {
        var renderer = this.GetComponent<SpriteRenderer>();
        renderer.sprite = this.sprites.GetSprite(type);
    }

    void Update()
    {
        
    }


}
