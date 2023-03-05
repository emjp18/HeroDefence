using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;



public class AIGrid : MonoBehaviour
{
    [SerializeField] Tilemap tilemap; //The tilemap where the collision geometry exists in.
                                      //The size of the grid cells need to be bigger
                                      //than the tiles and the tiles with collision all need to be on the same map
    
   
    A_STAR_NODE[,] customGrid;
   
    public Tilemap GetTileMap()
    {
        return tilemap;
    }
    public A_STAR_NODE[,] GetCustomGrid()
    {
        return customGrid;
    }
   
    public void Awake() //Before start in case start elsewhere calls for the Grid
    {

        GenerateGrid();

    }
    
    public void RegenerateGrid() //Can be done before every night phase in case new obstacles have been placed
    {
        Vector3Int origin = tilemap.origin;
        Vector3Int cellCount = tilemap.size;
        
        int shiftX = -origin.x; //The custom grid starts from 0 but the tilemaps dont
        int shiftY = -origin.y;

        for (int x = origin.x; x < cellCount.x + origin.x; x++)
        {
            for (int y = origin.y; y < cellCount.y + origin.y; y++)
            {


                Vector3 wp = tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0));
               
                Vector2 pos = new Vector2(wp.x, wp.y);
                customGrid[x + shiftX, y + shiftY].pos = pos;
                customGrid[x + shiftX, y + shiftY].obstacle = tilemap.HasTile(new Vector3Int(x, y, 0));
                customGrid[x + shiftX, y + shiftY].previous = new A_STAR_NODE[1];
                customGrid[x + shiftX, y + shiftY].previous[0] = new A_STAR_NODE();
                customGrid[x + shiftX, y + shiftY].previous[0].isNull = true;
                customGrid[x + shiftX, y + shiftY].index = new Vector2Int(x + shiftX, y + shiftY);
                customGrid[x + shiftX, y + shiftY].neighbours.Clear();
                customGrid[x + shiftX, y + shiftY].isNull = false;
            }
        }
      
        for (int x = origin.x; x < cellCount.x + origin.x; x++)
        {
            for (int y = origin.y; y < cellCount.y + origin.y; y++)
            {

                AddNeighbors(customGrid[x + shiftX, y + shiftY]);
            }
        }
    }
    public void GenerateGrid()
    {
        Vector3Int origin = tilemap.origin;
        int shiftX = -origin.x;
        int shiftY = -origin.y;

        Vector3Int cellCount = tilemap.size;
        customGrid = new A_STAR_NODE[cellCount.x, cellCount.y];

        
        for (int x = origin.x; x < cellCount.x + origin.x; x++)
        {
            for (int y = origin.y; y < cellCount.y + origin.y; y++)
            {

                customGrid[x + shiftX, y + shiftY] = new A_STAR_NODE();
                Vector3 wp = tilemap.GetCellCenterWorld(new Vector3Int(x, y, 0));
               
                Vector2 pos = new Vector2(wp.x, wp.y);
                customGrid[x + shiftX, y + shiftY].pos = pos;

                customGrid[x + shiftX, y + shiftY].obstacle = tilemap.HasTile(new Vector3Int(x, y, 0)); //Assumes all tiles on this map have collision
                customGrid[x + shiftX, y + shiftY].previous = new A_STAR_NODE[1];
                customGrid[x + shiftX, y + shiftY].previous[0] = new A_STAR_NODE();
                customGrid[x + shiftX, y + shiftY].neighbours = new List<A_STAR_NODE>();
                customGrid[x + shiftX, y + shiftY].previous[0].isNull = true;
                customGrid[x + shiftX, y + shiftY].index = new Vector2Int(x + shiftX, y + shiftY);
                customGrid[x + shiftX, y + shiftY].isNull = false;

            }
        }
        
        for (int x = origin.x; x < cellCount.x + origin.x; x++)
        {
            for (int y = origin.y; y < cellCount.y + origin.y; y++)
            {

                AddNeighbors(customGrid[x + shiftX, y + shiftY]);
            }
        }
    }
    
    void AddNeighbors(A_STAR_NODE node)
    {


        Vector3Int cellCount = tilemap.size;
        int cols = cellCount.y;
        int rows = cellCount.x;
        if (node.index.x < (rows - 1))
            node.neighbours.Add(customGrid[node.index.x + 1, node.index.y]);
        if (node.index.x > 0)
            node.neighbours.Add(customGrid[node.index.x - 1, node.index.y]);
        if (node.index.y < (cols - 1))
            node.neighbours.Add(customGrid[node.index.x, node.index.y + 1]);
        if (node.index.y > 0)
            node.neighbours.Add(customGrid[node.index.x, node.index.y - 1]);

        //If there is a problem with enemies walking diagonally just comment out below

        if (node.index.x < (rows - 1) && node.index.y < (cols - 1))
            node.neighbours.Add(customGrid[node.index.x + 1, node.index.y + 1]);
        if (node.index.x > 0 && node.index.y > 0)
            node.neighbours.Add(customGrid[node.index.x - 1, node.index.y - 1]);
        if (node.index.y < (cols - 1) && node.index.x > 0)
            node.neighbours.Add(customGrid[node.index.x - 1, node.index.y + 1]);
        if (node.index.y > 0 && node.index.x < (rows - 1))
            node.neighbours.Add(customGrid[node.index.x + 1, node.index.y - 1]);

    }
   
}
