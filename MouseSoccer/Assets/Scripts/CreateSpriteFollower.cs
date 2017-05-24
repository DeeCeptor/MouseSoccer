using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CreateSpriteFollower : NetworkBehaviour
{
    public float lerp_speed = 50f;
    public bool create_only_if_not_local = true;
    public bool create_only_if_not_server = false;

	void Start ()
    {
        if ((create_only_if_not_local && isLocalPlayer) || (create_only_if_not_server && isServer))
        {
            Debug.Log("Not creating sprite follower for " + this.transform.name, this.gameObject);
            Destroy(this);
            return;
        }
        else
        {
            StartCoroutine(Delayed_Start());
        }
    }

    IEnumerator Delayed_Start()
    {
        yield return 0;

        Debug.Log("Creating sprite follower for " + this.transform.name, this.gameObject);
        GameObject instance = Instantiate(Resources.Load("SpriteFollower", typeof(GameObject))) as GameObject;
        instance.GetComponent<FollowObject>().Setup(this.transform, lerp_speed);
        instance.tag = this.gameObject.tag;
        instance.layer = this.gameObject.layer;

        // Maybe create a collider to live on the art?

        Destroy(this);
    }
}
