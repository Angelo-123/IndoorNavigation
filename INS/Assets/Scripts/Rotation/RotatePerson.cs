using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotatePerson : MonoBehaviour
{
    public Text rotationText;
    public GameObject User;
    public GameObject User2;
    public Image compassImg;
    public Image compassImg2;
    public Vector3 StartVec;


    float oldRot;
    float currentRotDeg, currentRotRad, currentRotSin, currentRotCos;
    float newRotation, newRotDeg;

    
    private Queue<float> Sin_samples = new Queue<float>();
    private Queue<float> Cos_samples = new Queue<float>();
    private int windowSize = 25;
    private float Sin_sampleAccumulator, Cos_sampleAccumulator;
    public float SinAverage { get; private set; }
    public float CosAverage { get; private set; }


    // Start is called before the first frame update
    void Start()
    {       
        Input.gyro.enabled = true;
        Input.compass.enabled = true;

        StartCoroutine(LateStart((float)0.1));

        oldRot = Input.compass.magneticHeading;
    }
    
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        StartVec = GlobalValues.pathScript.StartVec;
        User.transform.position = StartVec;
        User2.transform.position = StartVec;
    }


        // Update is called once per frame
        void Update()
    {

        currentRotDeg = Input.compass.magneticHeading;          
        currentRotRad = currentRotDeg * (Mathf.PI * 2) / 360;
        currentRotSin = Mathf.Sin(currentRotRad);
        currentRotCos = Mathf.Cos(currentRotRad);


        Sin_sampleAccumulator += currentRotSin;
        Sin_samples.Enqueue(currentRotSin);

        Cos_sampleAccumulator += currentRotCos;
        Cos_samples.Enqueue(currentRotCos);

        if ((Sin_samples.Count > windowSize) || (Cos_samples.Count > windowSize))
        {
            Sin_sampleAccumulator -= Sin_samples.Dequeue();
            Cos_sampleAccumulator -= Cos_samples.Dequeue();
        }

        SinAverage = Sin_sampleAccumulator / Sin_samples.Count;
        CosAverage = Cos_sampleAccumulator / Cos_samples.Count;

        newRotation = Mathf.Atan2(SinAverage, CosAverage);


        newRotDeg = newRotation * 360 / (Mathf.PI * 2);
        if (newRotDeg < 0)
        {
            newRotDeg += 360;
        }


        User.transform.rotation = Quaternion.Euler(0, newRotDeg, 0);
        User2.transform.rotation = Quaternion.Euler(0, newRotDeg, 0);
        compassImg.transform.rotation = Quaternion.Euler(0, 0, newRotDeg);
        compassImg2.transform.rotation = Quaternion.Euler(0, 0, Input.compass.magneticHeading);

        currentRotDeg = Mathf.RoundToInt(currentRotDeg);
        newRotDeg = Mathf.RoundToInt(newRotDeg);

        rotationText.text = "current: " + currentRotDeg + "°\nnew: " + newRotDeg + "°" + "°\nnow: " + Input.compass.magneticHeading + "°";


        oldRot = newRotDeg;
    }

}
