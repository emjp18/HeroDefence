using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct A_STAR_NODE
{
    public bool isNull;
    public A_STAR_NODE[] previous;
    public Vector2 pos;
    public float g;
    public float h;
    public float f;
    public bool obstacle;
    public bool openSet;
    public bool closedSet;
    //public bool correctPath;
    public List<A_STAR_NODE> neighbours;
    public Vector2Int index;

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
