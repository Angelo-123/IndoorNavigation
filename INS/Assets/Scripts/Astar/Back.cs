using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Back : MonoBehaviour
{
    #region UI Buttons

    public void OnBackClick()
    {
        GlobalValues.stairsReached = false;
        SceneManager.LoadScene("UI_First");
    }

    #endregion
}
