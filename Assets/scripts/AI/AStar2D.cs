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

    public QUAD_NODE Quadtree
    {
        get => rootQuadNode;
    }
  
    public void UpdateGrid(AiGrid grid)
    {
        rootQuadNode = grid.Getroot();
        customGrid = grid.GetCustomGrid();
    }
    public AStar2D(AiGrid grid)
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
        pathFound = false;
        startIndex = Vector2Int.zero;
        endIndex = Vector2Int.zero;

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
    public void FindPath(Vector2Int startIndex, Vector2Int endIndex, int maxiumNodes)
    {
        if(resetPath)
        {
            isFinding = false;
            return;
        }
        var copy = customGrid;
      
        start = copy[startIndex.x, startIndex.y];
        end = copy[endIndex.x, endIndex.y];
        if (start.obstacle || end.obstacle)
        {
            isFinding = false;
            return;
        }
           
        open.Clear();
        closed.Clear();
        open.Add(start);
        path.Clear();
        int c = 0;
        int nr = 0;
        //customGrid[start.index.x, start.index.y].openSet = true;
        while (open.Count > 0 && !pathFound)
        {
            c++;
            
            if (resetPath||c>maxiumNodes)
            {
                
                isFinding = false;
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
            


                Vector2Int index = current.index;


                for (int i=0; i< nr; i++)
                {

                    if (index.x == int.MaxValue)
                        break;

                    path.Add(copy[index.x, index.y]);
                    index = copy[index.x, index.y].prevIndex;
                    


                }
                //path.Add(copy[startIndex.x, startIndex.y]);
                path.Reverse();
                isFinding = false;
                //
                return;
            }
            else
            {
                open.Remove(open.First());
           
                closed.Add(current);
               

                for (int i = 0; i < current.neighbours.Count; i++)
                {
                    if ((!closed.Any() || !closed.Contains(current.neighbours[i]))
                        && !current.neighbours[i].obstacle)        //Closes the entire Cell if there is a tile inside of it.
                    {
                        float tempG = current.g + 1;

                       
                        if (open.Contains(current.neighbours[i]))
                        {
                            if (tempG < current.neighbours[i].g)
                            {
                                

                                A_STAR_NODE a_STAR_NODE = current.neighbours[i];
                                a_STAR_NODE.g = tempG;
                                a_STAR_NODE.h = GetDistance(current.neighbours[i].neighbours[i], end);
                                a_STAR_NODE.f = current.neighbours[i].neighbours[i].g + current.neighbours[i].h;
                                a_STAR_NODE.bounds = current.neighbours[i].neighbours[i].bounds;
                                a_STAR_NODE.index = current.neighbours[i].index;
                                a_STAR_NODE.pos = current.neighbours[i].pos;
                                a_STAR_NODE.neighbours = current.neighbours[i].neighbours;
                                a_STAR_NODE.obstacle = current.neighbours[i].obstacle;
                                a_STAR_NODE.prevIndex = current.index;
                                current.neighbours[i] = a_STAR_NODE;
                                copy[a_STAR_NODE.index.x, a_STAR_NODE.index.y].prevIndex = a_STAR_NODE.prevIndex;
                            }
                        }
                        else
                        {
                           
                           
                            A_STAR_NODE a_STAR_NODE = current.neighbours[i];
                            a_STAR_NODE.g = tempG;
                            a_STAR_NODE.h = GetDistance(current.neighbours[i].neighbours[i], end);
                            a_STAR_NODE.f = current.neighbours[i].neighbours[i].g + current.neighbours[i].h;
                            a_STAR_NODE.bounds = current.neighbours[i].neighbours[i].bounds;
                            a_STAR_NODE.index = current.neighbours[i].index;
                            a_STAR_NODE.pos = current.neighbours[i].pos;
                            a_STAR_NODE.neighbours = current.neighbours[i].neighbours;
                            a_STAR_NODE.obstacle = current.neighbours[i].obstacle;
                            a_STAR_NODE.prevIndex = current.index;
                            current.neighbours[i] = a_STAR_NODE;
                            open.Add(current.neighbours[i]);
                            copy[a_STAR_NODE.index.x, a_STAR_NODE.index.y].prevIndex = a_STAR_NODE.prevIndex;
                            nr++;
                        }
                        
                    }
                }
            }
        }
        
        isFinding = false;
        return;
    }
    public void AStarSearch(Vector2 currentPos, Vector2 goalPos, int maximumNodes = 50)
    {
        if(!isFinding&&!isSearching)
        {
            isSearching = true; 
            resetPath = false;
            
            GetAIGridIndex(currentPos, rootQuadNode, maximumNodes, true);
            GetAIGridIndex(goalPos, rootQuadNode, maximumNodes, false);
           
        }
       
        
        




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
    void GetAIGridIndex(Vector2 pos,QUAD_NODE node, int maximumNodes, bool isCurrentPos = true)
    {
        //abort if restart
        if (resetPath)
        {
            isSearching= false;
            return;
        }
         



        if (PointAABBIntersectionTest(node.bounds, pos))
        {
            if(node.leaf)
            {
                isSearching = false;
                if (isCurrentPos)
                {
                    if (node.gridIndices.Count == 0)
                        return;
                    startIndex = node.gridIndices[0];
                    if (customGrid[startIndex.x, startIndex.y].obstacle)
                        startIndex = Vector2Int.zero;

                    return;
                }
                else if (!isCurrentPos)
                {
                    if (node.gridIndices.Count == 0)
                        return;
                    endIndex = node.gridIndices[0];
                    if (startIndex != Vector2Int.zero && endIndex != Vector2Int.zero)
                    {
                        if (customGrid[endIndex.x, endIndex.y].obstacle)
                        {
                            endIndex = Vector2Int.zero;
                            return;
                        }
                        
                        isFinding = true;
                   
                        FindPath(startIndex, endIndex, maximumNodes);
                        return;
                    }
                    return;
                }
                
            }
            else
            {
                for(int i=0; i<4; i++)
                {
                    GetAIGridIndex(pos, node.children[i], maximumNodes, isCurrentPos);
                }
                return;
            }
        }
        return;






    }
}