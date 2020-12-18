using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class pedo : MonoBehaviour
{
	public Text acc;

	public GameObject User;


	public float loLim = 0.005f; // level to fall to the low state 
	public float hiLim = 0.3f; // level to go to high state (and detect step) 
	public int steps = 0; // step counter - counts when comp state goes high private 
	bool stateH = false; // comparator state

	public float fHigh = 10.0f; // noise filter control - reduces frequencies above fHigh private 
	public float curAcc = 0f; // noise filter 
	public float fLow = 0.1f; // average gravity filter control - time constant about 1/fLow 
	float avgAcc = 0f;

	///public int wait_time = 30;
	private int old_steps;
	private int steps_taken = 0;



	// Start is called before the first frame update
	void Awake()
    {

		avgAcc = Input.acceleration.magnitude; // initialize avg filter
		//old_steps = steps;
	}

    private void Start()
    {
		Input.compass.enabled = true;
		//Input.acceleration.enabled = true;

	}

    // Update is called once per frame
    void Update()
	{
		User.transform.rotation = Quaternion.Euler(0, Input.compass.magneticHeading, 0);

		if (steps != old_steps)
        {
			User.transform.position += User.transform.forward*6;
			steps_taken += 1;
		}
			
		old_steps = steps;

		
	}

	void FixedUpdate()
	{ // filter input.acceleration using Lerp
		curAcc = Mathf.Lerp(curAcc, Input.acceleration.sqrMagnitude, Time.deltaTime * fHigh);
		avgAcc = Mathf.Lerp(avgAcc, Input.acceleration.sqrMagnitude, Time.deltaTime * fLow);
		float delta = curAcc - avgAcc; // gets the acceleration pulses
		if (!stateH)
		{ // if state == low...
			if (delta > hiLim)
			{ // only goes high if input > hiLim
				stateH = true;
				steps++; // count step when comp goes high 
				
			}
		}
		else
		{
			if (delta < loLim)
			{ // only goes low if input < loLim 
				stateH = false;
			}
		}

		acc.text = "pedo steps:" + steps + "\n steps_taken: " + steps_taken + "\n CurAcc: " + curAcc + "\n avgAcc: " + avgAcc + "\n delta: " + delta;

	}




}
