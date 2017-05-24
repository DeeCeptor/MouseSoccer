using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public string network_guid;
    public NetworkPlayer network_player;
    [SyncVar]
    public Team team;

	void Awake ()
    {
        ScoreManager.score_manager.AssignTeam(this.gameObject);

        StartCoroutine(Delayed_Setup());
    }
    private void Start()
    {
        if (isLocalPlayer)
            AssignNetworkInfo(Network.player);

    }
    IEnumerator Delayed_Setup()
    {
        yield return new WaitForSeconds(0f);
        this.transform.name = network_guid;
        ScoreManager.score_manager.SetPlayerColours(team, this.gameObject);
    }


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //AssignNetworkInfo(Network.player);
        //ScoreManager.score_manager.player_count+
    }


    public void AssignNetworkInfo(NetworkPlayer n_player)
    {
        network_player = n_player;

        network_guid = network_player.guid;

        this.transform.name = "Player " + network_guid;
    }



    void Update ()
    {
		
	}
}
