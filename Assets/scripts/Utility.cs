using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public static class Utility
{
    public static Dictionary<RectangleFloat, List<Vector2>> staticObstacles
        = new Dictionary<RectangleFloat, List<Vector2>>();
    
    public static bool PointAABBIntersectionTest(RectangleFloat bounds, Vector2 p)
    {
        return p.x >= bounds.X
            && p.x <= bounds.X + bounds.Width
            && p.y >= bounds.Y - bounds.Height
            && p.y <= bounds.Y;
    }
    public static bool GetAIGridIndex(Vector2 pos, QUAD_NODE node,  ref Vector2Int index)
    {
       
        if (PointAABBIntersectionTest(node.bounds, pos))
        {
            if (node.leaf)
            {
              
                index = node.gridIndices[0];
                return true;

            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    GetAIGridIndex(pos, node.children[i], ref index);
                }
                return false;
            }
        }
        return false;






    }

    public static void FindNearbyStaticObstacles(Vector2 pos, QUAD_NODE node, AiGrid grid,out bool obstacleFound)
    {
        if (PointAABBIntersectionTest(node.bounds, pos))
        {
            if (node.leaf)
            {
                obstacleFound = grid.GetCustomGrid()[node.gridIndices[0].x,
                    node.gridIndices[0].y].obstacle;
                return;

            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    FindNearbyStaticObstacles(pos, node.children[i], grid, out obstacleFound);
                }
                obstacleFound = false;
                return;
            }
        }
        obstacleFound = false;
        return;
    }
    public static void FindObstaclesFromNode(Vector2 pos, QUAD_NODE node, ref int currentDepth, ref List<Vector2>
        nearbyCollisions,int maxDepth = 1)
    {
        if (PointAABBIntersectionTest(node.bounds, pos))
        {
            if (currentDepth==maxDepth)
            {
                Debug.Log(node.bounds);
                nearbyCollisions = staticObstacles[node.bounds];
                return;

            }
            else
            {
                currentDepth += 1;
                for (int i = 0; i < 4; i++)
                {
                    FindObstaclesFromNode(pos, node.children[i], ref currentDepth, ref nearbyCollisions,maxDepth);
                }
             
                return;
            }
        }
       
        return;
    }
}
