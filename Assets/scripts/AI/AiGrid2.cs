using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Unity.VisualScripting.Metadata;

public struct QUAD_NODE
{
    public QUAD_NODE[] children;
    public Rectangle bounds;
    public bool leaf;
    public List<Vector2Int> gridIndices;
}
public class AiGrid2 : MonoBehaviour
{
    float z = 100;
    //[SerializeField] int collisionLayerInteger;
    //LayerMask collisionLayer = new LayerMask();
    Collider2D[] colliderResult = new Collider2D[2];
    ContactFilter2D contactFilter = new ContactFilter2D();
    //Needs to be uniform for the sake of the quad tree
    [SerializeField] int rows = 10;
    int columns;
    [SerializeField] float cellSize = 5.0f;
    QUAD_NODE root;
    A_STAR_NODE[,] customGrid;
    
    [SerializeField] Transform gridCenterTransform; //Use a transform to mark where the grid center should be in worldspace
                                                     // Use Integegrs
    Vector2 gridCenter; 
    BoxCollider2D colliderBox; //This is the only alternative I could think of to find collisio where tiles overlap
                  //cells. In Unity it can only find a sprite with collision per tile, so if it overlaps
                  // Unity wont find it from the tile map or the grid. So Instead I make a new grid that
                  //checks for collision in each custom cell assuming the sprites are there before the Awake()
                  //function, and that the physics engine generates a callback for collision
    public QUAD_NODE Getroot() { return root; }
    public A_STAR_NODE[,] GetCustomGrid()
    {
        return customGrid;
    }
    public void VisualizeAINodes()
    {
        foreach (A_STAR_NODE node in customGrid)
        {
            DrawDebugLine(node);
        }
    }
    void DrawDebugBounds(Bounds b)
    {
        UnityEngine.Color c = UnityEngine.Color.magenta;

        float leftX = b.center.x - b.extents.x;
        float topY = b.center.y + b.extents.y;
        float rightX = b.center.x + b.extents.x;
        float downY = b.center.y - b.extents.y;
        //upper left to upper right
        Debug.DrawLine(new Vector3(leftX, topY, z), new Vector3(rightX,
            topY, z), c, float.PositiveInfinity);
        //upper left to lower Left
        Debug.DrawLine(new Vector3(leftX, topY, z), new Vector3(leftX,
            downY, z), c, float.PositiveInfinity);
        //upper right to lower right
        Debug.DrawLine(new Vector3(rightX, topY, z), new Vector3(rightX,
            downY, z), c, float.PositiveInfinity);
        //lower left to lower right
        Debug.DrawLine(new Vector3(leftX, downY, z), new Vector3(rightX,
            downY, z), c, float.PositiveInfinity);
    }
    void DrawDebugLine(A_STAR_NODE node)
    {
        UnityEngine.Color c = UnityEngine.Color.blue;
        if (node.obstacle)
            c = UnityEngine.Color.red;
        float leftX = node.pos.x - (cellSize * 0.5f);
        float topY = node.pos.y + (cellSize * 0.5f);
        float rightX = node.pos.x + (cellSize * 0.5f);
        float downY = node.pos.y - (cellSize * 0.5f);
        //upper left to upper right
        Debug.DrawLine(new Vector3(leftX, topY, z), new Vector3(rightX,
            topY, z), c, float.PositiveInfinity);
        //upper left to lower Left
        Debug.DrawLine(new Vector3(leftX, topY, z), new Vector3(leftX,
            downY, z), c, float.PositiveInfinity);
        //upper right to lower right
        Debug.DrawLine(new Vector3(rightX, topY, z), new Vector3(rightX,
            downY, z), c, float.PositiveInfinity);
        //lower left to lower right
        Debug.DrawLine(new Vector3(leftX, downY, z), new Vector3(rightX,
            downY, z), c, float.PositiveInfinity);
    }
    public Vector2 GetCenter() { return gridCenter; }
    //some lines are drawn on top of each other.
    void DrawDebugLine(QUAD_NODE node)
    {
        //upper left to upper right
        Debug.DrawLine(new Vector3(node.bounds.X, node.bounds.Y, z), new Vector3(node.bounds.X + node.bounds.Width,
            node.bounds.Y, z), UnityEngine.Color.green, float.PositiveInfinity);
        //upper left to lower Left
        Debug.DrawLine(new Vector3(node.bounds.X, node.bounds.Y, z), new Vector3(node.bounds.X,
            node.bounds.Y - node.bounds.Height, z), UnityEngine.Color.green, float.PositiveInfinity);
        //upper right to lower right
        Debug.DrawLine(new Vector3(node.bounds.X + node.bounds.Width, node.bounds.Y, z), new Vector3(node.bounds.X + node.bounds.Width,
            node.bounds.Y - node.bounds.Height, z), UnityEngine.Color.green, float.PositiveInfinity);
        //lower left to lower right
        Debug.DrawLine(new Vector3(node.bounds.X, node.bounds.Y - node.bounds.Height, z), new Vector3(node.bounds.X + node.bounds.Width,
            node.bounds.Y - node.bounds.Height, z), UnityEngine.Color.green, float.PositiveInfinity);
    }
    public void VisualizeQuadNodes()
    {
        DrawDebugLine(root);
        GetChildNodes(GetChildNodes(root));
        

    }
    QUAD_NODE[] GetChildNodes(QUAD_NODE parent)
    {
        return parent.children;
    }
    void GetChildNodes(QUAD_NODE[] parents)
    {
        for(int i=0; i<4; i++)
        {
            DrawDebugLine(parents[i]);
        }
        if (!parents[0].leaf)
        {
            GetChildNodes(GetChildNodes(parents[0]));
            GetChildNodes(GetChildNodes(parents[1]));
            GetChildNodes(GetChildNodes(parents[2]));
            GetChildNodes(GetChildNodes(parents[3]));
        }
       
    }
    public void Awake() //Before start in case start elsewhere calls for the Grid
    {
        //collisionLayer.value = collisionLayerInteger; //All of the colliders that are on the layer with this index will count in the grid.
        columns = rows;
        gridCenter = gridCenterTransform.position;
        //contactFilter.SetLayerMask(collisionLayer);
        contactFilter.NoFilter();
        colliderBox = GetComponent<BoxCollider2D>();
        colliderBox.size = new Vector2(cellSize, cellSize);
        GenerateGrid();
        int w = rows;
        while(w % 4 != 0)
        {
            w += 1;
        }
        
        root = new QUAD_NODE();
        //x and y is upper left corner
        Vector2 c = gridCenter;
        c.x += cellSize * 0.5f;
        c.y -= cellSize * 0.5f;
        root.bounds = new Rectangle((int)c.x- (int)(rows * cellSize*0.5f),
            (int)c.y + (int)(columns * cellSize * 0.5f), w * (int)Mathf.Ceil(cellSize), w * (int)Mathf.Ceil(cellSize));
        CreateQuadTree(ref root);

    }

    public void RegenerateGrid() //Can be done before every night phase in case new obstacles have been placed
    {
        int center = rows / 2;
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                Vector2 worldPos;
                
                if (rows < center)
                {
                    worldPos.x = gridCenter.x - cellSize * (center - x);
                }
                else if (rows == center)
                {
                    worldPos.x = gridCenter.x;
                }
                else
                {
                    worldPos.x = gridCenter.x + cellSize * (x - center);
                }
                if (columns < center)
                {
                    worldPos.y = gridCenter.y - cellSize * (center - y);
                }
                else if (columns == center)
                {
                    worldPos.y = gridCenter.y;
                }
                else
                {
                    worldPos.y = gridCenter.y + cellSize * (y - center);
                }
                worldPos.x += cellSize * 0.5f; //making the worldpos be in the center of the node assuming world center is up and left
                worldPos.y -= cellSize * 0.5f;

                colliderBox.transform.position = worldPos;


                //Only regenerate collision
                customGrid[x, y].obstacle = colliderBox.OverlapCollider(contactFilter, colliderResult) > 0;
         
                
            
               

            }
        }

       
    }
    public void GenerateGrid()
    {

        
       
        customGrid = new A_STAR_NODE[rows, columns];
        //Left and Down is Decreasing in Unity
        //0,0 is left and down in this grid
        int center = rows / 2;
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                Vector2 worldPos;
                customGrid[x,y] = new A_STAR_NODE();
                if(rows<center)
                {
                    worldPos.x = gridCenter.x - cellSize * (center - x);
                }
                else if(rows == center)
                {
                    worldPos.x = gridCenter.x;
                }
                else
                {
                    worldPos.x = gridCenter.x + cellSize * (x - center);
                }
                if (columns < center)
                {
                    worldPos.y = gridCenter.y - cellSize * (center - y);
                }
                else if (columns == center)
                {
                    worldPos.y = gridCenter.y;
                }
                else
                {
                    worldPos.y = gridCenter.y + cellSize * (y - center);
                }
                worldPos.x += cellSize * 0.5f; //making the worldpos be in the center of the node
                worldPos.y -= cellSize * 0.5f;

               
                colliderBox.offset = worldPos;
              
               
                 Vector2 pos = new Vector2(worldPos.x, worldPos.y);
                customGrid[x, y].pos = pos;
                bool obstacle = false;
                if(colliderBox.OverlapCollider(contactFilter, colliderResult) > 0)
                {
                   
                    foreach(Collider2D collider in colliderResult)
                    {
                        if(collider != null)
                        {
                            if (collider.gameObject.tag == "Character")
                            {
                                obstacle = false;
                            }
                            else
                            {
                                obstacle = true;
                                break;
                            }
                        }
                       
                    }
                   
                    
                    
                }
                customGrid[x, y].obstacle = obstacle;

                //if (customGrid[x, y].obstacle)
                //    DrawDebugBounds(colliderBox.bounds);
                
                customGrid[x, y].previous = new A_STAR_NODE[1];
                customGrid[x, y].previous[0] = new A_STAR_NODE();
                customGrid[x, y].neighbours = new List<A_STAR_NODE>();
                customGrid[x, y].previous[0].isNull = true;
                customGrid[x, y].index = new Vector2Int(x,y);
                customGrid[x, y].isNull = false;

            }
        }

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0;y< columns; y++)
            {

                AddNeighbors(customGrid[x, y]);
            }
        }
    }

    void AddNeighbors(A_STAR_NODE node)
    {


        
        if (node.index.x < (rows - 1))
            node.neighbours.Add(customGrid[node.index.x + 1, node.index.y]);
        if (node.index.x > 0)
            node.neighbours.Add(customGrid[node.index.x - 1, node.index.y]);
        if (node.index.y < (columns - 1))
            node.neighbours.Add(customGrid[node.index.x, node.index.y + 1]);
        if (node.index.y > 0)
            node.neighbours.Add(customGrid[node.index.x, node.index.y - 1]);

        //If there is a problem with enemies walking diagonally just comment out below

        if (node.index.x < (rows - 1) && node.index.y < (columns - 1))
            node.neighbours.Add(customGrid[node.index.x + 1, node.index.y + 1]);
        if (node.index.x > 0 && node.index.y > 0)
            node.neighbours.Add(customGrid[node.index.x - 1, node.index.y - 1]);
        if (node.index.y < (columns - 1) && node.index.x > 0)
            node.neighbours.Add(customGrid[node.index.x - 1, node.index.y + 1]);
        if (node.index.y > 0 && node.index.x < (rows - 1))
            node.neighbours.Add(customGrid[node.index.x + 1, node.index.y - 1]);

    }
    
    //The quad tree is used to find AI nodes quicker based on location.
    //each level of depth 4 pow depth
    //4, 4x4, 4x4x4
    //For simplicity sake each qud tree node should contain only one grid index So the smallest quad node size
    //is smaller or equal to the AI cell Size
    //Each recursion divides the bounds and creates 4 new equally large nodes, when the nodes are small enough the recursion stops
    void CreateQuadTree(ref QUAD_NODE node)
    {
        
        node.leaf = false;
        node.gridIndices = new List<Vector2Int>(); //this list should be empty for every node that isn't a leaf node
        if (node.bounds.Width > cellSize) // the rectangles uses integer division
        {
            node.children = new QUAD_NODE[4];
            node.children[0] = new QUAD_NODE();

            node.children[1] = new QUAD_NODE();

            node.children[2] = new QUAD_NODE();

            node.children[3] = new QUAD_NODE();


            node.children[0].bounds = new Rectangle(node.bounds.X, node.bounds.Y,
                    node.bounds.Width / 2,
                    node.bounds.Height / 2);
            node.children[1].bounds = new Rectangle(node.bounds.X + node.bounds.Width / 2,
                node.bounds.Y,
                    node.bounds.Width / 2,
                    node.bounds.Height / 2);
            node.children[2].bounds = new Rectangle(node.bounds.X + node.bounds.Width / 2,
                node.bounds.Y - node.bounds.Height / 2,
                    node.bounds.Width / 2,
                    node.bounds.Height / 2);
            node.children[3].bounds = new Rectangle(node.bounds.X, node.bounds.Y -
               node.bounds.Height / 2,
                    node.bounds.Width / 2,
                    node.bounds.Height / 2);
            
            CreateQuadTree(ref node.children[0]);
            CreateQuadTree(ref node.children[1]);
            CreateQuadTree(ref node.children[2]);
            CreateQuadTree(ref node.children[3]);
            return;
        }
       
        node.leaf = true;




        for (int x = 0; x < customGrid.GetLength(0); x++)
        {

            for (int y = 0; y < customGrid.GetLength(1); y++)
            {


                if (PointAABBIntersectionTest(node.bounds, customGrid[x, y].pos))
                {
                    node.gridIndices.Add(customGrid[x, y].index); //Since the nodes can be smaller than the AI nodes multiple nodes
                    
                }
            }

        }

        return;

    }
    //Unity Rectangle Contains function dosen't work
    bool PointAABBIntersectionTest(Rectangle bounds, Vector2 p)
    {
        return p.x >= bounds.X
            && p.x <= bounds.X + bounds.Width
            && p.y >= bounds.Y - bounds.Height
            && p.y <= bounds.Y;
    }
}

