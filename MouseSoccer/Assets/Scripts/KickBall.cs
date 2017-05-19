using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KickBall : NetworkBehaviour
{
    float kick_cooldown = 0.3f;
    float cur_kick_cooldown = 0;

    float min_kick_force = 100f;
    float extra_kick_force = 1000f;

    Vector3 prev_position;
    float mouse_delta;

    int track_prev_deltas = 5;
    Queue<float> mouse_delta_history = new Queue<float>();


    private void Start()
    {
        // Populate the mouse_delta_history with just 0's for now
        for (int x = 0; x < track_prev_deltas; x++)
            mouse_delta_history.Enqueue(0);
    }


    private void Update()
    {
        // Remove the last mouse_delta histories
        mouse_delta_history.Dequeue();

        Vector2 mouse_d = (this.transform.position - prev_position) * (1 - Time.deltaTime);
        mouse_d.x = Mathf.Abs(mouse_d.x);
        mouse_d.y = Mathf.Abs(mouse_d.y);
        mouse_delta = Mathf.Clamp(mouse_d.magnitude, 0f, 1);

        // Add new mouse_delta to top of queue
        mouse_delta_history.Enqueue(mouse_delta);

        // Gotta figure out better delta. Values are too small or non-existent

        cur_kick_cooldown -= Time.deltaTime;

        prev_position = this.transform.position;
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
                CmdKick(collision.gameObject, AverageMouseDelta());
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ball" && cur_kick_cooldown <= 0)
        {
            if (isLocalPlayer)
                CmdKick(collision.gameObject, AverageMouseDelta());
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

        // Figure out what direction we're kicking ball from
        Vector2 dir = (ball.transform.position - this.transform.position).normalized;

        // Figure out the speed with which we collided with ball
        Vector2 force = dir * min_kick_force + dir * extra_kick_force * speed;
        Debug.Log("Kick " + dir + ":" + force + " delta:" + speed);

        physics.AddForce(force);
    }
    [Command]
    public void CmdStopBall(GameObject ball)
    {
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Debug.Log("Stopping ball", this.gameObject);
    }
}
