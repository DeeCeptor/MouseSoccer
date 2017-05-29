using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSettings : MonoBehaviour 
{
	void Start () 
	{
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
        Debug.Log("Setting frame rate to: " + Application.targetFrameRate + ", and vSync to: " + QualitySettings.vSyncCount);
    }

    void Update()
    {
        //Debug.Log(Application.targetFrameRate + " " + QualitySettings.vSyncCount);
    }
}
