using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Load_Startpos : MonoBehaviour
{
    public Text startPosTxt;


    // Start is called before the first frame update
    void Start()
    {
        startPosTxt.text = "Starting Location: " + GlobalValues.myStartPosString;
    }

}
