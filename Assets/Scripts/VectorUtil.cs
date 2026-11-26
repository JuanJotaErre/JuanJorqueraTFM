

using UnityEngine;

public static class VectorUtil 
{




    public static Vector3Int GetRandomDirection()
    {
        return Random.Range(0, 4) switch
        {
            0 =>  Vector3Int.up,
            1 =>  Vector3Int.right,
            2 =>  Vector3Int.down,
            3 =>  Vector3Int.left,
            _=> new Vector3Int(0,0,0)
        };
    }

}
