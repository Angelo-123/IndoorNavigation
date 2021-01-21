using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Path_Loader : MonoBehaviour
{
     #region UI Buttons

    public void OnGoClick()
    { 
        /*
        if (GlobalValues.dropQrScript == null)
        {
            GlobalValues.dropQrScript = GameObject.Find("DropdownStart").GetComponent<DropdownQR>();
        }*/

        if (GlobalValues.inputSearch == null)
        {
            GlobalValues.inputSearch = GameObject.Find("TextSearch").GetComponent<InputSearch>();
        }


        if (GlobalValues.startFloorLvl == 0)
        {
            SceneManager.LoadScene("AStarProject");
        }
        else if (GlobalValues.startFloorLvl == 1)
        {
            SceneManager.LoadScene("FirstFloor");
        }
        else if (GlobalValues.startFloorLvl == 2)
        {
            SceneManager.LoadScene("SecondFloor");
        }


        else if (GlobalValues.startFloorLvl == 5)
        {
            SceneManager.LoadScene("HomeFloor");
        }

    }

    #endregion


}
