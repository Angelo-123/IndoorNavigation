using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownQR : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start()
    {
        var dropdown = transform.GetComponent<Dropdown>();

        dropdown.options.Clear();

        List<string> items = new List<string>();
        items.Add("Entrance East");
        items.Add("Entrance South");
        items.Add("Lift Main");
        items.Add("Staircase East");
        items.Add("Staircase West");
        items.Add("Waiting Area");
        items.Add("Office Passage");
        //First Floor
        items.Add("FF 150 WAITING AREA");
        items.Add("FF 199G STAIRS EAST");
        items.Add("FF 199H STAIRS WEST");
        items.Add("FF ENTRANCE SOUTH");
        items.Add("FF HALL SOUTH");
        items.Add("FF HALL NORTH");
        //Second Floor
        items.Add("SF Hall North");
        items.Add("SF Hall South");
        items.Add("SF 236 Waiting Area");
        items.Add("SF 299C Staircase West");
        items.Add("SF 299D Staircase East");
        items.Add("SF 299E Lift Main");
        items.Add("SF 299H Passage South");

        foreach (var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        DropdownItemSelected(dropdown);

        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });

    }

    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        print("index: " + index);
        UpdateDest(index);
    }

    private void UpdateDest(int index)
    {
        //Ground Floor
        if (index == 0)//Entrance East
        {
            GlobalValues.myStartPosString = "Entrance East N1A-GF-G99K";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 0;
            GlobalValues.startFloorBlock = 4;
        }
        if (index == 1)//Entrance South
        {
            GlobalValues.myStartPosString = "Entrance South N1A-GF";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 0;
            GlobalValues.startFloorBlock = 3;
        }
        if (index == 2)//Lift Main
        {
            GlobalValues.myStartPosString = "Lift Main N1A-GF-G99F";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 0;
            GlobalValues.startFloorBlock = 4;
        }
        if (index == 3)//Staircase East
        {
            GlobalValues.myStartPosString = "Staircase East N1A-GF-G99G";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 0;
            GlobalValues.startFloorBlock = 4;
        }
        if (index == 4)//Staircase West
        {
            GlobalValues.myStartPosString = "Staircase West N1A-GF-G99H";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 0;
            GlobalValues.startFloorBlock = 1;
        }
        if (index == 5)//Waiting Area
        {
            GlobalValues.myStartPosString = "Waiting Area N1A-GF-G42";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 0;
            GlobalValues.startFloorBlock = 4;
        }
        if (index == 6)//Office Passage
        {
            GlobalValues.myStartPosString = "Office Passage N1A-GF-G99I";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 0;
            GlobalValues.startFloorBlock = 2;
        }

        //First Floor
        if (index == 7)//FF 150 WAITING AREA
        {
            GlobalValues.myStartPosString = "FF 150 WAITING AREA";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 1; 
            GlobalValues.startFloorBlock = 4;
        }
        if (index == 8)//FF 199G STAIRS EAST
        {
            GlobalValues.myStartPosString = "FF 199G STAIRS EAST";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 1;
            GlobalValues.startFloorBlock = 4;
        }
        if (index == 9)//FF 199H STAIRS WEST
        {
            GlobalValues.myStartPosString = "FF 199H STAIRS WEST";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 1;
            GlobalValues.startFloorBlock = 1;
        }
        if (index == 10)//FF ENTRANCE SOUTH
        {
            GlobalValues.myStartPosString = "FF ENTRANCE SOUTH";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 1;
            GlobalValues.startFloorBlock = 3;
        }
        if (index == 11)//FF HALL SOUTH
        {
            GlobalValues.myStartPosString = "FF HALL SOUTH";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 1; 
            GlobalValues.startFloorBlock = 2;
        }
        if (index == 12)//FF HALL NORTH
        {
            GlobalValues.myStartPosString = "FF HALL NORTH";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 1;
            GlobalValues.startFloorBlock = 2;
        }


        //Second Floor
        if (index == 13)//SF Hall North
        {
            GlobalValues.myStartPosString = "SF Hall North";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 2;
            GlobalValues.startFloorBlock = 2;
        }
        if (index == 14)//SF Hall South
        {
            GlobalValues.myStartPosString = "SF Hall South";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 2;
            GlobalValues.startFloorBlock = 2;
        }
        if (index == 15)//SF 236 Waiting Area
        {
            GlobalValues.myStartPosString = "SF 236 Waiting Area";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 2;
            GlobalValues.startFloorBlock = 4;
        }
        if (index == 16)//SF 299C Staircase West
        {
            GlobalValues.myStartPosString = "SF 299C Staircase West";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 2;
            GlobalValues.startFloorBlock = 1;
        }
        if (index == 17)//SF 299D Staircase East
        {
            GlobalValues.myStartPosString = "SF 299D Staircase East";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 2;
            GlobalValues.startFloorBlock = 4;
        }
        if (index == 18)//SF 299E Lift Main
        {
            GlobalValues.myStartPosString = "SF 299E Lift Main";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 2;
            GlobalValues.startFloorBlock = 4;
        }
        if (index == 19)//SF 299H Passage South
        {
            GlobalValues.myStartPosString = "SF 299H Passage South";
            GlobalValues.newPath = true;
            GlobalValues.startFloorLvl = 2;
            GlobalValues.startFloorBlock = 3;
        }

    }
}
