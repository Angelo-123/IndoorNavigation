using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pathfinding : MonoBehaviour
{

    Grid grid;//For referencing the grid class
    public Transform StartPosition;//Starting position to pathfind from
    public Transform TargetPosition;//Starting position to pathfind to

    
    public Vector3 StartVec;
    public Vector3 TargetVec;
    public Text textBack;


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

        StartCoroutine(LateStart((float)0.1));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);


        checkStartPos();

        if (GlobalValues.startFloorLvl != GlobalValues.destFloorLvl)
        {
            changeDestPos();
        }
        else
        {
            checkDestPos();
        }

        if (GlobalValues.stairsReached == true)
        {
            newFloorPos();
        }

        FindPath(StartVec, TargetVec);
    }



    private void Update()//Every frame
    {

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

        GlobalValues.xPosArray = new List<float>();
        GlobalValues.zPosArray = new List<float>();

        for (int i = 0; i < FinalPath.Count; i++)
        {
            GlobalValues.xPosArray.Add(FinalPath[i].Position.x);
            GlobalValues.zPosArray.Add(FinalPath[i].Position.z);
        }
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
            else if (StartPosString == "Entrance South N1A-GF")//Entrance South
            {
                startPosObject = GameObject.Find("Entrance South N1A-GF");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            else if (StartPosString == "Lift Main N1A-GF-G99F")//Lift Main
            {
                startPosObject = GameObject.Find("Lift Main N1A-GF-G99F");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            else if (StartPosString == "Staircase East N1A-GF-G99G")//Staircase East
            {
                startPosObject = GameObject.Find("Staircase East N1A-GF-G99G");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            else if (StartPosString == "Staircase West N1A-GF-G99H")//Staircase West
            {
                startPosObject = GameObject.Find("Staircase West N1A-GF-G99H");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            else if (StartPosString == "Waiting Area N1A-GF-G42")//Waiting Area
            {
                startPosObject = GameObject.Find("Waiting Area N1A-GF-G42");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            else if (StartPosString == "Office Passage N1A-GF-G99I")//Office Passage
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
            else if (StartPosString == "FF 199G STAIRS EAST")//STAIRS EAST
            {
                startPosObject = GameObject.Find("FF 199G STAIRS EAST");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            else if (StartPosString == "FF 199H STAIRS WEST")//STAIRS WEST
            {
                startPosObject = GameObject.Find("FF 199H STAIRS WEST");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            else if (StartPosString == "FF ENTRANCE SOUTH")//ENTRANCE SOUTH
            {
                startPosObject = GameObject.Find("FF ENTRANCE SOUTH");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            else if (StartPosString == "FF HALL SOUTH")//HALL SOUTH
            {
                startPosObject = GameObject.Find("FF 150 WAITING AREA");
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            else if (StartPosString == "FF HALL NORTH")//HALL NORTH
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






        //HomeFloor
        if (GlobalValues.startFloorLvl == 5)
        {
            switch (StartPosString)
            {
                case "Home - Study":
                    startPosObject = GameObject.Find("Home - Study");
                    break;
                case "Home - Kitchen":
                    startPosObject = GameObject.Find("Home - Kitchen");
                    break;
                case "Home - Living":
                    startPosObject = GameObject.Find("Home - Living");
                    break;
                case "Home - Balcony":
                    startPosObject = GameObject.Find("Home - Balcony");
                    break;
                case "Home - Stairs":
                    startPosObject = GameObject.Find("Home - Stairs");
                    break;
                default:
                    break;
            }

            if (startPosObject != null)
            {
                StartVec = new Vector3(startPosObject.transform.position.x, startPosObject.transform.position.y, startPosObject.transform.position.z);
            }
            
        }



        //Home First Floor
        if (GlobalValues.startFloorLvl == 6)
        {
            switch (StartPosString)
            {
                case "HomeFF -  Bedroom A":
                    startPosObject = GameObject.Find("HomeFF -  Bedroom A");
                    break;
                case "HomeFF -  Bedroom B":
                    startPosObject = GameObject.Find("HomeFF -  Bedroom B");
                    break;
                case "HomeFF -  Bedroom Main":
                    startPosObject = GameObject.Find("HomeFF -  Bedroom Main");
                    break;
                case "HomeFF -  Bathroom":
                    startPosObject = GameObject.Find("HomeFF -  Bathroom");
                    break;
                case "HomeFF -  Stairs":
                    startPosObject = GameObject.Find("HomeFF -  Stairs");
                    break;
                case "HomeFF -  Bathroom Main":
                    startPosObject = GameObject.Find("HomeFF -  Bathroom Main");
                    break;
                default:
                    break;
            }

            if (startPosObject != null)
            {
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
            switch (DestPosString)
            {
                case "G01 - Meeting Roo":
                    destPosObject = GameObject.Find("G01 - Meeting Room");
                    break;
                case "G02 - Meeting Room":
                    destPosObject = GameObject.Find("G01 - Meeting Room");
                    break;
                case "G03 - Meeting Room":
                    destPosObject = GameObject.Find("G03 - Meeting Room");
                    break;
                case "G04 - Koffee Station":
                    destPosObject = GameObject.Find("G04 - Koffee Station");
                    break;
                case "G05 - Kitchen":
                    destPosObject = GameObject.Find("G05 - Kitchen");
                    break;
                case "G98I - Electrical":
                    destPosObject = GameObject.Find("G98I - Electrical");
                    break;
                case "G98H - Data":
                    destPosObject = GameObject.Find("G98H - Data");
                    break;
                case "G99F - Lift":
                    destPosObject = GameObject.Find("G99F - Lift");
                    break;
                case "G99J - Lift":
                    destPosObject = GameObject.Find("G99J - Lift");
                    break;
                case "G21 - Computer Lab":
                    destPosObject = GameObject.Find("G21 - Computer Lab");
                    break;
                case "G22 - Computer Lab":
                    destPosObject = GameObject.Find("G22 - Computer Lab");
                    break;
                case "G23 - Storage":
                    destPosObject = GameObject.Find("G23 - Storage");
                    break;
                case "G24 - Ladies Bathroom":
                    destPosObject = GameObject.Find("G24 - Ladies Bathroom");
                    break;
                case "G25 - Gents Bathroom":
                    destPosObject = GameObject.Find("G25 - Gents Bathroom");
                    break;
                case "G26 - Storage":
                    destPosObject = GameObject.Find("G26 - Storage");
                    break;
                case "G27 - Conference Room":
                    destPosObject = GameObject.Find("G27 - Conference Room");
                    break;
                case "G28 - Office":
                    destPosObject = GameObject.Find("G28 - Office");
                    break;
                case "G29 - Office":
                    destPosObject = GameObject.Find("G29 - Office");
                    break;
                case "G30 - Office":
                    destPosObject = GameObject.Find("G30 - Office");
                    break;
                case "G31 - Office":
                    destPosObject = GameObject.Find("G31 - Office");
                    break;
                case "G32 - Office":
                    destPosObject = GameObject.Find("G32 - Office");
                    break;
                case "G32A - Office":
                    destPosObject = GameObject.Find("G32A - Office");
                    break;
                case "G32B - Office":
                    destPosObject = GameObject.Find("G32B - Office");
                    break;
                case "G33 - Office":
                    destPosObject = GameObject.Find("G33 - Office");
                    break;
                case "G34 - Office":
                    destPosObject = GameObject.Find("G34 - Office");
                    break;
                case "G35 - Office":
                    destPosObject = GameObject.Find("G35 - Office");
                    break;
                case "G36 - Office":
                    destPosObject = GameObject.Find("G36 - Office");
                    break;
                case "G37 - Storage":
                    destPosObject = GameObject.Find("G37 - Storage");
                    break;
                case "G38 - Storage":
                    destPosObject = GameObject.Find("G38 - Storage");
                    break;
                case "G39 - Office":
                    destPosObject = GameObject.Find("G39 - Office");
                    break;
                case "G40 - Office":
                    destPosObject = GameObject.Find("G40 - Office");
                    break;
                case "G41 - Office":
                    destPosObject = GameObject.Find("G41 - Office");
                    break;
                case "G42 - Waiting Area":
                    destPosObject = GameObject.Find("G42 - Waiting Area");
                    break;
                case "G43 - Office":
                    destPosObject = GameObject.Find("G43 - Office");
                    break;
                case "G44 - Office":
                    destPosObject = GameObject.Find("G44 - Office");
                    break;
                case "G45 - Kitchen":
                    destPosObject = GameObject.Find("G45 - Kitchen");
                    break;
                case "G46 - Computer Lab":
                    destPosObject = GameObject.Find("G46 - Computer Lab");
                    break;
                default:
                    break;
            }

            if (destPosObject != null)
            {
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }

        }

        //First Floor
        if (GlobalValues.destFloorLvl == 1)
        {
            switch (DestPosString)
            {
                case "FF 101 OFFICE":
                    destPosObject = GameObject.Find("FF 101 OFFICE");
                    break;
                case "FF 102 OFFICE":
                    destPosObject = GameObject.Find("FF 102 OFFICE");
                    break;
                case "FF 103 OFFICE":
                    destPosObject = GameObject.Find("FF 103 OFFICE");
                    break;
                case "FF 104 OFFICE":
                    destPosObject = GameObject.Find("FF 104 OFFICE");
                    break;
                case "FF 105 PROJECT LAB":
                    destPosObject = GameObject.Find("FF 105 PROJECT LAB");
                    break;
                case "FF 106 PROJECT LAB":
                    destPosObject = GameObject.Find("FF 106 PROJECT LAB");
                    break;
                case "FF 107 COFFEE STATION":
                    destPosObject = GameObject.Find("FF 107 COFFEE STATION");
                    break;
                case "FF 108 STORAGE":
                    destPosObject = GameObject.Find("FF 108 STORAGE");
                    break;
                case "FF 119J LIFT":
                    destPosObject = GameObject.Find("FF 119J LIFT");
                    break;
                case "FF 198G DATA":
                    destPosObject = GameObject.Find("FF 198G DATA");
                    break;
                case "FF 198H ELECTRICAL":
                    destPosObject = GameObject.Find("FF 198H ELECTRICAL");
                    break;
                case "FF 123 SEMINAR":
                    destPosObject = GameObject.Find("FF 123 SEMINAR");
                    break;
                case "FF 124 PROJECT LAB":
                    destPosObject = GameObject.Find("FF 124 PROJECT LAB");
                    break;
                case "FF 126 PROJECT LAB":
                    destPosObject = GameObject.Find("FF 126 PROJECT LAB");
                    break;
                case "FF 128 PROJECT LAB":
                    destPosObject = GameObject.Find("FF 128 PROJECT LAB");
                    break;
                case "FF 129 STORAGE":
                    destPosObject = GameObject.Find("FF 129 STORAGE");
                    break;
                case "FF 130 LADIES":
                    destPosObject = GameObject.Find("FF 130 LADIES");
                    break;
                case "FF 131 GENTS":
                    destPosObject = GameObject.Find("FF 131 GENTS");
                    break;
                case "FF 132 SHOWER":
                    destPosObject = GameObject.Find("FF 132 SHOWER");
                    break;
                case "FF 133 OFFICE":
                    destPosObject = GameObject.Find("FF 133 OFFICE");
                    break;
                case "FF 134 OFFICE":
                    destPosObject = GameObject.Find("FF 134 OFFICE");
                    break;
                case "FF 135 OFFICE":
                    destPosObject = GameObject.Find("FF 135 OFFICE");
                    break;
                case "FF 136 OFFICE":
                    destPosObject = GameObject.Find("FF 136 OFFICE");
                    break;
                case "FF 137 OFFICE":
                    destPosObject = GameObject.Find("FF 137 OFFICE");
                    break;
                case "FF 138 OFFICE":
                    destPosObject = GameObject.Find("FF 138 OFFICE");
                    break;
                case "FF 139 OFFICE":
                    destPosObject = GameObject.Find("FF 139 OFFICE");
                    break;
                case "FF 140 OFFICE":
                    destPosObject = GameObject.Find("FF 140 OFFICE");
                    break;
                case "FF 141 OFFICE":
                    destPosObject = GameObject.Find("FF 141 OFFICE");
                    break;
                case "FF 142 OFFICE":
                    destPosObject = GameObject.Find("FF 142 OFFICE");
                    break;
                case "FF 143 OFFICE":
                    destPosObject = GameObject.Find("FF 143 OFFICE");
                    break;
                case "FF 144 STORAGE":
                    destPosObject = GameObject.Find("FF 144 STORAGE");
                    break;
                case "FF 145 STORAGE":
                    destPosObject = GameObject.Find("FF 145 STORAGE");
                    break;
                case "FF 146 STORAGE":
                    destPosObject = GameObject.Find("FF 146 STORAGE");
                    break;
                case "FF 147 OFFICE":
                    destPosObject = GameObject.Find("FF 147 OFFICE");
                    break;
                case "FF 148 OFFICE":
                    destPosObject = GameObject.Find("FF 148 OFFICE");
                    break;
                case "FF 149 OFFICE":
                    destPosObject = GameObject.Find("FF 149 OFFICE");
                    break;
                case "FF 150 WAITING AREA":
                    destPosObject = GameObject.Find("FF 150 WAITING AREA");
                    break;
                case "FF 151 OFFICE":
                    destPosObject = GameObject.Find("FF 151 OFFICE");
                    break;
                case "FF 152 OFFICE":
                    destPosObject = GameObject.Find("FF 152 OFFICE");
                    break;
                case "FF 153 KITCHEN":
                    destPosObject = GameObject.Find("FF 153 KITCHEN");
                    break;
                case "FF 154 OFFICE":
                    destPosObject = GameObject.Find("FF 154 OFFICE");
                    break;
                case "FF 155 OFFICE":
                    destPosObject = GameObject.Find("FF 155 OFFICE");
                    break;
                case "FF 156 OFFICE":
                    destPosObject = GameObject.Find("FF 156 OFFICE");
                    break;
                case "FF 157 OFFICE":
                    destPosObject = GameObject.Find("FF 157 OFFICE");
                    break;
                case "FF 158 OFFICE":
                    destPosObject = GameObject.Find("FF 158 OFFICE");
                    break;
                default:
                    break;
            }

            if (destPosObject != null)
            {
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
        }


        //Second Floor
        if (GlobalValues.destFloorLvl == 2)
        {
            switch (DestPosString)
            {
                case "SF 201 - Office":
                    destPosObject = GameObject.Find("SF 201 - Office");
                    break;
                case "SF 202 - Office":
                    destPosObject = GameObject.Find("SF 202 - Office");
                    break;
                case "SF 203 - Office":
                    destPosObject = GameObject.Find("SF 203 - Office");
                    break;
                case "SF 204 - Office":
                    destPosObject = GameObject.Find("SF 204 - Office");
                    break;
                case "SF 205 - Project Lab":
                    destPosObject = GameObject.Find("SF 205 - Project Lab");
                    break;
                case "SF 206 - Project Lab":
                    destPosObject = GameObject.Find("SF 206 - Project Lab");
                    break;
                case "SF 207 - Koffee Station":
                    destPosObject = GameObject.Find("SF 207 - Koffee Station");
                    break;
                case "SF 208 - Kitchen":
                    destPosObject = GameObject.Find("SF 208 - Kitchen");
                    break;
                case "SF 299E - Lift":
                    destPosObject = GameObject.Find("SF 299E - Lift");
                    break;
                case "SF 299G - Lift":
                    destPosObject = GameObject.Find("SF 299G - Lift");
                    break;
                case "SF 298A - Data":
                    destPosObject = GameObject.Find("SF 298A - Data");
                    break;
                case "SF 298B - Electrical":
                    destPosObject = GameObject.Find("SF 298B - Electrical");
                    break;
                case "SF 209 - Project Lab":
                    destPosObject = GameObject.Find("SF 209 - Project Lab");
                    break;
                case "SF 210 - Project Lab":
                    destPosObject = GameObject.Find("SF 210 - Project Lab");
                    break;
                case "SF 211 - Project Lab":
                    destPosObject = GameObject.Find("SF 211 - Project Lab");
                    break;
                case "SF 212 - Project Lab":
                    destPosObject = GameObject.Find("SF 212 - Project Lab");
                    break;
                case "SF 213 - Project Lab":
                    destPosObject = GameObject.Find("SF 213 - Project Lab");
                    break;
                case "SF 214 - Project Lab":
                    destPosObject = GameObject.Find("SF 214 - Project Lab");
                    break;
                case "SF 215 - Storage":
                    destPosObject = GameObject.Find("SF 215 - Storage");
                    break;
                case "SF 216 - Ladies Bathroom":
                    destPosObject = GameObject.Find("SF 216 - Ladies Bathroom");
                    break;
                case "SF 217 - Gents Bathroom":
                    destPosObject = GameObject.Find("SF 217 - Gents Bathroom");
                    break;
                case "SF 218 - Shower":
                    destPosObject = GameObject.Find("SF 218 - Shower");
                    break;
                case "SF 219 - Conference Room":
                    destPosObject = GameObject.Find("SF 219 - Conference Room");
                    break;
                case "SF 220 - Office":
                    destPosObject = GameObject.Find("SF 220 - Office");
                    break;
                case "SF 221 - Office":
                    destPosObject = GameObject.Find("SF 221 - Office");
                    break;
                case "SF 222 - Office":
                    destPosObject = GameObject.Find("SF 222 - Office");
                    break;
                case "SF 223 - Office":
                    destPosObject = GameObject.Find("SF 223 - Office");
                    break;
                case "SF 224 - Office":
                    destPosObject = GameObject.Find("SF 224 - Office");
                    break;
                case "SF 225 - Office":
                    destPosObject = GameObject.Find("SF 225 - Office");
                    break;
                case "SF 226 - Office":
                    destPosObject = GameObject.Find("SF 226 - Office");
                    break;
                case "SF 227 - Office":
                    destPosObject = GameObject.Find("SF 227 - Office");
                    break;
                case "SF 228 - Office":
                    destPosObject = GameObject.Find("SF 228 - Office");
                    break;
                case "SF 229 - Office":
                    destPosObject = GameObject.Find("SF 229 - Office");
                    break;
                case "SF 230 - Copy Room":
                    destPosObject = GameObject.Find("SF 230 - Copy Room");
                    break;
                case "SF 231 - Storage":
                    destPosObject = GameObject.Find("SF 231 - Storage");
                    break;
                case "SF 233 - Office":
                    destPosObject = GameObject.Find("SF 233 - Office");
                    break;
                case "SF 234 - Conference":
                    destPosObject = GameObject.Find("SF 234 - Conference");
                    break;
                case "SF 235 - Office":
                    destPosObject = GameObject.Find("SF 235 - Office");
                    break;
                case "SF 236 - Waiting Area":
                    destPosObject = GameObject.Find("SF 236 - Waiting Area");
                    break;
                case "SF 237 - Office":
                    destPosObject = GameObject.Find("SF 237 - Office");
                    break;
                case "SF 238 - Office":
                    destPosObject = GameObject.Find("SF 238 - Office");
                    break;
                case "SF 239 - Kitchen":
                    destPosObject = GameObject.Find("SF 239 - Kitchen");
                    break;
                case "SF 240 - Office":
                    destPosObject = GameObject.Find("SF 240 - Office");
                    break;
                case "SF 241 - Office":
                    destPosObject = GameObject.Find("SF 241 - Office");
                    break;
                case "SF 242 - Office":
                    destPosObject = GameObject.Find("SF 242 - Office");
                    break;
                case "SF 243 - Office":
                    destPosObject = GameObject.Find("SF 243 - Office");
                    break;
                case "SF 244 - Office":
                    destPosObject = GameObject.Find("SF 244 - Office");
                    break;
                default:
                    break;
            }

            if (destPosObject != null)
            {
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
        }


        //HomeFloor
        if (GlobalValues.destFloorLvl == 5)
        {
            switch (DestPosString)
            {
                case "Home - Study":
                    destPosObject = GameObject.Find("Home - Study");
                    break;
                case "Home - Kitchen":
                    destPosObject = GameObject.Find("Home - Kitchen");
                    break;
                case "Home - Living":
                    destPosObject = GameObject.Find("Home - Living");
                    break;
                case "Home - Balcony":
                    destPosObject = GameObject.Find("Home - Balcony");
                    break;
                case "Home - Stairs":
                    destPosObject = GameObject.Find("Home - Stairs");
                    break;
                default:
                    break;
            }
        

            if (destPosObject != null)
            {
                TargetVec = new Vector3(destPosObject.transform.position.x, destPosObject.transform.position.y, destPosObject.transform.position.z);
            }
        }



        //Home First Floor
        if (GlobalValues.destFloorLvl == 6)
        {
            switch (DestPosString)
            {
                case "HomeFF -  Bedroom A":
                    destPosObject = GameObject.Find("HomeFF -  Bedroom A");
                    break;
                case "HomeFF -  Bedroom B":
                    destPosObject = GameObject.Find("HomeFF -  Bedroom B");
                    break;
                case "HomeFF -  Bedroom Main":
                    destPosObject = GameObject.Find("HomeFF -  Bedroom Main");
                    break;
                case "HomeFF -  Bathroom":
                    destPosObject = GameObject.Find("HomeFF -  Bathroom");
                    break;
                case "HomeFF - Stairs":
                    destPosObject = GameObject.Find("HomeFf - Stairs");
                    break;
                case "HomeFF -  Bathroom Main":
                    destPosObject = GameObject.Find("HomeFF -  Bathroom Main");
                    break;
                default:
                    break;
            }


            if (destPosObject != null)
            {
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