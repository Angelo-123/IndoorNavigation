/* 
*   Pedometer
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace PedometerU.Tests {
    using System.Security.Cryptography.X509Certificates;
    using UnityEngine;
    using UnityEngine.Analytics;
    using UnityEngine.UI;

    public class StepCounter : MonoBehaviour {

        public Text stepText;
        private Pedometer pedometer;
        public GameObject User;

        private void Start () {
            // Create a new pedometer
            pedometer = new Pedometer(OnStep);
            // Reset UI
            OnStep(0, 0);
        }

        private void OnStep (int steps, double distance) {
            // Display the values // Distance in feet
            stepText.text = "Steps: " + steps.ToString();
            //distanceText.text = (distance * 3.28084).ToString("F2") + " ft";
            User.transform.position += User.transform.forward*10;
        }

        private void OnDisable () {
            // Release the pedometer
            pedometer.Dispose();
            pedometer = null;
        }

        public void onClick()
        {
            User.transform.position += User.transform.forward*10;
        }
    }
}