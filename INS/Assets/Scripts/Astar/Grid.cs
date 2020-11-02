using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public Transform StartPosition;//This is where the program will start the pathfinding from.
    public LayerMask WallMask;//This is the mask that the program will look for when trying to find obstructions to the path.
    public Vector2 gridWorldSize;//A vector2 to store the width and height of the graph in world units.
    public float nodeRadius;//This stores how big each square on the graph will be
    public float Distance;//The distance that the squares will spawn from eachother.

    Node[,] grid;//The array of nodes that the A Star algorithm uses.
    public List<Node> FinalPath;//The completed path that the red line will be drawn along


    float nodeDiameter;//Twice the amount of the radius (Set in the start function)
    int gridSizeX, gridSizeY;//Size of the Grid in Array units.


    //public GameObject gridPathPrefab;

    /*
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    */

    private void Start()//Ran once the program starts
    {
        nodeDiameter = nodeRadius * 2;//Double the radius to get diameter
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);//Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        CreateGrid();//Draw the grid
        GlobalValues.newPath = true;
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];//Declare the array of nodes.
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;//Get the real world position of the bottom left of the grid.
        for (int x = 0; x < gridSizeX; x++)//Loop through the array of nodes.
        {
            for (int y = 0; y < gridSizeY; y++)//Loop through the array of nodes
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);//Get the world co ordinates of the bottom left of the graph
                bool Wall = true;//Make the node a wall

                //If the node is not being obstructed
                //Quick collision check against the current node and anything in the world at its position. If it is colliding with an object with a WallMask,
                //The if statement will return false.
                if (Physics.CheckSphere(worldPoint, nodeRadius, WallMask))
                {
                    Wall = false;//Object is not a wall
                }

                grid[x, y] = new Node(Wall, worldPoint, x, y);//Create a new node in the array.
            }
        }
    }

    
    //Function that gets the neighboring nodes of the given node.
    public List<Node> GetNeighboringNodes(Node a_Node)
    {
        List<Node> NeighboringNodes = new List<Node>();//Make a new list of all available neighbors.
        int xCheck;//Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int yCheck;//Variable to check if the YPosition is within range of the node array to avoid out of range errors.

        //Check the right side of the current node.
        xCheck = a_Node.gridX + 1;
        yCheck = a_Node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)//If the XPosition is in range of the array
        {
            if (yCheck >= 0 && yCheck < gridSizeY)//If the YPosition is in range of the array
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);//Add the grid to the available neighbors list
            }
        }
        //Check the Left side of the current node.
        xCheck = a_Node.gridX - 1;
        yCheck = a_Node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)//If the XPosition is in range of the array
        {
            if (yCheck >= 0 && yCheck < gridSizeY)//If the YPosition is in range of the array
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);//Add the grid to the available neighbors list
            }
        }
        //Check the Top side of the current node.
        xCheck = a_Node.gridX;
        yCheck = a_Node.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)//If the XPosition is in range of the array
        {
            if (yCheck >= 0 && yCheck < gridSizeY)//If the YPosition is in range of the array
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);//Add the grid to the available neighbors list
            }
        }
        //Check the Bottom side of the current node.
        xCheck = a_Node.gridX;
        yCheck = a_Node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)//If the XPosition is in range of the array
        {
            if (yCheck >= 0 && yCheck < gridSizeY)//If the YPosition is in range of the array
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);//Add the grid to the available neighbors list
            }
        }


        //Check sideways
        //Check the Bottom Left side of the current node.
        xCheck = a_Node.gridX - 1;
        yCheck = a_Node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)//If the XPosition is in range of the array
        {
            if (yCheck >= 0 && yCheck < gridSizeY)//If the YPosition is in range of the array
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);//Add the grid to the available neighbors list
            }
        }
        //Check the Bottom Right side of the current node.
        xCheck = a_Node.gridX + 1;
        yCheck = a_Node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)//If the XPosition is in range of the array
        {
            if (yCheck >= 0 && yCheck < gridSizeY)//If the YPosition is in range of the array
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);//Add the grid to the available neighbors list
            }
        }
        //Check the Top Right side of the current node.
        xCheck = a_Node.gridX + 1;
        yCheck = a_Node.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)//If the XPosition is in range of the array
        {
            if (yCheck >= 0 && yCheck < gridSizeY)//If the YPosition is in range of the array
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);//Add the grid to the available neighbors list
            }
        }
        //Check the Top Left side of the current node.
        xCheck = a_Node.gridX + 1;
        yCheck = a_Node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)//If the XPosition is in range of the array
        {
            if (yCheck >= 0 && yCheck < gridSizeY)//If the YPosition is in range of the array
            {
                NeighboringNodes.Add(grid[xCheck, yCheck]);//Add the grid to the available neighbors list
            }
        }

        return NeighboringNodes;//Return the neighbors list.
    }
    
    
    //Gets the closest node to the given world position.
    public Node NodeFromWorldPoint(Vector3 a_WorldPosition)
    {
        float xpoint = ((a_WorldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float ypoint = ((a_WorldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y);

        xpoint = Mathf.Clamp01(xpoint);
        ypoint = Mathf.Clamp01(ypoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xpoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * ypoint);

        return grid[x, y];
    }

    private void Update()
    {
        int i = 0;
        if (grid != null)//If the grid is not empty
        {
            foreach (Node n in grid)//Loop through every node in the grid
            {
                if (FinalPath != null)//If the final path is not empty
                {
                    if (FinalPath.Contains(n))//If the current node is in the final path
                    {
                        if(GlobalValues.newPath == true)
                        {
                            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            GameObject go = Instantiate(cube, new Vector3(n.Position.x, n.Position.y, n.Position.z), Quaternion.identity) as GameObject;
                            go.transform.localScale = new Vector3(2f, 2f, 2f);
                            go.transform.name = n.Position.x.ToString() + " " + n.Position.y.ToString() + " " + n.Position.z.ToString();
                            go.transform.parent = transform;
                            go.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
                        }
                        i += 1;
                        if(i == FinalPath.Count)
                        {
                            GlobalValues.newPath = false;
                        }
                        
                    }
                    
                }
            }
        }
    }
    
    //Function that draws the wireframe
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));//Draw a wire cube with the given dimensions from the Unity inspector

        if (grid != null)//If the grid is not empty
        {
            foreach (Node n in grid)//Loop through every node in the grid
            {
                if (n.IsWall)//If the current node is a wall node
                {
                    Gizmos.color = Color.white;//Set the color of the node
                }
                else
                {
                    Gizmos.color = Color.yellow;//Set the color of the node
                }
                if (FinalPath != null)//If the final path is not empty
                {
                    if (FinalPath.Contains(n))//If the current node is in the final path
                    {
                        Gizmos.color = Color.red;//Set the color of that node
                    }

                }


                Gizmos.DrawCube(n.Position, Vector3.one * (nodeDiameter - Distance));//Draw the node at the position of the node.
            }
        }
    }
}