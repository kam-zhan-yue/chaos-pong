using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObject/Game Settings")]
public class GameSettings : ScriptableObject
{
    public bool startGameImmediately = false;
    [Title("Red Team Info")]
    [HideLabel] public TeamInfo redTeamInfo;
    
    [Title("Blue Team Info")]
    [HideLabel] public TeamInfo blueTeamInfo;

    public Robot robotPrefab;
    public Trainer trainerPrefab;
    public Player redPlayerPrefab;
    public Player bluePlayerPrefab;

    public TeamSide servingSide;
}