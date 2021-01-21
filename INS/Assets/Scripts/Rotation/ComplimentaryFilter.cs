using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComplimentaryFilter : MonoBehaviour
{
    [Header("Tweaks")]
    private Quaternion rotation;
    private float gyroRot;
    private float startRot = 180;

    public Text rotationText;
    public GameObject User;
    public Vector3 StartVec;

    float currentRotRad, currentRotSin, currentRotCos;
    float newRotation, newRotDeg;

    private Queue<float> Sin_samples = new Queue<float>();
    private Queue<float> Cos_samples = new Queue<float>();
    private int windowSize = 25;
    private float Sin_sampleAccumulator, Cos_sampleAccumulator;
    public float SinAverage { get; private set; }
    public float CosAverage { get; private set; }


    float compassMeasure, gyroMeasure, output, newGyro, filteredOutput;


    // Start is called before the first frame update
    private void Start()
    {
        Input.compass.enabled = true;
        GyroManager.Instance.EnableGyro();

        StartCoroutine(LateStart((float)0.1));


        //output = (float)(0.02 * Input.compass.magneticHeading + 0.98 * GyroManager.Instance.GetGyroRotation().eulerAngles.z);
        output = Input.compass.magneticHeading;
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        StartVec = GlobalValues.pathScript.StartVec;
        User.transform.position = StartVec;
    }

    // Update is called once per frame
    private void Update()
    {
        rotation = GyroManager.Instance.GetGyroRotation();
        gyroRot = -rotation.eulerAngles.z;
        while (gyroRot < 0)
        {
            gyroRot += 360;
        }

        compassMeasure = Input.compass.magneticHeading;
        gyroMeasure = gyroRot;

        newGyro = (float)0.5 * (gyroMeasure + output);

        output = (float)(0.02 * compassMeasure) + (float)(0.98 * newGyro);

        filteredOutput = MAF(output);


        User.transform.rotation = Quaternion.Euler(0, filteredOutput + startRot, 0);


        //rotationText.text = "gyroRotation: " + (-rotation.eulerAngles.z + 360) + "°";
    }

    private float MAF(float currentRotDeg)
    {
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
