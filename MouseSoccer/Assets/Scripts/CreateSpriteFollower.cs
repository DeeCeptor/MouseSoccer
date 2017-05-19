using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class CreateSpriteFollower : NetworkBehaviour
{
    public float lerp_speed = 0.5f;
    public bool create_only_if_not_local = true;

	void Start ()
    {
        if (create_only_if_not_local && (isLocalPlayer))
        {
            Destroy(this);
            return;
        }

        GameObject instance = Instantiate(Resources.Load("SpriteFollower", typeof(GameObject))) as GameObject;
        instance.GetComponent<FollowObject>().Setup(this.transform, lerp_speed);
        Destroy(this);
    }
}
