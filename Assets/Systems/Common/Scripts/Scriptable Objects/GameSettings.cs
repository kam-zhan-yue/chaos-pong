using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObject/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Title("Red Team Info")]
    [HideLabel] public TeamInfo redTeamInfo;
    
    [Title("Blue Team Info")]
    [HideLabel] public TeamInfo blueTeamInfo;

    public Player redPlayerPrefab;
    public Player bluePlayerPrefab;
}