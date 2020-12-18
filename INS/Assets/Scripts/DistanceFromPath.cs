using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class DistanceFromPath : MonoBehaviour
{
    public GameObject User;
    public float xdist, zdist, diagDist, minDist;
    public Text distx;
    public GameObject Panel2;


    // Start is called before the first frame update
    void Start()
    {
        Panel2.SetActive(false);
        //InvokeRepeating("CalcDist", 3.0f, 1.0f);
        InvokeRepeating(nameof(CalcDist), 3.0f, 1.0f);
    }

    void CalcDist()
    {
        List<float> DiagDistArray = new List<float>();
        for (int i = 0; i < GlobalValues.xPosArray.Count; i++)
        {
            xdist = Mathf.Abs(User.transform.position.x - GlobalValues.xPosArray[i]);
            zdist = Mathf.Abs(User.transform.position.z - GlobalValues.zPosArray[i]);
            diagDist = Mathf.Sqrt(xdist * xdist + zdist * zdist);
            DiagDistArray.Add(diagDist);
        }

        minDist = DiagDistArray.Min() / 10;
        distx.text = "Min Distance: " + minDist;

        if (minDist > 5)
        {
            if (Panel2 != null)
            {
                Panel2.SetActive(true);
            }
        }
    }

    public void OnUpdateClick()
    {
        SceneManager.LoadScene("QR_Scanner");
        GlobalValues.PathUpdate = true;
    }
}
