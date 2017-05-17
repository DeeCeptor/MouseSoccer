using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KickBall : NetworkBehaviour
{
    float kick_cooldown = 0.3f;
    float cur_kick_cooldown = 0;

    float min_kick_force = 100f;
    float extra_kick_force = 10000f;

    Vector3 prev_position;
    float mouse_delta;


    private void Update()
    {
        Vector2 mouse_d = (this.transform.position - prev_position) * (1 - Time.deltaTime);
        mouse_d.x = Mathf.Abs(mouse_d.x);
        mouse_d.y = Mathf.Abs(mouse_d.y);
        mouse_delta = Mathf.Clamp(mouse_d.magnitude, 0f, 1);
        
        // Gotta figure out better delta. Values are too small or non-existent

        cur_kick_cooldown -= Time.deltaTime;

        prev_position = this.transform.position;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball" && cur_kick_cooldown <= 0)
        {
            CmdKick(collision.gameObject, mouse_delta);
        }
    }


    [Command]
    public void CmdKick(GameObject ball, float speed)
    {
        Rigidbody2D physics = ball.GetComponent<Rigidbody2D>();
         
        // Figure out what direction we're kicking ball from
        Vector2 dir = (ball.transform.position - this.transform.position).normalized;

        // Figure out the speed with which we collided with ball
        Vector2 force = dir * min_kick_force + dir * extra_kick_force * speed;
        Debug.Log("Kick " + dir + ":" + force + " delta:" + mouse_delta);

        physics.AddForce(force);
    }
}
