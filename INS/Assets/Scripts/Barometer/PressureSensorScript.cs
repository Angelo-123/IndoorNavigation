using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PressureSensorScript : MonoBehaviour
{
    public GameObject Panel1;
    public Text popupText;

    public Text pressureText;
    private float pressureStart;
    private float pressureCurrent;
    private int startvalue;
    private bool levelUp;

    // Start is called before the first frame update
    void Start()
    {
        InputSystem.EnableDevice(PressureSensor.current);
        startvalue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(startvalue <= 5)
        {
            pressureStart = PressureSensor.current.atmosphericPressure.ReadValue();
            startvalue +=1;
        }
        


        pressureCurrent = PressureSensor.current.atmosphericPressure.ReadValue();
        //pressureText.text = PressureSensor.current.atmosphericPressure.ReadValue().ToString() + "hPa";
        //pressureText.text = pressureCurrent.ToString() + "hPa" + "\n Starting Pressure: " + pressureStart.ToString();

        
        if ((pressureStart-pressureCurrent) > 0.2)
        {     
            if (Panel1 != null)
            {
                Panel1.SetActive(true);
                popupText.text = "Going up?" + "\n Please only press the Change Floor Button upon reaching the next Floor Level";
                levelUp = true;
            }
        }
        else if((pressureCurrent-pressureStart) > 0.2)
        {
            if (Panel1 != null)
            {
                Panel1.SetActive(true);
                popupText.text = "Going down?" + "\n Please only press the Change Floor Button upon reaching the lower Floor Level";
                levelUp = false;
            }
        }
        else
        {
            pressureText.text = pressureCurrent.ToString() + "hPa" + "\n Starting Pressure: " + pressureStart;
        }

    }

    public void onClickCancel()
    {
        if(Panel1 != null)
        {
            Panel1.SetActive(false);
        }
    }

    public void onClickGo()
    {
        GlobalValues.stairsReached = true;

        if (levelUp)
        {
            if ((GlobalValues.startFloorLvl == 0) && (GlobalValues.destFloorLvl == 1))
            {
                GlobalValues.startFloorLvl = 1;
                GlobalValues.oldFloorLvl = 0;
                GlobalValues.newPath = true;
                SceneManager.LoadScene("FirstFloor");
            }
        }
        else
        {
            if ((GlobalValues.startFloorLvl == 0) && (GlobalValues.destFloorLvl == 1))
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


}
