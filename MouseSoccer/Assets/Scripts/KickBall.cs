using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KickBall : NetworkBehaviour
{
    float kick_cooldown = 0.3f;
    float cur_kick_cooldown = 0;

    float min_kick_force = 100f;
    float extra_kick_force = 150f;

    Vector3 prev_position;
    float mouse_delta;
    float prev_mouse_delta;

    int track_prev_deltas = 5;
    Queue<float> mouse_delta_history = new Queue<float>();


    private void Start()
    {
        // Populate the mouse_delta_history with just 0's for now
        for (int x = 0; x < track_prev_deltas; x++)
            mouse_delta_history.Enqueue(0);
    }


    private void FixedUpdate()
    {
        Vector2 mouse_d = (this.transform.position - prev_position) * (1 / Time.fixedDeltaTime);
        mouse_d.x = Mathf.Abs(mouse_d.x);
        mouse_d.y = Mathf.Abs(mouse_d.y);
        mouse_delta = mouse_d.magnitude;
        //mouse_delta = Mathf.Clamp(mouse_d.magnitude, 0f, 1);
        //Debug.Log(mouse_d.magnitude, this.gameObject);

        // Add new mouse_delta to top of queue, but skip first frame of each 0 speed
        if (mouse_delta != 0 || (mouse_delta == 0 && prev_mouse_delta == 0))
        {
            if (mouse_delta_history.Count > 0)
                mouse_delta_history.Dequeue();

            mouse_delta_history.Enqueue(mouse_delta);
        }

        // Gotta figure out better delta. Values are too small or non-existent

        cur_kick_cooldown -= Time.fixedDeltaTime;

        prev_position = this.transform.position;
        prev_mouse_delta = mouse_delta;
    }


    private void Update()
    {

    }


    public float AverageMouseDelta()
    {
        float total = 0;
        foreach (float f in mouse_delta_history)
            total += f;
        return total / mouse_delta_history.Count;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball" && cur_kick_cooldown <= 0)
        {
            if (isLocalPlayer)
            {
                CmdKick(collision.gameObject, AverageMouseDelta());
                cur_kick_cooldown = kick_cooldown;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ball" && cur_kick_cooldown <= 0)
        {
            if (isLocalPlayer)
            {
                CmdKick(collision.gameObject, AverageMouseDelta());
                cur_kick_cooldown = kick_cooldown;
            }
        }
    }


    [Command]
    public void CmdKick(GameObject ball, float speed)
    {
        Rigidbody2D physics = ball.GetComponent<Rigidbody2D>();
         
        if (speed <= 0)
        {
            CmdStopBall(ball);
            return;
        }

        // Stop the ball
        physics.velocity = Vector2.zero;

        // Figure out what direction we're kicking ball from
        Vector2 dir = (ball.transform.position - this.transform.position).normalized;

        // Figure out the speed with which we collided with ball
        Vector2 force = dir * min_kick_force + dir * extra_kick_force * speed;
        Debug.Log("Kick " + dir + ":" + force + " delta:" + speed, this.gameObject);

        physics.AddForce(force);
    }
    [Command]
    public void CmdStopBall(GameObject ball)
    {
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Debug.Log("Stopping ball", this.gameObject);
    }
}
