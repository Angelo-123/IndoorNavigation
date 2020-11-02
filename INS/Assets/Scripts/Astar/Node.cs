using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node{

	public int gridX;               //X Position in the Node Array
	public int gridY;               //Y Position in the Node Array

	public bool IsWall;             //Tells the program if this node is being obstructed.
	public Vector3 Position;        //The world position of the node.

	public Node Parent;             //For the AStar algoritm, will store what node it previously came from so it cn trace the shortest path.

	public int gCost;               //The cost of moving to the next square.
	public int hCost;               //The distance to the goal from this node.

	public int FCost { get { return gCost + hCost; } }  //Quick get function to add G cost and H Cost, and since we'll never need to edit FCost, we dont need a set function.

	public Node(bool a_IsWall, Vector3 a_Pos, int a_gridX, int a_gridY) //Constructor
	{
		IsWall = a_IsWall;          //Tells the program if this node is being obstructed.
		Position = a_Pos;           //The world position of the node.
		gridX = a_gridX;            //X Position in the Node Array
		gridY = a_gridY;            //Y Position in the Node Array
	}



}
