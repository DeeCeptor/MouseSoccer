using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour 
{
    public static Ball ball;

	void Awake () 
	{
        ball = this;
	}
}
