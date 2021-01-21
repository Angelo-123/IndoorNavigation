using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowGyro : MonoBehaviour
{
    [Header("Tweaks")]
    [SerializeField] private Quaternion baseRotation = new Quaternion(0, 0, 1, 0);
    private Quaternion rotation;
    private Quaternion correctedRot;
    private float yRot;
    private Vector3 euler;
    private float rot;
    private float startRot = 180;

    public Text rotationText;
    public GameObject User;
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
    private void Start()
    {
        GyroManager.Instance.EnableGyro();

        StartCoroutine(LateStart((float)0.1));
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
        //correctedRot = new Quaternion(rotation.x, rotation.y, -rotation.z, -rotation.w);
        //yRot = baseRotation.y;
        //euler = rotation.eulerAngles;
        //print("euler: " + euler);

        //transform.localRotation = rotation * baseRotation;
        //rot = -correctedRot.eulerAngles.z;
        //MAF(rot);
        MAF(-rotation.eulerAngles.z);

        User.transform.rotation = Quaternion.Euler(0, newRotDeg + startRot, 0);


        //rot = -euler.z;
        //transform.localRotation = Quaternion.Euler(0, rot, 0);
        //print("gyro z: " + -euler.z);
        rotationText.text = "gyroRotation: " + (-rotation.eulerAngles.z + 360) + "°";

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
