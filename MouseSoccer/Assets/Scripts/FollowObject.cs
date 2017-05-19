using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FollowObject : MonoBehaviour
{
    public Transform object_to_follow;
    public float lerp_rate = 0.5f;


	void Start ()
    {
        if (object_to_follow == null)
        {
            Debug.LogError("Object to follow is null", this.gameObject);
            Destroy(this.gameObject);
            return;
        }

        this.transform.localScale = object_to_follow.transform.localScale;

        // Set same sprite, disable original's sprite
        this.GetComponent<SpriteRenderer>().sprite = object_to_follow.GetComponentInChildren<SpriteRenderer>().sprite;
        object_to_follow.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
	

    public void Setup(Transform obj_to_follow, float lerp_speed)
    {
        lerp_rate = lerp_speed;
        object_to_follow = obj_to_follow;
        this.transform.name = obj_to_follow.name + " Graphics";
    }


	void LateUpdate ()
    {
        if (!object_to_follow)
        {
            Destroy(this);
            return;
        }

        // Set position and rotation based on object we're following
        this.transform.position = Vector3.Lerp(this.transform.position, object_to_follow.transform.position, lerp_rate);
        this.transform.rotation = object_to_follow.transform.rotation;
	}
}
