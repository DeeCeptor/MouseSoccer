using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public enum Team {  Blue, Red }

public class ScoreManager : NetworkBehaviour 
{
    public static ScoreManager score_manager;
    public Text score_text;

    [SyncVar]
    public int blue_score;
    [SyncVar]
    public int red_score;


	void Awake () 
	{
        score_manager = this;
	}


    public void BlueScored(int amount)
    {
        if (!isServer)
            return;

        blue_score += amount;
        Debug.Log("Blue scored " + amount);
    }
    public void RedScored(int amount)
    {
        if (!isServer)
            return;

        red_score += amount;
        Debug.Log("Red scored " + amount);
    }


    void Update()
    {
        score_text.text = "Score: <color=blue>" + blue_score + "</color> : <color=red>" + red_score + "</color>";
    }
}
