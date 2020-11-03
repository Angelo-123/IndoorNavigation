using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Pathfinding : MonoBehaviour
{

    Grid grid;//For referencing the grid class
    public Transform StartPosition;//Starting position to pathfind from
    public Transform TargetPosition;//Starting position to pathfind to

    
    public Vector3 StartVec;
    public Vector3 TargetVec;
    public Text textBack;

    /*
    QR_Scanner_Script qrScript;
    InputSearch inputSearch;
    DropdownQR dropQrScript;
    */

    public bool notLoaded;

    public string StartPosString;
    public GameObject startPosObject;

    public string DestPosString;
    public GameObject destPosObject;

    



    private void Awake()//When the program starts
    {
        grid = GetComponent<Grid>();//Get a reference to the game manager
    }

    private void Start()
    {
        if (GlobalValues.pathScript == null)
        {
            GlobalValues.pathScript = GameObject.Find("GameManager").GetComponent<Pathfinding>();
        }
    }

    private void Update()//Every frame
    {
        
        checkStartPos();


        if (GlobalValues.startFloorLvl != GlobalValues.destFloorLvl)
        {
            changeDestPos();
            //GlobalValues.stairsReached = true;
        }
        else
        {
            checkDestPos();
        }

        if(GlobalValues.stairsReached == true)
        {
            newFloorPos();
        }
        
        FindPath(StartVec, TargetVec);

    }

    

    void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = grid.NodeFromWorldPoint(a_StartPos);//Gets the node closest to the starting position
        Node TargetNode = grid.NodeFromWorldPoint(a_TargetPos);//Gets the node closest to the target position


        List<Node> OpenList = new List<Node>();//List of nodes for the open list
        HashSet<Node> ClosedList = new HashSet<Node>();//Hashset of nodes for the closed list

        OpenList.Add(StartNode);//Add the starting node to the open list to begin the program

        while (OpenList.Count > 0)//Whilst there is something in the open list
        {
            Node CurrentNode = OpenList[0];//Create a node and set it to the first item in the open list
            for (int i = 1; i < OpenList.Count; i++)//Loop through the open list starting from the second object
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)//If the f cost of that object is less than or equal to the f cost of the current node
                {
                    CurrentNode = OpenList[i];//Set the current node to that object
                }
            }
            OpenList.Remove(CurrentNode);//Remove that from the open list
            ClosedList.Add(CurrentNode);//And add it to the closed list

            if (CurrentNode == TargetNode)//If the current node is the same as the target node
            {
                GetFinalPath(StartNode, TargetNode);//Calculate the final path
            }

            foreach (Node NeighborNode in grid.GetNeighboringNodes(CurrentNode))//Loop through each neighbor of the current node
            {
                if (!NeighborNode.IsWall || ClosedList.Contains(NeighborNode))//If the neighbor is a wall or has already been checked
                {
                    continue;//Skip it
                }
                int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighborNode);//Get the F cost of that neighbor

                if (MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))//If the f cost is greater than the g cost or it is not in the open list
                {
                    NeighborNode.gCost = MoveCost;//Set the g cost to the f cost
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, TargetNode);//Set the h cost
                    NeighborNode.Parent = CurrentNode;//Set the parent of the node for retracing steps

                    if (!OpenList.Contains(NeighborNode))//If the neighbor is not in the openlist
                    {
                        OpenList.Add(NeighborNode);//Add it to the list
                    }
                }
            }

        }
    }



    void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        List<Node> FinalPath = new List<Node>();//List to hold the path sequentially 
        Node CurrentNode = a_EndNode;//Node to store the current node being checked

        while (CurrentNode != a_StartingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            FinalPath.Add(CurrentNode);//Add that node to the final path
            CurrentNode = CurrentNode.Parent;//Move onto its parent node
        }

        FinalPath.Reverse();//Reverse the path to get the correct order

        grid.FinalPath = FinalPath;//Set the final path

    }

    int GetManhattenDistance(Node a_nodeA, Node a_nodeB)
    {
        int ix = Mathf.Abs(a_nodeA.gridX - a_nodeB.gridX);//x1-x2
        int iy = Mathf.Abs(a_nodeA.gridY - a_nodeB.gridY);//y1-y2

        return ix + iy;//Return the sum
    }




    void checkStartPos()
    {
        /*
        dropQrScript = GlobalValues.dropQrScript;
        StartPosString = dropQrScript.myStartPos;
        */
        StartPosString = GlobalValues.myStartPosString;

        //Ground Floor
        if(GlobalValues.startFloorLvl == 0)
        {
            if (StartPosString == "Entrance East N1A-GF-G99K")//Entrance East
            {
                startPosObject = GameObject.Find("Entrance East N1A-GF-G99K");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "Entrance South N1A-GF")//Entrance South
            {
                startPosObject = GameObject.Find("Entrance South N1A-GF");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "Lift Main N1A-GF-G99F")//Lift Main
            {
                startPosObject = GameObject.Find("Lift Main N1A-GF-G99F");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "Staircase East N1A-GF-G99G")//Staircase East
            {
                startPosObject = GameObject.Find("Staircase East N1A-GF-G99G");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "Staircase West N1A-GF-G99H")//Staircase West
            {
                startPosObject = GameObject.Find("Staircase West N1A-GF-G99H");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "Waiting Area N1A-GF-G42")//Waiting Area
            {
                startPosObject = GameObject.Find("Waiting Area N1A-GF-G42");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "Office Passage N1A-GF-G99I")//Office Passage
            {
                startPosObject = GameObject.Find("Office Passage N1A-GF-G99I");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
        }

        //First Floor
        if (GlobalValues.startFloorLvl == 1)
        {
            if (StartPosString == "FF 150 WAITING AREA")//WAITING AREA
            {
                startPosObject = GameObject.Find("FF 150 WAITING AREA");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "FF 199G STAIRS EAST")//STAIRS EAST
            {
                startPosObject = GameObject.Find("FF 199G STAIRS EAST");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "FF 199H STAIRS WEST")//STAIRS WEST
            {
                startPosObject = GameObject.Find("FF 199H STAIRS WEST");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "FF ENTRANCE SOUTH")//ENTRANCE SOUTH
            {
                startPosObject = GameObject.Find("FF ENTRANCE SOUTH");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "FF HALL SOUTH")//HALL SOUTH
            {
                startPosObject = GameObject.Find("FF 150 WAITING AREA");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "FF HALL NORTH")//HALL NORTH
            {
                startPosObject = GameObject.Find("FF HALL NORTH");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }

        }


        //Second Floor
        if (GlobalValues.startFloorLvl == 2)
        {
            if (StartPosString == "SF Hall North")//SF Hall North
            {
                startPosObject = GameObject.Find("SF Hall North");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "SF Hall South")//SF Hall South
            {
                startPosObject = GameObject.Find("SF Hall South");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "SF 236 Waiting Area")//SF 236 Waiting Area
            {
                startPosObject = GameObject.Find("SF 236 Waiting Area");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "SF 299C Staircase West")//SF 299C Staircase West
            {
                startPosObject = GameObject.Find("SF 299C Staircase West");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "SF 299D Staircase East")//SF 299D Staircase East
            {
                startPosObject = GameObject.Find("SF 299D Staircase East");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "SF 299E Lift Main")//SF 299E Lift Main
            {
                startPosObject = GameObject.Find("SF 299E Lift Main");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            if (StartPosString == "SF 299H Passage South")//SF 299H Passage South
            {
                startPosObject = GameObject.Find("SF 299H Passage South");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }

        }


        StartPosition.position = StartVec;
        print("StartVect set: " + StartVec);
    }

    void checkDestPos()
    {
        DestPosString = GlobalValues.myDestPosString;

         //Ground Floor
        if (GlobalValues.destFloorLvl == 0)
        {
            if (DestPosString == "G01 - Meeting Room")//G01 - Meeting Room
            {
                destPosObject = GameObject.Find("G01 - Meeting Room");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G02 - Meeting Room")//G02 - Meeting Room
            {
                destPosObject = GameObject.Find("G02 - Meeting Room");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G03 - Meeting Room")//G03 - Meeting Room
            {
                destPosObject = GameObject.Find("G03 - Meeting Room");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G04 - Koffee Station")//G04 - Koffee Station
            {
                destPosObject = GameObject.Find("G04 - Koffee Station");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G05 - Kitchen")//G05 - Kitchen
            {
                destPosObject = GameObject.Find("G05 - Kitchen");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G98I - Electrical")//G98I - Electrical
            {
                destPosObject = GameObject.Find("G98I - Electrical");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G98H - Data")//G98H - Data
            {
                destPosObject = GameObject.Find("G98H - Data");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G99F - Lift")//G99F - Lift
            {
                destPosObject = GameObject.Find("G99F - Lift");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G99J - Lift")//G99J - Lift
            {
                destPosObject = GameObject.Find("G99J - Lift");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G21 - Computer Lab")//G21 - Computer Lab
            {
                destPosObject = GameObject.Find("G21 - Computer Lab");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G22 - Computer Lab")//G22 - Computer Lab
            {
                destPosObject = GameObject.Find("G22 - Computer Lab");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G23 - Storage")//G23 - Storage
            {
                destPosObject = GameObject.Find("G23 - Storage");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G24 - Ladies Bathroom")//G24 - Ladies Bathroom
            {
                destPosObject = GameObject.Find("G24 - Ladies Bathroom");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G25 - Gents Bathroom")//G25 - Gents Bathroom
            {
                destPosObject = GameObject.Find("G25 - Gents Bathroom");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G26 - Storage")//G26 - Storage
            {
                destPosObject = GameObject.Find("G26 - Storage");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G27 - Conference Room")//G27 - Conference Room
            {
                destPosObject = GameObject.Find("G27 - Conference Room");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G28 - Office")//G28 - Office
            {
                destPosObject = GameObject.Find("G28 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G29 - Office")//G29 - Office
            {
                destPosObject = GameObject.Find("G29 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G30 - Office")//G30 - Office
            {
                destPosObject = GameObject.Find("G30 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G31 - Office")//G31 - Office
            {
                destPosObject = GameObject.Find("G31 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G32 - Office")//G32 - Office
            {
                destPosObject = GameObject.Find("G32 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G32A - Office")//G32A - Office
            {
                destPosObject = GameObject.Find("G32A - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G32B - Office")//G32B - Office
            {
                destPosObject = GameObject.Find("G32B - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G33 - Office")//G33 - Office
            {
                destPosObject = GameObject.Find("G33 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G34 - Office")//G34 - Office
            {
                destPosObject = GameObject.Find("G34 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G35 - Office")//G35 - Office
            {
                destPosObject = GameObject.Find("G35 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G36 - Office")//G36 - Office
            {
                destPosObject = GameObject.Find("G36 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G37 - Storage")//G37 - Storage
            {
                destPosObject = GameObject.Find("G37 - Storage");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G38 - Storage")//G38 - Storage
            {
                destPosObject = GameObject.Find("G38 - Storage");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G39 - Office")//G39 - Office
            {
                destPosObject = GameObject.Find("G39 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G40 - Office")//G40 - Office
            {
                destPosObject = GameObject.Find("G40 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G41 - Office")//G41 - Office
            {
                destPosObject = GameObject.Find("G41 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G42 - Waiting Area")//G42 - Waiting Area
            {
                destPosObject = GameObject.Find("G42 - Waiting Area");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G43 - Office")//G43 - Office
            {
                destPosObject = GameObject.Find("G43 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G44 - Office")//G44 - Office
            {
                destPosObject = GameObject.Find("G44 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G45 - Kitchen")//G45 - Kitchen
            {
                destPosObject = GameObject.Find("G45 - Kitchen");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "G46 - Computer Lab")//G46 - Computer Lab
            {
                destPosObject = GameObject.Find("G46 - Computer Lab");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
        }

        //First Floor
        if (GlobalValues.destFloorLvl == 1)
        {
            if (DestPosString == "FF 101 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 101 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 102 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 102 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 103 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 103 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 104 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 104 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 105 PROJECT LAB")//
            {
                destPosObject = GameObject.Find("FF 105 PROJECT LAB");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 106 PROJECT LAB")//
            {
                destPosObject = GameObject.Find("FF 106 PROJECT LAB");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 107 COFFEE STATION")//
            {
                destPosObject = GameObject.Find("FF 107 COFFEE STATION");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 108 STORAGE")//
            {
                destPosObject = GameObject.Find("FF 108 STORAGE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 119J LIFT")//
            {
                destPosObject = GameObject.Find("FF 119J LIFT");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 198G DATA")//
            {
                destPosObject = GameObject.Find("FF 198G DATA");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 198H ELECTRICAL")//
            {
                destPosObject = GameObject.Find("FF 198H ELECTRICAL");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 123 SEMINAR")//
            {
                destPosObject = GameObject.Find("FF 123 SEMINAR");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 124 PROJECT LAB")//
            {
                destPosObject = GameObject.Find("FF 124 PROJECT LAB");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 126 PROJECT LAB")//
            {
                destPosObject = GameObject.Find("FF 126 PROJECT LAB");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 128 PROJECT LAB")//
            {
                destPosObject = GameObject.Find("FF 128 PROJECT LAB");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 129 STORAGE")//
            {
                destPosObject = GameObject.Find("FF 129 STORAGE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 130 LADIES")//
            {
                destPosObject = GameObject.Find("FF 130 LADIES");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 131 GENTS")//
            {
                destPosObject = GameObject.Find("FF 131 GENTS");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 132 SHOWER")//
            {
                destPosObject = GameObject.Find("FF 132 SHOWER");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 133 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 133 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 134 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 134 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 135 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 135 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 136 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 136 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 137 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 137 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 138 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 138 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 139 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 139 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 140 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 140 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 141 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 141 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 142 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 142 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 143 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 143 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 144 STORAGE")//
            {
                destPosObject = GameObject.Find("FF 144 STORAGE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 145 STORAGE")//
            {
                destPosObject = GameObject.Find("FF 145 STORAGE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 146 STORAGE")//
            {
                destPosObject = GameObject.Find("FF 146 STORAGE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 147 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 147 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 148 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 148 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 149 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 149 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 150 WAITING AREA")//
            {
                destPosObject = GameObject.Find("FF 150 WAITING AREA");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 151 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 151 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 152 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 152 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 153 KITCHEN")//
            {
                destPosObject = GameObject.Find("FF 153 KITCHEN");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 154 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 154 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 155 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 155 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 156 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 156 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 157 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 157 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "FF 158 OFFICE")//
            {
                destPosObject = GameObject.Find("FF 158 OFFICE");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }

        }


        //Second Floor
        if (GlobalValues.destFloorLvl == 2)
        {
            if (DestPosString == "SF 201 - Office")//
            {
                destPosObject = GameObject.Find("SF 201 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 202 - Office")//
            {
                destPosObject = GameObject.Find("SF 202 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 203 - Office")//
            {
                destPosObject = GameObject.Find("SF 203 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 204 - Office")//
            {
                destPosObject = GameObject.Find("SF 204 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 205 - Project Lab")//
            {
                destPosObject = GameObject.Find("SF 205 - Project Lab");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 206 - Project Lab")//
            {
                destPosObject = GameObject.Find("SF 206 - Project Lab");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 207 - Koffee Station")//
            {
                destPosObject = GameObject.Find("SF 207 - Koffee Station");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 208 - Kitchen")//
            {
                destPosObject = GameObject.Find("SF 208 - Kitchen");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 299E - Lift")//
            {
                destPosObject = GameObject.Find("SF 299E - Lift");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 299G - Lift")//
            {
                destPosObject = GameObject.Find("SF 299G - Lift");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 298A - Data")//
            {
                destPosObject = GameObject.Find("SF 298A - Data");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 298B - Electrical")//
            {
                destPosObject = GameObject.Find("SF 298B - Electrical");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 209 - Project Lab")//
            {
                destPosObject = GameObject.Find("SF 209 - Project Lab");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 210 - Project Lab")//
            {
                destPosObject = GameObject.Find("SF 210 - Project Lab");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 211 - Project Lab")//
            {
                destPosObject = GameObject.Find("SF 211 - Project Lab");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 212 - Project Lab")//
            {
                destPosObject = GameObject.Find("SF 212 - Project Lab");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 213 - Project Lab")//
            {
                destPosObject = GameObject.Find("SF 213 - Project Lab");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 214 - Project Lab")//
            {
                destPosObject = GameObject.Find("SF 214 - Project Lab");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 215 - Storage")//
            {
                destPosObject = GameObject.Find("SF 215 - Storage");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 216 - Ladies Bathroom")//
            {
                destPosObject = GameObject.Find("SF 216 - Ladies Bathroom");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 217 - Gents Bathroom")//
            {
                destPosObject = GameObject.Find("SF 217 - Gents Bathroom");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 218 - Shower")//
            {
                destPosObject = GameObject.Find("SF 218 - Shower");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 219 - Conference Room")//
            {
                destPosObject = GameObject.Find("SF 219 - Conference Room");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 220 - Office")//
            {
                destPosObject = GameObject.Find("SF 220 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 221 - Office")//
            {
                destPosObject = GameObject.Find("SF 221 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 222 - Office")//
            {
                destPosObject = GameObject.Find("SF 222 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 223 - Office")//
            {
                destPosObject = GameObject.Find("SF 223 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 224 - Office")//
            {
                destPosObject = GameObject.Find("SF 224 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 225 - Office")//
            {
                destPosObject = GameObject.Find("SF 225 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 226 - Office")//
            {
                destPosObject = GameObject.Find("SF 226 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 227 - Office")//
            {
                destPosObject = GameObject.Find("SF 227 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 228 - Office")//
            {
                destPosObject = GameObject.Find("SF 228 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 229 - Office")//
            {
                destPosObject = GameObject.Find("SF 229 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 230 - Copy Room")//
            {
                destPosObject = GameObject.Find("SF 230 - Copy Room");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 231 - Storage")//
            {
                destPosObject = GameObject.Find("SF 231 - Storage");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 233 - Office")//
            {
                destPosObject = GameObject.Find("SF 233 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 234 - Conference")//
            {
                destPosObject = GameObject.Find("SF 234 - Conference");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 235 - Office")//
            {
                destPosObject = GameObject.Find("SF 235 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 236 - Waiting Area")//
            {
                destPosObject = GameObject.Find("SF 236 - Waiting Area");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 237 - Office")//
            {
                destPosObject = GameObject.Find("SF 237 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 238 - Office")//
            {
                destPosObject = GameObject.Find("SF 238 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 239 - Kitchen")//
            {
                destPosObject = GameObject.Find("SF 239 - Kitchen");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 240 - Office")//
            {
                destPosObject = GameObject.Find("SF 240 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 241 - Office")//
            {
                destPosObject = GameObject.Find("SF 241 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 242 - Office")//
            {
                destPosObject = GameObject.Find("SF 242 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 243 - Office")//
            {
                destPosObject = GameObject.Find("SF 243 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            if (DestPosString == "SF 244 - Office")//
            {
                destPosObject = GameObject.Find("SF 244 - Office");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }

        }


            TargetPosition.position = TargetVec;
        print("TargetVec set: " + TargetVec);
    }





    private void changeDestPos()
    {
        if (GlobalValues.destFloorBlock == 1)
        {
            GlobalValues.eastStaircase = false;
        }
        else if (GlobalValues.destFloorBlock == 4)
        {
            GlobalValues.eastStaircase = true;
        }
        else if (GlobalValues.destFloorBlock == 2)
        {
            if ((GlobalValues.startFloorBlock == 1) || (GlobalValues.startFloorBlock == 2))
            {
                GlobalValues.eastStaircase = false;
            }
            else if ((GlobalValues.startFloorBlock == 3) || (GlobalValues.startFloorBlock == 4))
            {
                GlobalValues.eastStaircase = true;
            }
        }
        else if (GlobalValues.destFloorBlock == 3)
        {
            if ((GlobalValues.startFloorBlock == 1) || (GlobalValues.startFloorBlock == 2))
            {
                GlobalValues.eastStaircase = false;
            }
            else if ((GlobalValues.startFloorBlock == 3) || (GlobalValues.startFloorBlock == 4))
            {
                GlobalValues.eastStaircase = true;
            }
        }

        //If the user starts on the ground floor
        if (GlobalValues.startFloorLvl == 0)
        {
            if (GlobalValues.eastStaircase)
            {
                destPosObject = GameObject.Find("Staircase East N1A-GF-G99G");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            else if (!GlobalValues.eastStaircase)
            {
                destPosObject = GameObject.Find("Staircase West N1A-GF-G99H");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }

        }

        //If the user starts on the first floor
        if (GlobalValues.startFloorLvl == 1)
        {
            if (GlobalValues.eastStaircase)
            {
                destPosObject = GameObject.Find("FF 199G STAIRS EAST");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            else if (!GlobalValues.eastStaircase)
            {
                destPosObject = GameObject.Find("FF 199H STAIRS WEST");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
        }

        //If the user starts on the second floor
        if (GlobalValues.startFloorLvl == 2)
        {
            if (GlobalValues.eastStaircase)
            {
                destPosObject = GameObject.Find("SF 299D Staircase East");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
            else if (!GlobalValues.eastStaircase)
            {
                destPosObject = GameObject.Find("SF 299C Staircase West");
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
        }
    }


    private void newFloorPos()
    {
        /*
        if(GlobalValues.oldFloorLvl == 0)
        {
            startPosObject = GameObject.Find("FF 199G STAIRS EAST");
            StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
        }


        if (GlobalValues.oldFloorLvl == 1)
        {
            startPosObject = GameObject.Find("Staircase East N1A-GF-G99G");
            StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
        }
        */

        

        if (GlobalValues.eastStaircase)
        {
            if (GlobalValues.startFloorLvl == 0)
            {
                startPosObject = GameObject.Find("Staircase East N1A-GF-G99G");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            else if (GlobalValues.startFloorLvl == 1)
            {
                startPosObject = GameObject.Find("FF 199G STAIRS EAST");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            else if (GlobalValues.startFloorLvl == 2)
            {
                startPosObject = GameObject.Find("SF 299D Staircase East");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }

        }
        else if (!GlobalValues.eastStaircase)
        {
            if (GlobalValues.startFloorLvl == 0)
            {
                startPosObject = GameObject.Find("Staircase West N1A-GF-G99H");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            else if (GlobalValues.startFloorLvl == 1)
            {
                startPosObject = GameObject.Find("FF 199H STAIRS WEST");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            else if (GlobalValues.startFloorLvl == 2)
            {
                startPosObject = GameObject.Find("SF 299C Staircase West");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
        }
    }
}