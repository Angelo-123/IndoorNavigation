using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class StaircaseScript : MonoBehaviour
{
    public void onClick()
    {
        GlobalValues.stairsReached = true;

        if((GlobalValues.startFloorLvl == 0) && (GlobalValues.destFloorLvl == 1))
        {
            GlobalValues.startFloorLvl = 1;
            GlobalValues.oldFloorLvl = 0;
            GlobalValues.newPath = true;
            SceneManager.LoadScene("FirstFloor");
        }

        if ((GlobalValues.startFloorLvl == 1) && (GlobalValues.destFloorLvl == 0))
        {
            GlobalValues.startFloorLvl = 0;
            GlobalValues.oldFloorLvl = 1;
            GlobalValues.newPath = true;
            SceneManager.LoadScene("AstarProject");
        }


    }
}
