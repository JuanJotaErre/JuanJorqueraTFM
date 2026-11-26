using UnityEngine;

public class Node
{
    private Node cameFrom;
    private Vector3Int position;
    private float gScore;
    private float hScore;

    public Node(Vector3Int position)
    {
        this.position = position;
        this.gScore = float.MaxValue;
    }

    public float GetGScore()
    {
        return gScore;
    }

    public void SetGScore(float score)
    {
        gScore = score;
    }

    public float GetHScore()
    {
        return hScore;
    }

    public void SetHScore(Node endNode)
    {
        hScore = Mathf.Abs(position.x - endNode.position.x)
               + Mathf.Abs(position.y - endNode.position.y);
    }

    public float FScore()
    {
        return gScore + hScore;
    }

    public Vector3Int GetPosition()
    {
        return position;
    }

    public Node GetCameFrom()
    {
        return cameFrom;
    }

    public void SetCameFrom(Node cameFrom)
    {
        this.cameFrom = cameFrom;
    }

}
