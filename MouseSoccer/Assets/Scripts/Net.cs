using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    public Team teams_goal;    // Blue or red


    void Start()
    {

    }


    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ball")
        {
            switch (teams_goal)
            {
                case Team.Blue:
                    ScoreManager.score_manager.RedScored(1);
                    break;
                case Team.Red:
                    ScoreManager.score_manager.BlueScored(1);
                    break;
            }

            // Move the ball to the center
            other.gameObject.transform.position = Vector2.zero;
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
