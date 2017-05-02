using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Team {  Blue, Red }

public class ScoreManager : MonoBehaviour 
{
    public static ScoreManager score_manager;
    public Text score_text;
    public int blue_score;
    public int red_score;


	void Awake () 
	{
        score_manager = this;
	}


    public void BlueScored(int amount)
    {
        blue_score += amount;
        Debug.Log("Blue scored " + amount);
    }
    public void RedScored(int amount)
    {
        red_score += amount;
        Debug.Log("Red scored " + amount);
    }


    void Update()
    {
        score_text.text = "Score: <color=blue>" + blue_score + "</color> : <color=red>" + red_score + "</color>";
    }
}
