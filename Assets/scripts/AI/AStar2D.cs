using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

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
    public bool correctPath;
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
public class AStar2D
{
    Vector2Int startIndex;
    Vector2Int endIndex;
    A_STAR_NODEComparer comp = new A_STAR_NODEComparer();
    bool pathFound = false;
    A_STAR_NODE start;
    A_STAR_NODE end;
    List<A_STAR_NODE> open = new List<A_STAR_NODE>();
    List<A_STAR_NODE> closed = new List<A_STAR_NODE>();
    List<A_STAR_NODE> path = new List<A_STAR_NODE>();
    bool resetPath = false;
    bool isSearching = false;
    bool isFinding = false;
    A_STAR_NODE[,] customGrid;
    QUAD_NODE rootQuadNode;
  

    public AStar2D(AiGrid2 grid)
    {

        rootQuadNode = grid.Getroot();
        customGrid = grid.GetCustomGrid();
    }
    public bool Finding
    {
        get { return isFinding; }
        set { isFinding = value; }
    }
    public bool Reset
    {
        get { return resetPath; }
        set { resetPath = value; }
    }
    public bool Searching
    {
        get { return isSearching; }
        set { isSearching = value; }
    }

    public void ResetPath()
    {
        resetPath= true;
        isSearching= false;
        isFinding= false;
        startIndex = Vector2Int.zero;
        endIndex = Vector2Int.zero;
        pathFound = false;
        path.Clear();
        
    }
    public bool GetPathFound()
    {
        return pathFound;
    }
    public List<A_STAR_NODE> GetPath()
    {
        return path;
    }
   
    int GetDistance(A_STAR_NODE nodeA, A_STAR_NODE nodeB)
    {
        return (int)MathF.Abs(nodeA.pos.x - nodeB.pos.x) + (int)MathF.Abs(nodeA.pos.y - nodeB.pos.y);
    }
    void FindPath(Vector2Int startIndex, Vector2Int endIndex)
    {
        if(resetPath)
        {
            return;
        }
        isFinding = true;

        start = customGrid[startIndex.x, startIndex.y];
        end = customGrid[endIndex.x, endIndex.y];
        open.Clear();
        closed.Clear();
        open.Add(start);


        customGrid[start.index.x, start.index.y].openSet = true;
        while (open.Count > 0 && !pathFound)
        {
            if (resetPath)
            {
                return;
            }
            if (open.Count > 1)
            {
                open.Sort(comp);
            }

            A_STAR_NODE current = open[0];
            if (current.pos.x == end.pos.x && current.pos.y == end.pos.y)
            {
                pathFound = true;

                A_STAR_NODE temp = current;
                customGrid[temp.index.x, temp.index.y].correctPath = true;
                path.Add(customGrid[temp.index.x, temp.index.y]);

                while (!temp.previous[0].isNull)
                {
                    temp.previous[0].isNull = true;
                    temp = temp.previous[0];
                    customGrid[temp.index.x, temp.index.y].correctPath = true;
                    path.Add(customGrid[temp.index.x, temp.index.y]);

                }
                path.Reverse();
                
            }
            else
            {
                open.Remove(open.First());

                customGrid[current.index.x, current.index.y].openSet = false;
                closed.Add(current);
                customGrid[current.index.x, current.index.y].closedSet = true;

                for (int i = 0; i < current.neighbours.Count; i++)
                {
                    if ((!closed.Any() || !closed.Contains(current.neighbours[i]))
                        && !current.neighbours[i].obstacle)        //Closes the entire Cell if there is a tile inside of it.
                    {
                        float tempG = current.g + 1;

                        bool newPath = false;
                        if (open.Contains(current.neighbours[i]))
                        {
                            if (tempG < current.neighbours[i].g)
                            {
                                A_STAR_NODE a_STAR_NODE = current.neighbours[i];
                                a_STAR_NODE.g = tempG;
                                current.neighbours[i] = a_STAR_NODE;
                                newPath = true;
                            }
                        }
                        else
                        {
                            A_STAR_NODE a_STAR_NODE = current.neighbours[i];
                            a_STAR_NODE.g = tempG;
                            current.neighbours[i] = a_STAR_NODE;
                            open.Add(current.neighbours[i]);
                            customGrid[current.neighbours[i].index.x,
                                current.neighbours[i].index.y].openSet = true;
                            newPath = true;
                        }
                        if (newPath)
                        {
                            A_STAR_NODE a_STAR_NODE = current.neighbours[i];
                            a_STAR_NODE.h = GetDistance(current.neighbours[i], end);
                            a_STAR_NODE.f = current.neighbours[i].g + current.neighbours[i].h;
                            a_STAR_NODE.previous[0] = current;
                            current.neighbours[i] = a_STAR_NODE;

                        }
                    }
                }
            }
        }
        
    }
    public void AStarSearch(Vector2 currentPos, Vector2 goalPos)
    {
        isSearching = true;
        resetPath= false;
        GetAIGridIndex(currentPos, rootQuadNode);
        GetAIGridIndex(goalPos, rootQuadNode,false);




    }
    //Unity Rectangle Contains function dosen't work
    bool PointAABBIntersectionTest(RectangleFloat bounds, Vector2 p)
    {
        return p.x >= bounds.X
            && p.x <= bounds.X + bounds.Width
            && p.y >= bounds.Y - bounds.Height
            && p.y <= bounds.Y;
    }
    public bool IsWithinGridBounds(Vector2 pos)
    {
        return PointAABBIntersectionTest(rootQuadNode.bounds, pos);
    }
    void GetAIGridIndex(Vector2 pos,QUAD_NODE node, bool isCurrentPos = true)
    {
        //abort if restart
        if (resetPath)
            return;



        if (PointAABBIntersectionTest(node.bounds, pos))
        {
            if(node.leaf)
            {
               
                if (isCurrentPos)
                {
                    
                    startIndex = node.gridIndices[0];
                    return;
                }
                else if (!isCurrentPos)
                {
                   
                    endIndex = node.gridIndices[0];
                    if (startIndex != Vector2Int.zero && endIndex != Vector2Int.zero)
                    {
                        
                        FindPath(startIndex, endIndex);
                        return;
                    }
                    return;
                }
                
            }
            else
            {
                for(int i=0; i<4; i++)
                {
                    GetAIGridIndex(pos, node.children[i], isCurrentPos);
                }
                return;
            }
        }
        return;






    }
}