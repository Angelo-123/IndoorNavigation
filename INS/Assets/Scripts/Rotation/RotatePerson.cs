﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotatePerson : MonoBehaviour
{
    public Text rotationText;
    public GameObject User;
    public Image compassImg;
    public Vector3 StartVec;


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
        Input.compass.enabled = true;
        StartCoroutine(LateStart((float)0.1));
    }
    
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        StartVec = GlobalValues.pathScript.StartVec;
        User.transform.position = StartVec;
    }


    // Update is called once per frame
    void Update()
    {
        MAF(Input.compass.magneticHeading);


        User.transform.rotation = Quaternion.Euler(0, newRotDeg + GlobalValues.startRot, 0);

        compassImg.transform.rotation = Quaternion.Euler(0, 0, newRotDeg);

        currentRotDeg = Mathf.RoundToInt(currentRotDeg);
        newRotDeg = Mathf.RoundToInt(newRotDeg);

    }


    private float MAF(float currentRotDeg)
    {
        //currentRotDeg = Input.compass.magneticHeading;
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


        return newRotDeg;
    }

}
