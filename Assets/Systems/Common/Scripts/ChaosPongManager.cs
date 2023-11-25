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
        SpawnTeam(_redTeam, gameSettings.redTeamInfo, redTeamSpawn);
        SpawnTeam(_blueTeam, gameSettings.blueTeamInfo, blueTeamSpawn);
    }

    private void SpawnTeam(Team team, TeamInfo teamInfo, Transform spawnPoint)
    {
        for (int i = 0; i < teamInfo.players.Count; ++i)
        {
            Player player = Instantiate(gameSettings.playerPrefab, spawnPoint);
            team.AddPlayer(player);
        }
    }
}
