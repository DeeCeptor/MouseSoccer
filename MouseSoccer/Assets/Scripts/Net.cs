using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Net : NetworkBehaviour
{
    public Team teams_goal;    // Blue or red
    public string scoring_message;

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
            Effects();

            if (!isServer)
                return;

            StartCoroutine(GoalScored(other));
        }
    }

    public void Effects()
    {
        this.GetComponentInChildren<ParticleSystem>().Play();

        GameObject instance = Instantiate(Resources.Load("ScoredText", typeof(GameObject))) as GameObject;
        instance.GetComponent<TextMesh>().text = scoring_message;
        Destroy(instance, 1.5f);
    }


    IEnumerator GoalScored(Collider2D other)
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

        // Respawn the ball after a second
        other.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);

        other.gameObject.SetActive(true);
        other.gameObject.transform.position = Vector2.zero;
        other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
