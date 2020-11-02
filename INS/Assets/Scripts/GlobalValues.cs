using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalValues
{
    public static InputSearch inputSearch;
    public static DropdownQR dropQrScript;

    public static QR_Scanner_Script qrScript;

    public static bool newPath;

    public static string myStartPosString;
    public static string myDestPosString;

    public static Pathfinding pathScript;

    //public static int floorLvl;
    public static int startFloorLvl;        //0=Ground Floor, 1=First Floor
    public static int destFloorLvl;
    public static int oldFloorLvl;

    public static bool stairsReached;

    //public static DropdownQR 
}
