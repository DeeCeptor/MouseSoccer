using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRect : MonoBehaviour 
{
    public static Rect camera_rect;


	void Awake () 
	{
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(
            Camera.main.pixelWidth, Camera.main.pixelHeight));

        camera_rect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);
    }
}
