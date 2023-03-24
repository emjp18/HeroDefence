using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static bool PointAABBIntersectionTest(RectangleFloat bounds, Vector2 p)
    {
        return p.x >= bounds.X
            && p.x <= bounds.X + bounds.Width
            && p.y >= bounds.Y - bounds.Height
            && p.y <= bounds.Y;
    }
}
