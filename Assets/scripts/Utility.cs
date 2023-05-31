using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEditor.PlayerSettings;
public struct RectangleFloat
{
    public float X;
    public float Y;
    public float Width;
    public float Height;
    public static bool operator ==(RectangleFloat left, RectangleFloat right)
    {
        return left.Width == right.Width && left.Height == right.Height && left.Y == right.Y && left.X == right.X;
    }
    public static bool operator !=(RectangleFloat left, RectangleFloat right)
    {
        return left.Width != right.Width || left.Height != right.Height || left.Y != right.Y || left.X != right.X;
    }
    public override bool Equals(object obj)
    {
        var convObj = (RectangleFloat)obj;
        return this.Width == convObj.Width && this.Height == convObj.Height && this.Y
            == convObj.Y && this.X == convObj.X;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 1430287;

            hash = hash * 7302013 ^ Width.GetHashCode();
            hash = hash * 7302013 ^ Height.GetHashCode();
            hash = hash * 7302013 ^ X.GetHashCode();
            hash = hash * 7302013 ^ Y.GetHashCode();
            return hash;
        }
    }
}

public struct QUAD_NODE
{
    public QUAD_NODE[] children;
    public RectangleFloat bounds;
    public bool leaf;
    public List<Vector2Int> gridIndices;
}
public enum Row_Count { ONE = 1, TWO = 2, FOUR = 4, EIGHT = 8, SIXTYFOUR = 64 };
public static class Utility
{
    static Dictionary<RectangleFloat, List<Vector2>> staticObstacles
        = new Dictionary<RectangleFloat, List<Vector2>>();
    static Dictionary<RectangleFloat, List<Vector2>> staticObstaclesLargeGrid
        = new Dictionary<RectangleFloat, List<Vector2>>();
    public static float GRID_CELL_SIZE;
    public static float GRID_CELL_SIZE_LARGE;
    static List<Vector2> temp = new List<Vector2>();
    static List<float> temp2 = new List<float>();
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
           
              
                nearbyCollisions = staticObstacles[node.bounds];
                Debug.Log(nearbyCollisions.Count);
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

    public static void UpdateStaticCollision(AiGrid grid)
    {
        staticObstacles.Clear();
        staticObstacles.Add(grid.Getroot().children[0].children[0].bounds, new List<Vector2>());
        staticObstacles.Add(grid.Getroot().children[0].children[1].bounds, new List<Vector2>());
        staticObstacles.Add(grid.Getroot().children[0].children[2].bounds, new List<Vector2>());
        staticObstacles.Add(grid.Getroot().children[0].children[3].bounds, new List<Vector2>());

        staticObstacles.Add(grid.Getroot().children[1].children[0].bounds, new List<Vector2>());
        staticObstacles.Add(grid.Getroot().children[1].children[1].bounds, new List<Vector2>());
        staticObstacles.Add(grid.Getroot().children[1].children[2].bounds, new List<Vector2>());
        staticObstacles.Add(grid.Getroot().children[1].children[3].bounds, new List<Vector2>());

        staticObstacles.Add(grid.Getroot().children[2].children[0].bounds, new List<Vector2>());
        staticObstacles.Add(grid.Getroot().children[2].children[1].bounds, new List<Vector2>());
        staticObstacles.Add(grid.Getroot().children[2].children[2].bounds, new List<Vector2>());
        staticObstacles.Add(grid.Getroot().children[2].children[3].bounds, new List<Vector2>());

        staticObstacles.Add(grid.Getroot().children[3].children[0].bounds, new List<Vector2>());
        staticObstacles.Add(grid.Getroot().children[3].children[1].bounds, new List<Vector2>());
        staticObstacles.Add(grid.Getroot().children[3].children[2].bounds, new List<Vector2>());
        staticObstacles.Add(grid.Getroot().children[3].children[3].bounds, new List<Vector2>());

        foreach (A_STAR_NODE node in grid.GetCustomGrid())
        {


            if (node.obstacle)
            {

                if (PointAABBIntersectionTest(grid.Getroot().children[0].children[0].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[0].children[0].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[0].children[1].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[0].children[1].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[0].children[2].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[0].children[2].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[0].children[3].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[0].children[3].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[1].children[0].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[1].children[0].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[1].children[1].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[1].children[1].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[1].children[2].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[1].children[2].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[1].children[3].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[1].children[3].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[2].children[0].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[2].children[0].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[2].children[1].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[2].children[1].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[2].children[2].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[2].children[2].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[2].children[3].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[2].children[3].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[3].children[0].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[3].children[0].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[3].children[1].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[3].children[1].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[3].children[2].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[3].children[2].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[3].children[3].bounds, node.pos))
                {
                    staticObstacles[grid.Getroot().children[3].children[3].bounds].Add(node.pos);

                }
            }
        }



    }
    public static void UpdateStaticCollisionLarge(AiGrid grid)
    {
        staticObstaclesLargeGrid.Clear();
        staticObstaclesLargeGrid.Add(grid.Getroot().children[0].children[0].bounds, new List<Vector2>());
        staticObstaclesLargeGrid.Add(grid.Getroot().children[0].children[1].bounds, new List<Vector2>());
        staticObstaclesLargeGrid.Add(grid.Getroot().children[0].children[2].bounds, new List<Vector2>());
        staticObstaclesLargeGrid.Add(grid.Getroot().children[0].children[3].bounds, new List<Vector2>());

        staticObstaclesLargeGrid.Add(grid.Getroot().children[1].children[0].bounds, new List<Vector2>());
        staticObstaclesLargeGrid.Add(grid.Getroot().children[1].children[1].bounds, new List<Vector2>());
        staticObstaclesLargeGrid.Add(grid.Getroot().children[1].children[2].bounds, new List<Vector2>());
        staticObstaclesLargeGrid.Add(grid.Getroot().children[1].children[3].bounds, new List<Vector2>());

        staticObstaclesLargeGrid.Add(grid.Getroot().children[2].children[0].bounds, new List<Vector2>());
        staticObstaclesLargeGrid.Add(grid.Getroot().children[2].children[1].bounds, new List<Vector2>());
        staticObstaclesLargeGrid.Add(grid.Getroot().children[2].children[2].bounds, new List<Vector2>());
        staticObstaclesLargeGrid.Add(grid.Getroot().children[2].children[3].bounds, new List<Vector2>());

        staticObstaclesLargeGrid.Add(grid.Getroot().children[3].children[0].bounds, new List<Vector2>());
        staticObstaclesLargeGrid.Add(grid.Getroot().children[3].children[1].bounds, new List<Vector2>());
        staticObstaclesLargeGrid.Add(grid.Getroot().children[3].children[2].bounds, new List<Vector2>());
        staticObstaclesLargeGrid.Add(grid.Getroot().children[3].children[3].bounds, new List<Vector2>());

        foreach (A_STAR_NODE node in grid.GetCustomGrid())
        {


            if (node.obstacle)
            {

                if (PointAABBIntersectionTest(grid.Getroot().children[0].children[0].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[0].children[0].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[0].children[1].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[0].children[1].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[0].children[2].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[0].children[2].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[0].children[3].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[0].children[3].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[1].children[0].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[1].children[0].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[1].children[1].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[1].children[1].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[1].children[2].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[1].children[2].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[1].children[3].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[1].children[3].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[2].children[0].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[2].children[0].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[2].children[1].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[2].children[1].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[2].children[2].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[2].children[2].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[2].children[3].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[2].children[3].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[3].children[0].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[3].children[0].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[3].children[1].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[3].children[1].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[3].children[2].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[3].children[2].bounds].Add(node.pos);

                }
                else if (PointAABBIntersectionTest(grid.Getroot().children[3].children[3].bounds, node.pos))
                {
                    staticObstaclesLargeGrid[grid.Getroot().children[3].children[3].bounds].Add(node.pos);

                }
            }
        }



    }

    public static Vector2 Avoid(Vector2 pos, QUAD_NODE root, Vector2 direction,
        float cellsDetection = 1, float cellsPower = 2)//Meant to make target points further away not for the character pos
    {
        int boxi = -1;
        int boxj =-1;
        for (int i = 0; i < 4; i++)
        {
            bool exit = false;
            for (int j = 0; j < 4; j++)
            {
                if (PointAABBIntersectionTest(root.children[i].children[j].bounds, pos))
                {
                    boxi = i;
                    boxj = j;
                    exit = true;
                    break;
                }
            }
            if (exit)
                break;
        }
        if (boxi == -1)
            return Vector2.zero;


        Vector2 avoidanceMove = Vector2.zero;

        temp.Clear();
        float sum = 0;
        int c = 0;
        foreach (Vector2 obstacle in staticObstacles[root.children[boxi].children[boxj].bounds])
        {
            
            
            float cos = Vector2.Dot((pos - obstacle).normalized, direction.normalized);
            if ((pos - obstacle).magnitude < GRID_CELL_SIZE* cellsDetection)
            {

                c++;

                temp.Add((pos - obstacle));

                break;
            }
        }

       
        for (int i = 0; i < c; i++)
        {
        
            avoidanceMove += (temp[i].normalized * GRID_CELL_SIZE* cellsPower);


        }
     
        return avoidanceMove;
    }
}
