using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public enum Team {  Blue, Red }

public class ScoreManager : NetworkBehaviour 
{
    public static ScoreManager score_manager;
    public Text score_text;
    public Text timer_text;
    public Text server_info;

    [SyncVar]
    public int blue_score;
    [SyncVar]
    public int red_score;
    [SyncVar]
    public int player_count = 0;
    [SyncVar]
    public float time_remaining = 60f;
    [SyncVar]
    public int red_players = 0;
    [SyncVar]
    public int blue_players = 0;

    // Dictionary of teams, and number of players on each team
    public Dictionary<string, int> players_on_teams = new Dictionary<string, int>();

    public Sprite blue_sprite;
    public Sprite red_sprite;

    public List<Player> players = new List<Player>();


	void Awake () 
	{
        score_manager = this;

        players_on_teams.Add("Blue", 0);
        players_on_teams.Add("Red", 0);
	}
    private void Start()
    {
        //Network.Connect("127.0.0.1");
        //Network.Disconnect();
    }


    public void AssignTeam(GameObject new_player)
    {
        /*
        players_on_teams.Keys
        foreach (string team in players_on_teams.Keys)
        {

        }*/
        if (!isServer)
            return;

        if (red_players > blue_players)
        {
            new_player.GetComponent<Player>().team = Team.Blue;
            blue_players--;
        }
        else
        {
            new_player.GetComponent<Player>().team = Team.Red;
            red_players++;
        }
    }

    public void SetPlayerColours(Team team, GameObject player)
    {
        switch (team)
        {
            case Team.Red:
                player.GetComponent<SpriteRenderer>().sprite = red_sprite;
                break;
            case Team.Blue:
                player.GetComponent<SpriteRenderer>().sprite = blue_sprite;
                break;
        }
    }


    public void BlueScored(int amount)
    {
        if (!isServer)
            return;

        blue_score += amount;
        Debug.Log("Blue scored " + amount);
    }
    public void RedScored(int amount)
    {
        if (!isServer)
            return;

        red_score += amount;
        Debug.Log("Red scored " + amount);
    }


    [Command]
    public void CmdReset()
    {
        blue_score = 0;
        red_score = 0;
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject go in balls)
        {
            go.transform.position = Vector3.zero;
        }
    }


    void OnPlayerConnected(NetworkPlayer player)
    {
        player_count++;
        Debug.Log("Player " + player_count + " connected from " + player.ipAddress + ":" + player.port);        
    }
    private void OnPlayerDisconnected(NetworkPlayer player)
    {
        player_count--;
        Debug.Log("Player " + player_count + " connected from " + player.ipAddress + ":" + player.port);

        // Find the player who left
        for (int x = 0; x < players.Count; x++)
        {
            Player p = players[x];

            if (p.network_guid == player.guid)
            {
                Destroy(p.gameObject);

                if (isServer)
                {
                    switch (p.team)
                    {
                        case Team.Blue:
                            blue_players--;
                            break;
                        case Team.Red:
                            red_players--;
                            break;
                    }
                }
                break;
            }
        }
    }


    [Server]
    public string ServerInfo()
    {
        string r = "";
        r += "Host IP address " + Network.player.ipAddress;
        r += ", external IP: " + Network.player.externalIP;
        return r;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            CmdReset();

        score_text.text = "Score: <color=blue>" + blue_score + "</color> : <color=red>" + red_score + "</color>";

        // Server updates
        if (isServer)
        {
            time_remaining -= Time.deltaTime;
            server_info.text = ServerInfo();
        }

        timer_text.text = "" + (int) time_remaining;


        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }


    void OnGUI()
    {
        GUILayout.Label("Player ping values");
        int i = 0;
        while (i < Network.connections.Length)
        {
            GUILayout.Label("Player " + Network.connections[i] + " - " + Network.GetAveragePing(Network.connections[i]) + " ms");
            i++;
        }

        if (isServer)
            ServerInfo();
    }
}
