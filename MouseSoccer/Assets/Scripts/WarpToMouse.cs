using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WarpToMouse : NetworkBehaviour
{
    Rigidbody2D physics;

    float rotation_target;

    public string team;

    void Awake()
    {
        // Get which team we're on
    }

    void Start () 
	{
        physics = this.GetComponent<Rigidbody2D>();

        ScoreManager.score_manager.AssignTeam(this.gameObject);
    }


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //ScoreManager.score_manager.player_count+
    }


    void Update () 
	{
        if (!isLocalPlayer)
            return;

        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Only rotate towards a point if the mouse is significantly away from our current character
        if (Vector2.Distance(mouse_pos, this.transform.position) > 0.05f)
        {
            // Calculate rotation
            Vector2 pos = (Vector2)mouse_pos - (Vector2)this.transform.position;
            float angleRadians = Mathf.Atan2(pos.y, pos.x);
            float angleDegrees = angleRadians * Mathf.Rad2Deg;
            if (angleDegrees > 1f || angleDegrees < -1f)
                rotation_target = angleDegrees;
        }
        physics.MoveRotation(Mathf.Lerp(physics.rotation, rotation_target, 0.1f));


        // Move to the mouse
        mouse_pos = new Vector2(
            Mathf.Clamp(mouse_pos.x, CameraRect.arena_rect.xMin, CameraRect.arena_rect.xMax),
            Mathf.Clamp(mouse_pos.y, CameraRect.arena_rect.yMin, CameraRect.arena_rect.yMax));
        physics.MovePosition(mouse_pos);
	}
}
