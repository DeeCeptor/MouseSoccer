using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour 
{
    public static Ball ball;

    Rigidbody2D physics;
    Animator anim;

	void Awake () 
	{
        ball = this;
        physics = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
	}


    private void Update()
    {
        // Change animation speed based on ball's velocity
        //anim.speed = physics.velocity.magnitude / 10f;
        anim.speed = 0;

        var dir = (Vector3) physics.velocity - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
