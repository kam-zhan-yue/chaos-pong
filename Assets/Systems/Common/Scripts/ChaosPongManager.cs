using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ChaosPongManager : MonoBehaviour
{
    public static ChaosPongManager instance;
    public GameSettings gameSettings;

    public Transform redTeamSpawn;
    public Transform blueTeamSpawn;

    private readonly Team _redTeam = new();
    private readonly Team _blueTeam = new();

    public Team RedTeam => _redTeam;
    public Team BlueTeam => _blueTeam;
    public bool MultiCamera => _redTeam.PlayerNum > 0 && _blueTeam.PlayerNum > 0;

    private void Awake()
    {
        if(instance && instance != this)
            Destroy(gameObject);
        instance = this;
        SetupGame();
    }

    private void SetupGame()
    {
        SpawnTeam(true);
        SpawnTeam(false);
    }

    private void SpawnTeam(bool red)
    {
        TeamInfo teamInfo = red ? gameSettings.redTeamInfo : gameSettings.blueTeamInfo;
        Player prefab = red ? gameSettings.redPlayerPrefab : gameSettings.bluePlayerPrefab;
        Transform spawn = red ? redTeamSpawn : blueTeamSpawn;
        Team team = red ? _redTeam : _blueTeam;
        
        for (int i = 0; i < teamInfo.players.Count; ++i)
        {
            Player player = Instantiate(prefab, spawn);
            player.Init(teamInfo.players[i]);
            team.AddPlayer(player);
        }
    }
}
