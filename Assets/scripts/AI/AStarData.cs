using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct A_STAR_NODE
{
   
    public Vector2 pos;
    public float g;
    public float h;
    public float f;
    public bool obstacle;
    public List<Vector2Int> neighbours;
    public Vector2Int index;
    public RectangleFloat bounds;
    public Vector2Int prevIndex;
    public A_STAR_NODE(A_STAR_NODE copy)
    {
        pos = copy.pos;
        g = copy.g;
        h = copy.h;
        f = copy.f;
        obstacle = copy.obstacle;
        neighbours = copy.neighbours;
        index = copy.index;
        bounds = copy.bounds;
        prevIndex = copy.prevIndex;
    }

    public override bool Equals(object obj)
    {
        var b = (A_STAR_NODE)obj;
        return pos == b.pos;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 1430287;

            hash = hash * 7302013 ^ pos.x.GetHashCode();
            hash = hash * 7302013 ^ pos.y.GetHashCode();
            return hash;
        }
    }
}

public class A_STAR_NODEComparer : IComparer<A_STAR_NODE>
{
    public int Compare(A_STAR_NODE x, A_STAR_NODE y)
    {

        return x.f.CompareTo(y.f);
    }
}
