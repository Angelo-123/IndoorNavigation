using BarcodeScanner;
using BarcodeScanner.Scanner;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QR_Scanner_Script : MonoBehaviour
{
	private IScanner BarcodeScanner;
	public Text TextHeader;
	public RawImage Image;
	public AudioSource Audio;
	private float RestartTime;

	//public Vector3 StartPos;

	//public bool newQr;
	//private string tempQrValue;

	void Awake()
	{
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;

		DontDestroyOnLoad(transform.gameObject);
	}

	void Start()
	{
		//StartPos = Vector3.zero;
		// Create a basic scanner
		BarcodeScanner = new Scanner();
		BarcodeScanner.Camera.Play();

		// Display the camera texture through a RawImage
		BarcodeScanner.OnReady += (sender, arg) => {
			// Set Orientation & Texture
			Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
			Image.transform.localScale = BarcodeScanner.Camera.GetScale();
			Image.texture = BarcodeScanner.Camera.Texture;

			// Keep Image Aspect Ratio
			var rect = Image.GetComponent<RectTransform>();
			var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
			rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);

			RestartTime = Time.realtimeSinceStartup;
		};
	}

	private void StartScanner()
	{
		BarcodeScanner.Scan((barCodeType, barCodeValue) => {
			BarcodeScanner.Stop();
			if (TextHeader.text.Length > 250)
			{
				TextHeader.text = "";
			}
			TextHeader.text += "Found: " + barCodeType + " / " + barCodeValue + "\n";
			RestartTime += Time.realtimeSinceStartup + 1f;

			// Feedback
			Audio.Play();

			#if UNITY_ANDROID || UNITY_IOS
			Handheld.Vibrate();
			#endif

			//tempQrValue = barCodeValue;

			UpdateStartPos(barCodeValue);


		});

		
	}

	void Update()
	{
		if (BarcodeScanner != null)
		{
			BarcodeScanner.Update();
		}

		// Check if the Scanner need to be started or restarted
		if (RestartTime != 0 && RestartTime < Time.realtimeSinceStartup)
		{
			StartScanner();
			RestartTime = 0;
		}
	}

	#region UI Buttons

	public void ClickBack()
	{
		/*
		GlobalValues.qrScript = GameObject.Find("Main_Camera_QR").GetComponent<QR_Scanner_Script>();
		GlobalValues.qrScript.StartPos = StartPos;
		*/

		// Try to stop the camera before loading another scene
		StartCoroutine(StopCamera(() => {	
			SceneManager.LoadScene("UI_First");
		}));
	}

	/// <summary>
	/// This coroutine is used because of a bug with unity (http://forum.unity3d.com/threads/closing-scene-with-active-webcamtexture-crashes-on-android-solved.363566/)
	/// Trying to stop the camera in OnDestroy provoke random crash on Android
	/// </summary>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator StopCamera(Action callback)
	{
		// Stop Scanning
		Image = null;
		BarcodeScanner.Destroy();
		BarcodeScanner = null;

		// Wait a bit
		yield return new WaitForSeconds(0.1f);

		callback.Invoke();
	}

	#endregion


	
	private void UpdateStartPos(string ScanValue)
    {
		if (TextHeader.text.Length > 250)
		{
			TextHeader.text = "";
		}


		//Ground Floor
		if (ScanValue == "Entrance East N1A-GF-G99K")//Entrance East
		{
			//StartPos = new Vector3(-31.0f, 0.0f, 4.0f);
			GlobalValues.myStartPosString = "Entrance East N1A-GF-G99K";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 0;
			GlobalValues.startFloorBlock = 4;
		}
		if (ScanValue == "Entrance South N1A-GF")//Entrance South
		{
			//StartPos = new Vector3(-2.0f, 0.0f, 16.0f);
			GlobalValues.myStartPosString = "Entrance South N1A-GF";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 0;
			GlobalValues.startFloorBlock = 3;
		}
		if (ScanValue == "Lift Main N1A-GF-G99F")//Lift Main
		{
			//StartPos = new Vector3(-15.0f, 0.0f, 3.0f);
			GlobalValues.myStartPosString = "Lift Main N1A-GF-G99F";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 0;
			GlobalValues.startFloorBlock = 4;
		}
		if (ScanValue == "Staircase East N1A-GF-G99G")//Staircase East
		{
			//StartPos = new Vector3(-8.5f, 0.0f, 4.0f);
			GlobalValues.myStartPosString = "Staircase East N1A-GF-G99G";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 0;
			GlobalValues.startFloorBlock = 4;
		}
		if (ScanValue == "Staircase West N1A-GF-G99H")//Staircase West
		{
			//StartPos = new Vector3(26.0f, 0.0f, 3.0f);
			GlobalValues.myStartPosString = "Staircase West N1A-GF-G99H";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 0;
			GlobalValues.startFloorBlock = 1;
		}
		if (ScanValue == "Waiting Area N1A-GF-G42")//Waiting Area
		{
			//StartPos = new Vector3(-16.0f, 0.0f, -5.0f);
			GlobalValues.myStartPosString = "Waiting Area N1A-GF-G42";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 0;
			GlobalValues.startFloorBlock = 4;
		}
		if (ScanValue == "Office Passage N1A-GF-G99I")//Office Passage
		{
			//StartPos = new Vector3(19.4f, 0.0f, -2.3f);
			GlobalValues.myStartPosString = "Office Passage N1A-GF-G99I";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 0;
			GlobalValues.startFloorBlock = 2;
		}

		//First Floor
		if (ScanValue == "FF 150 WAITING AREA")//WAITING AREA
		{
			GlobalValues.myStartPosString = "FF 150 WAITING AREA";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 1;
			GlobalValues.startFloorBlock = 4;
		}
		if (ScanValue == "FF 199G STAIRS EAST")//STAIRS EAST
		{
			GlobalValues.myStartPosString = "FF 199G STAIRS EAST";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 1;
			GlobalValues.startFloorBlock = 4;
		}
		if (ScanValue == "FF 199H STAIRS WEST")//STAIRS WEST
		{
			GlobalValues.myStartPosString = "FF 199H STAIRS WEST";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 1;
			GlobalValues.startFloorBlock = 1;
		}
		if (ScanValue == "FF ENTRANCE SOUTH")//ENTRANCE SOUTH
		{
			GlobalValues.myStartPosString = "FF ENTRANCE SOUTH";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 1;
			GlobalValues.startFloorBlock = 3;
		}
		if (ScanValue == "FF HALL SOUTH")//Hall Passage
		{
			GlobalValues.myStartPosString = "FF HALL SOUTH";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 1;
			GlobalValues.startFloorBlock = 2;
		}
		if (ScanValue == "FF HALL NORTH")//Hall Passage
		{
			GlobalValues.myStartPosString = "FF HALL NORTH";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 1;
			GlobalValues.startFloorBlock = 2;
		}


		//Second Floor
		if (ScanValue == "SF Hall North")//SF Hall North
		{
			GlobalValues.myStartPosString = "SF Hall North";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 2;
			GlobalValues.startFloorBlock = 2;
		}
		if (ScanValue == "SF Hall South")//SF Hall South
		{
			GlobalValues.myStartPosString = "SF Hall South";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 2;
			GlobalValues.startFloorBlock = 2;
		}
		if (ScanValue == "SF 236 Waiting Area")//SF 236 Waiting Area
		{
			GlobalValues.myStartPosString = "SF 236 Waiting Area";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 2;
			GlobalValues.startFloorBlock = 4;
		}
		if(ScanValue == "SF 299C Staircase West")//SF 299C Staircase West
		{
			GlobalValues.myStartPosString = "SF 299C Staircase West";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 2;
			GlobalValues.startFloorBlock = 1;
		}
		if (ScanValue == "SF 299D Staircase East")//SF 299D Staircase East
		{
			GlobalValues.myStartPosString = "SF 299D Staircase East";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 2;
			GlobalValues.startFloorBlock = 4;
		}
		if (ScanValue == "SF 299E Lift Main")//SF 299E Lift Main
		{
			GlobalValues.myStartPosString = "SF 299E Lift Main";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 2;
			GlobalValues.startFloorBlock = 4;
		}
		if (ScanValue == "SF 299H Passage South")//SF 299H Passage South
		{
			GlobalValues.myStartPosString = "SF 299H Passage South";
			GlobalValues.newPath = true;
			GlobalValues.startFloorLvl = 2;
			GlobalValues.startFloorBlock = 3;
		}
		TextHeader.text += "StartPos: " + GlobalValues.myStartPosString + "\n";

	}
}
