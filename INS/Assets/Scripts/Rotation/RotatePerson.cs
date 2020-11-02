using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotatePerson : MonoBehaviour
{
    public Text rotationText;
    public GameObject User;
    public Image compassImg;
    public Vector3 StartVec;

    public bool triggered;

    [Range(0.0f, 1.0f)]
    public float weight = 0.5f;

    float oldRotSin, oldRotCos, oldRotDeg, oldRotRad;
    float currentRotDeg, currentRotRad, currentRotSin, currentRotCos;
    float newRotation, newRotDeg;
    bool firstFrame = true;


    // Start is called before the first frame update
    void Start()
    {       
        Input.gyro.enabled = true;
        Input.compass.enabled = true;
        triggered = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(triggered == false)
        {
            StartVec = GlobalValues.pathScript.StartVec;
            User.transform.position = StartVec;
            oldRotDeg = Input.compass.magneticHeading;
            User.transform.rotation = Quaternion.Euler(0, -oldRotDeg, 0);
            triggered = true;
        }


        if (firstFrame)
        {
            oldRotDeg = Input.compass.magneticHeading;
            oldRotRad = oldRotDeg * (Mathf.PI * 2) / 360;
            oldRotSin = Mathf.Sin(oldRotRad);
            oldRotCos = Mathf.Cos(oldRotRad);
            firstFrame = false;
        }
        else
        {
            currentRotDeg = Input.compass.magneticHeading;
            currentRotRad = currentRotDeg * (Mathf.PI * 2) / 360;
            currentRotSin = Mathf.Sin(currentRotRad);
            currentRotCos = Mathf.Cos(currentRotRad);
            newRotation = Mathf.Atan2(oldRotSin + currentRotSin, oldRotCos + currentRotCos);

            oldRotSin = currentRotSin;
            oldRotCos = currentRotCos;
        }


        //newRotation2 = Mathf.Asin(newRotation);

        newRotDeg = newRotation * 360 / (Mathf.PI * 2);
        if(newRotDeg < 0)
        {
            newRotDeg = newRotDeg + 360;
        }

        User.transform.rotation = Quaternion.Euler(0, newRotDeg, 0);
        compassImg.transform.rotation = Quaternion.Euler(0, 0, newRotDeg);

        currentRotDeg = Mathf.RoundToInt(currentRotDeg);
        newRotDeg = Mathf.RoundToInt(newRotDeg);

        rotationText.text = "current: " + currentRotDeg +  "°\nnew: " + newRotDeg + "°";
    }

}
