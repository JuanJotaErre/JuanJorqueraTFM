using System;
using UnityEngine;

public class TerrainController : MonoBehaviour
{

    [SerializeField]
    TerrainSprite sprites;

    GameObject[,] tiles = new GameObject[100,100];

    public GameObject tilePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                tiles[i,j] = this.BuildTile(i,j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject BuildTile(int i, int j)
    {
        var position = new Vector3(i-50, j-50, 1);
        var gameObject = Instantiate(tilePrefab, position, Quaternion.identity);
        var tileComponent = gameObject.GetComponent<TerrainTile>();
        TerrainType[] types = (TerrainType[]) Enum.GetValues(typeof(TerrainType));
        TerrainType type = types[UnityEngine.Random.Range(0, types.Length)];
        tileComponent.Initialize(type, true, sprites);
        return gameObject;
    }
}
