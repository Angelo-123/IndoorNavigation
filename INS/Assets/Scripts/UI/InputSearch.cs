using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputSearch : MonoBehaviour
{
    public Vector3 DestSearchPos;


    //public Text displaytext;
    public Text inputText;
    public Dropdown dropdown;

    List<string> items = new List<string>();
    string myText = "";


    private void Start()
    {
        dropdown.options.Clear();

        items.Add("G01 - Meeting Room");
        items.Add("G02 - Meeting Room");
        items.Add("G03 - Meeting Room");
        items.Add("G04 - Koffee Station");
        items.Add("G05 - Kitchen");

        items.Add("G98I - Electrical");
        items.Add("G98H - Data");
        items.Add("G99F - Lift");
        items.Add("G99J - Lift");

        items.Add("G21 - Computer Lab");
        items.Add("G22 - Computer Lab");
        items.Add("G23 - Storage");
        items.Add("G24 - Ladies Bathroom");
        items.Add("G25 - Gents Bathroom");
        items.Add("G26 - Storage");
        items.Add("G27 - Conference Room");
        items.Add("G28 - Office");
        items.Add("G29 - Office");
        items.Add("G30 - Office");
        items.Add("G31 - Office");
        items.Add("G32 - Office");
        items.Add("G32A - Office");
        items.Add("G32B - Office");
        items.Add("G33 - Office");
        items.Add("G34 - Office");
        items.Add("G35 - Office");
        items.Add("G36 - Office");
        items.Add("G37 - Storage");
        items.Add("G38 - Storage");
        items.Add("G39 - Office");
        items.Add("G40 - Office");
        items.Add("G41 - Office");
        items.Add("G42 - Waiting Area");
        items.Add("G43 - Office");
        items.Add("G44 - Office");
        items.Add("G45 - Kitchen");
        items.Add("G46 - Computer Lab");

        //FirstFloor
        items.Add("FF 101 OFFICE");
        items.Add("FF 102 OFFICE");
        items.Add("FF 103 OFFICE");
        items.Add("FF 104 OFFICE");
        items.Add("FF 105 PROJECT LAB");
        items.Add("FF 106 PROJECT LAB");
        items.Add("FF 107 COFFEE STATION");
        items.Add("FF 108 STORAGE");
        items.Add("FF 119J LIFT");
        items.Add("FF 198G DATA");
        items.Add("FF 198H ELECTRICAL");
        items.Add("FF 123 SEMINAR");
        items.Add("FF 124 PROJECT LAB");
        items.Add("FF 126 PROJECT LAB");
        items.Add("FF 128 PROJECT LAB");
        items.Add("FF 129 STORAGE");
        items.Add("FF 130 LADIES");
        items.Add("FF 131 GENTS");
        items.Add("FF 132 SHOWER");
        items.Add("FF 133 OFFICE");
        items.Add("FF 134 OFFICE");
        items.Add("FF 135 OFFICE");
        items.Add("FF 136 OFFICE");
        items.Add("FF 137 OFFICE");
        items.Add("FF 138 OFFICE");
        items.Add("FF 139 OFFICE");
        items.Add("FF 140 OFFICE");
        items.Add("FF 141 OFFICE");
        items.Add("FF 142 OFFICE");
        items.Add("FF 143 OFFICE");
        items.Add("FF 144 STORAGE");
        items.Add("FF 145 STORAGE");
        items.Add("FF 146 STORAGE");
        items.Add("FF 147 OFFICE");
        items.Add("FF 148 OFFICE");
        items.Add("FF 149 OFFICE");
        items.Add("FF 150 WAITING AREA");
        items.Add("FF 151 OFFICE");
        items.Add("FF 152 OFFICE");
        items.Add("FF 153 KITCHEN");
        items.Add("FF 154 OFFICE");
        items.Add("FF 155 OFFICE");
        items.Add("FF 156 OFFICE");
        items.Add("FF 157 OFFICE");
        items.Add("FF 158 OFFICE");


        //SecondFloor
        items.Add("SF 201 - Office");
        items.Add("SF 202 - Office");
        items.Add("SF 203 - Office");
        items.Add("SF 204 - Office");
        items.Add("SF 205 - Project Lab");
        items.Add("SF 206 - Project Lab");
        items.Add("SF 207 - Koffee Station");
        items.Add("SF 208 - Kitchen");
        items.Add("SF 299E - Lift");
        items.Add("SF 299G - Lift");
        items.Add("SF 298A - Data");
        items.Add("SF 298B - Electrical");
        items.Add("SF 209 - Project Lab");
        items.Add("SF 210 - Project Lab");
        items.Add("SF 211 - Project Lab");
        items.Add("SF 212 - Project Lab");
        items.Add("SF 213 - Project Lab");
        items.Add("SF 214 - Project Lab");
        items.Add("SF 215 - Storage");
        items.Add("SF 216 - Ladies Bathroom");
        items.Add("SF 217 - Gents Bathroom");
        items.Add("SF 218 - Shower");
        items.Add("SF 219 - Conference Room");
        items.Add("SF 220 - Office");
        items.Add("SF 221 - Office");
        items.Add("SF 222 - Office");
        items.Add("SF 223 - Office");
        items.Add("SF 224 - Office");
        items.Add("SF 225 - Office");
        items.Add("SF 226 - Office");
        items.Add("SF 227 - Office");
        items.Add("SF 228 - Office");
        items.Add("SF 229 - Office");
        items.Add("SF 230 - Copy Room");
        items.Add("SF 231 - Storage");
        items.Add("SF 233 - Office");
        items.Add("SF 234 - Conference");
        items.Add("SF 235 - Office");
        items.Add("SF 236 - Waiting Area");
        items.Add("SF 237 - Office");
        items.Add("SF 238 - Office");
        items.Add("SF 239 - Kitchen");
        items.Add("SF 240 - Office");
        items.Add("SF 241 - Office");
        items.Add("SF 242 - Office");
        items.Add("SF 243 - Office");
        items.Add("SF 244 - Office");



        foreach (var item in items)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = item });
        }

        DropdownItemSelected(dropdown);

        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
    }

    // Update is called once per frame
    public void inputUpdate()
    {
        if (!string.IsNullOrEmpty(inputText.text))
        {
            dropdown.options.Clear();
            string oldString = myText;
            myText = inputText.text;

            if (!string.IsNullOrEmpty(myText) && myText.Length > oldString.Length)
            {
                List<string> itemsfound = items.FindAll(w => w.ToLower().Trim().Contains(myText.ToLower().Trim()));
                if (itemsfound.Count > 0)
                {
                    
                    foreach (var item in itemsfound)
                    {
                        dropdown.options.Add(new Dropdown.OptionData() { text = item });
                    }
                    print(itemsfound.Count);
                }
            }
        }
        else
        {
            foreach (var item in items)
            {
                dropdown.options.Add(new Dropdown.OptionData() { text = item });
            }
            print(items.Count);
        }
    }



    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        print("index: " + index);
        print("selected: " + dropdown.captionText.text);
        //TextBox.text = dropdown.options[index].text;
        UpdateDest(index);
    }

    private void UpdateDest(int index)
    {
        //Ground Floor
        if (dropdown.captionText.text == "G01 - Meeting Room")//G01 - Meeting Room
        {
            GlobalValues.myDestPosString = "G01 - Meeting Room";
            GlobalValues.destFloorLvl = 0;
        }
        if (dropdown.captionText.text == "G02 - Meeting Room")//G02 - Meeting Room
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G02 - Meeting Room";
        }
        if (dropdown.captionText.text == "G03 - Meeting Room")//G03 - Meeting Room
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G03 - Meeting Room";
        }
        if (dropdown.captionText.text == "G04 - Koffee Station")//G04 - Koffee Station
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G04 - Koffee Station";
        }
        if (dropdown.captionText.text == "G05 - Kitchen")//G05 - Kitchen
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G05 - Kitchen";
        }

        if (dropdown.captionText.text == "G98I - Electrical")//G98I - Electrical
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G98I - Electrical";
        }
        if (dropdown.captionText.text == "G98H - Data")//G98H - Data
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G98H - Data";
        }
        if (dropdown.captionText.text == "G99F - Lift")//G99F - Lift
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G99F - Lift";
        }
        if (dropdown.captionText.text == "G99J - Lift")//G99J - Lift
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G99J - Lift";
        }

        if (dropdown.captionText.text == "G21 - Computer Lab")//G21 - Computer Lab
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G21 - Computer Lab";
        }
        if (dropdown.captionText.text == "G22 - Computer Lab")//G22 - Computer Lab
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G22 - Computer Lab";
        }
        if (dropdown.captionText.text == "G23 - Storage")//G23 - Storage
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G23 - Storage";
        }
        if (dropdown.captionText.text == "G24 - Ladies Bathroom")//G24 - Ladies Bathroom
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G24 - Ladies Bathroom";
        }
        if (dropdown.captionText.text == "G25 - Gents Bathroom")//G25 - Gents Bathroom
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G25 - Gents Bathroom";
        }
        if (dropdown.captionText.text == "G26 - Storage")//G26 - Storage
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G26 - Storage";
        }
        if (dropdown.captionText.text == "G27 - Conference Room")//G27 - Conference Room
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G27 - Conference Room";
        }
        if (dropdown.captionText.text == "G28 - Office")//G28 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G28 - Office";
        }
        if (dropdown.captionText.text == "G29 - Office")//G29 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G29 - Office";
        }
        if (dropdown.captionText.text == "G30 - Office")//G30 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G30 - Office";
        }
        if (dropdown.captionText.text == "G31 - Office")//G31 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G31 - Office";
        }
        if (dropdown.captionText.text == "G32 - Office")//G32 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G32 - Office";
        }
        if (dropdown.captionText.text == "G32A - Office")//G32A - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G32A - Office";
        }
        if (dropdown.captionText.text == "G32B - Office")//G32B - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G32B - Office";
        }
        if (dropdown.captionText.text == "G33 - Office")//G33 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G33 - Office";
        }
        if (dropdown.captionText.text == "G34 - Office")//G34 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G34 - Office";
        }
        if (dropdown.captionText.text == "G35 - Office")//G35 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G35 - Office";
        }
        if (dropdown.captionText.text == "G36 - Office")//G36 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G36 - Office";
        }
        if (dropdown.captionText.text == "G37 - Storage")//G37 - Storage
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G37 - Storage";
        }
        if (dropdown.captionText.text == "G38 - Storage")//G38 - Storage
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G38 - Storage";
        }
        if (dropdown.captionText.text == "G39 - Office")//G39 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G39 - Office";
        }
        if (dropdown.captionText.text == "G40 - Office")//G40 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G40 - Office";
        }
        if (dropdown.captionText.text == "G41 - Office")//G41 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G41 - Office";
        }
        if (dropdown.captionText.text == "G42 - Waiting Area")//G42 - Waiting Area
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G42 - Waiting Area";
        }
        if (dropdown.captionText.text == "G43 - Office")//G43 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G43 - Office";
        }
        if (dropdown.captionText.text == "G44 - Office")//G44 - Office
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G44 - Office";
        }
        if (dropdown.captionText.text == "G45 - Kitchen")//G45 - Kitchen
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G45 - Kitchen";
        }
        if (dropdown.captionText.text == "G46 - Computer Lab")//G46 - Computer Lab
        {
            GlobalValues.destFloorLvl = 0;
            GlobalValues.myDestPosString = "G46 - Computer Lab";
        }

        //First Floor
        if (dropdown.captionText.text == "FF 101 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 101 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 102 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 102 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 103 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 103 OFFICE"; 
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 104 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 104 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 105 PROJECT LAB")//
        {
            GlobalValues.myDestPosString = "FF 105 PROJECT LAB";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 106 PROJECT LAB")//
        {
            GlobalValues.myDestPosString = "FF 106 PROJECT LAB";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 107 COFFEE STATION")//
        {
            GlobalValues.myDestPosString = "FF 107 COFFEE STATION";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 108 STORAGE")//
        {
            GlobalValues.myDestPosString = "FF 108 STORAGE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 119J LIFT")//
        {
            GlobalValues.myDestPosString = "FF 119J LIFT";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 198G DATA")//
        {
            GlobalValues.myDestPosString = "FF 198G DATA";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 198H ELECTRICAL")//
        {
            GlobalValues.myDestPosString = "FF 198H ELECTRICAL";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 123 SEMINAR")//
        {
            GlobalValues.myDestPosString = "FF 123 SEMINAR";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 124 PROJECT LAB")//
        {
            GlobalValues.myDestPosString = "FF 124 PROJECT LAB";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 126 PROJECT LAB")//
        {
            GlobalValues.myDestPosString = "FF 126 PROJECT LAB";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 128 PROJECT LAB")//
        {
            GlobalValues.myDestPosString = "FF 128 PROJECT LAB";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 129 STORAGE")//
        {
            GlobalValues.myDestPosString = "FF 129 STORAGE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 130 LADIES")//
        {
            GlobalValues.myDestPosString = "FF 130 LADIES";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 131 GENTS")//
        {
            GlobalValues.myDestPosString = "FF 131 GENTS";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 132 SHOWER")//
        {
            GlobalValues.myDestPosString = "FF 132 SHOWER";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 133 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 133 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 134 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 134 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 135 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 135 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 136 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 136 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 137 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 137 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 138 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 138 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 139 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 139 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 140 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 140 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 141 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 141 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 142 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 142 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 143 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 143 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 144 STORAGE")//
        {
            GlobalValues.myDestPosString = "FF 144 STORAGE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 145 STORAGE")//
        {
            GlobalValues.myDestPosString = "FF 145 STORAGE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 146 STORAGE")//
        {
            GlobalValues.myDestPosString = "FF 146 STORAGE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 147 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 147 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 148 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 148 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 149 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 149 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 150 WAITING AREA")//
        {
            GlobalValues.myDestPosString = "FF 150 WAITING AREA";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 151 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 151 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 152 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 152 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 153 KITCHEN")//
        {
            GlobalValues.myDestPosString = "FF 153 KITCHEN";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 154 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 154 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 155 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 155 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 156 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 156 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 157 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 157 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }
        if (dropdown.captionText.text == "FF 158 OFFICE")//
        {
            GlobalValues.myDestPosString = "FF 158 OFFICE";
            GlobalValues.destFloorLvl = 1;
        }




        //Second Floor
        if (dropdown.captionText.text == "SF 201 - Office")//
        {
            GlobalValues.myDestPosString = "SF 201 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 202 - Office")//
        {
            GlobalValues.myDestPosString = "SF 202 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 203 - Office")//
        {
            GlobalValues.myDestPosString = "SF 203 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 204 - Office")//
        {
            GlobalValues.myDestPosString = "SF 204 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 205 - Project Lab")//
        {
            GlobalValues.myDestPosString = "SF 205 - Project Lab";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 206 - Project Lab")//
        {
            GlobalValues.myDestPosString = "SF 206 - Project Lab";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 207 - Koffee Station")//
        {
            GlobalValues.myDestPosString = "SF 207 - Koffee Station";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 208 - Kitchen")//
        {
            GlobalValues.myDestPosString = "SF 208 - Kitchen";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 299E - Lift")//
        {
            GlobalValues.myDestPosString = "SF 299E - Lift";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 299G - Lift")//
        {
            GlobalValues.myDestPosString = "SF 299G - Lift";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 298A - Data")//
        {
            GlobalValues.myDestPosString = "SF 298A - Data";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 298B - Electrical")//
        {
            GlobalValues.myDestPosString = "SF 298B - Electrical";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 209 - Project Lab")//
        {
            GlobalValues.myDestPosString = "SF 209 - Project Lab";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 210 - Project Lab")//
        {
            GlobalValues.myDestPosString = "SF 210 - Project Lab";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 211 - Project Lab")//
        {
            GlobalValues.myDestPosString = "SF 211 - Project Lab";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 212 - Project Lab")//
        {
            GlobalValues.myDestPosString = "SF 212 - Project Lab";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 213 - Project Lab")//
        {
            GlobalValues.myDestPosString = "SF 213 - Project Lab";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 214 - Project Lab")//
        {
            GlobalValues.myDestPosString = "SF 214 - Project Lab";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 215 - Storage")//
        {
            GlobalValues.myDestPosString = "SF 215 - Storage";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 216 - Ladies Bathroom")//
        {
            GlobalValues.myDestPosString = "SF 216 - Ladies Bathroom";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 217 - Gents Bathroom")//
        {
            GlobalValues.myDestPosString = "SF 217 - Gents Bathroom";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 218 - Shower")//
        {
            GlobalValues.myDestPosString = "SF 218 - Shower";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 219 - Conference Room")//
        {
            GlobalValues.myDestPosString = "SF 219 - Conference Room";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 220 - Office")//
        {
            GlobalValues.myDestPosString = "SF 220 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 221 - Office")//
        {
            GlobalValues.myDestPosString = "SF 221 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 222 - Office")//
        {
            GlobalValues.myDestPosString = "SF 222 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 223 - Office")//
        {
            GlobalValues.myDestPosString = "SF 223 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 224 - Office")//
        {
            GlobalValues.myDestPosString = "SF 224 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 225 - Office")//
        {
            GlobalValues.myDestPosString = "SF 225 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 226 - Office")//
        {
            GlobalValues.myDestPosString = "SF 226 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 227 - Office")//
        {
            GlobalValues.myDestPosString = "SF 227 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 228 - Office")//
        {
            GlobalValues.myDestPosString = "SF 228 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 229 - Office")//
        {
            GlobalValues.myDestPosString = "SF 229 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 230 - Copy Room")//
        {
            GlobalValues.myDestPosString = "SF 230 - Copy Room";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 231 - Storage")//
        {
            GlobalValues.myDestPosString = "SF 231 - Storage";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 233 - Office")//
        {
            GlobalValues.myDestPosString = "SF 233 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 234 - Conference")//
        {
            GlobalValues.myDestPosString = "SF 234 - Conference";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 235 - Office")//
        {
            GlobalValues.myDestPosString = "SF 235 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 236 - Waiting Area")//
        {
            GlobalValues.myDestPosString = "SF 236 - Waiting Area";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 237 - Office")//
        {
            GlobalValues.myDestPosString = "SF 237 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 238 - Office")//
        {
            GlobalValues.myDestPosString = "SF 238 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 239 - Kitchen")//
        {
            GlobalValues.myDestPosString = "SF 239 - Kitchen";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 240 - Office")//
        {
            GlobalValues.myDestPosString = "SF 240 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 241 - Office")//
        {
            GlobalValues.myDestPosString = "SF 241 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 242 - Office")//
        {
            GlobalValues.myDestPosString = "SF 242 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 243 - Office")//
        {
            GlobalValues.myDestPosString = "SF 243 - Office";
            GlobalValues.destFloorLvl = 2;
        }
        if (dropdown.captionText.text == "SF 244 - Office")//
        {
            GlobalValues.myDestPosString = "SF 244 - Office";
            GlobalValues.destFloorLvl = 2;
        }


        print("DestSearchPos: " + DestSearchPos);

    }
}