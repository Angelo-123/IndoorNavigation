using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWorld : MonoBehaviour
{
    public GameObject World;


    // Start is called before the first frame update
    void Start()
    {
        Input.compass.enabled = true;
        // Orient an object to point to magnetic north.
        World.transform.rotation = Quaternion.Euler(0, -Input.compass.magneticHeading+180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
