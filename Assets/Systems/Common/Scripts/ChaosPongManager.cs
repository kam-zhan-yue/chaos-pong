using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ChaosPongManager : MonoBehaviour
{
    public GameSettings gameSettings;

    public Transform redTeamSpawn;
    public Transform blueTeamSpawn;

    private void Awake()
    {
        SetupGame();
    }

    private void SetupGame()
    {
        SpawnTeam(gameSettings.redTeamInfo, redTeamSpawn);
        SpawnTeam(gameSettings.blueTeamInfo, blueTeamSpawn);
    }

    private void SpawnTeam(TeamInfo teamInfo, Transform spawnPoint)
    {
        for (int i = 0; i < teamInfo.players.Count; ++i)
        {
            Instantiate(gameSettings.playerPrefab, spawnPoint);
        }
    }
}
