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

    [SyncVar]
    public int blue_score;
    [SyncVar]
    public int red_score;
    [SyncVar]
    public int player_count = 0;

    public Dictionary<string, int> players_on_teams = new Dictionary<string, int>();

    public Sprite blue_sprite;
    public Sprite red_sprite;

	void Awake () 
	{
        score_manager = this;

        players_on_teams.Add("Blue", 0);
        players_on_teams.Add("Red", 0);
	}


    public void AssignTeam(GameObject new_player)
    {
        /*
        players_on_teams.Keys
        foreach (string team in players_on_teams.Keys)
        {

        }*/
        if (players_on_teams["Red"] > players_on_teams["Blue"])
        {
            JoinTeam("Blue", new_player);
        }
        else
            JoinTeam("Red", new_player);
    }

    public void JoinTeam(string team_name, GameObject player)
    {
        players_on_teams[team_name]++;
        player.GetComponent<WarpToMouse>().team = team_name;

        switch (team_name)
        {
            case "Red":
                player.GetComponent<SpriteRenderer>().sprite = red_sprite;
                break;
            case "Blue":
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


    void OnPlayerConnected(NetworkPlayer player)
    {
        player_count++;
        Debug.Log("Player " + player_count + " connected from " + player.ipAddress + ":" + player.port);        
    }
    private void OnPlayerDisconnected(NetworkPlayer player)
    {
        player_count--;
        Debug.Log("Player " + player_count + " connected from " + player.ipAddress + ":" + player.port);
    }


    void Update()
    {
        score_text.text = "Score: <color=blue>" + blue_score + "</color> : <color=red>" + red_score + "</color>";
    }
}
