using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarPathFinding : MonoBehaviour
{

    public static AStarPathFinding instance;

    private static List<Vector3Int> directions = new List<Vector3Int>{Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right};

    private static float DistanceBetweenNodes = 1;

    [SerializeField]
    Tilemap tilemap;


    private void Awake()
    {
        instance = this;
    }

    public List<Node> GeneratePath(Vector3Int from, Vector3Int to)
    {
        var start = new Node(from);
        var target = new Node(to);
        var openNodes = new List<Node>();
        var closedNodes = new List<Node>();
        start.SetGScore(0);
        start.SetHScore(target);
        openNodes.Add(start);
        
        while (openNodes.Count > 0)
        {
            var currentNode = GetLowestFNode(openNodes);
            
            if(currentNode.GetPosition().Equals(target.GetPosition()))
            {
                return BuildPath(currentNode);
            }

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            
            directions.Shuffle();
            foreach (var direction in directions)
            {
                var nextPosition = currentNode.GetPosition() + direction;
                
                if(!PositionIsFree(nextPosition)) continue;

                var nextNode = FindNode(nextPosition, openNodes, closedNodes);
                if(closedNodes.Contains(nextNode)) continue;


                var nextGScore = currentNode.GetGScore() + DistanceBetweenNodes;
                if(nextGScore < nextNode.GetGScore())
                {
                    nextNode.SetCameFrom(currentNode);
                    nextNode.SetGScore(nextGScore);
                    nextNode.SetHScore(target);
                    
                    if(!openNodes.Contains(nextNode))
                    {
                        openNodes.Add(nextNode);
                    }
                }
            }
        }
        Debug.Log("Path not found");
        return null;
    }

    private Node FindNode(Vector3Int position, List<Node> openNodes, List<Node> closedNodes)
    {
        return openNodes.Find(n => n.GetPosition() == position)
            ?? closedNodes.Find(n => n.GetPosition() == position)
            ?? new Node(position);
    }


    private Node GetLowestFNode(List<Node> nodes)
    {
        int lowestF = 0;
        for (int i=1; i < nodes.Count; i++)
        {
            if(nodes[i].FScore() < nodes[lowestF].FScore())
            {
                lowestF = i;
            }
        }
        return nodes[lowestF];
    }

    private List<Node> BuildPath(Node currentNode)
    {
        var path = new List<Node>{currentNode};
        while (currentNode.GetCameFrom() != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.GetCameFrom();
        }

        path.Reverse();

        return path;
    }

     

    private bool PositionIsFree(Vector3 nextPosition)
    {
        return tilemap == null || !tilemap.HasTile(Vector3Int.FloorToInt(nextPosition));
    }


}


static class Extensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1) 
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}