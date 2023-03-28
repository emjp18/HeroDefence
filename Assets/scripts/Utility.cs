using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class Utility
{
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
}
