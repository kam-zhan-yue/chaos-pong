using ChaosPong.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaosPongManager : MonoBehaviour, IGameManager
{
    public GameSettings gameSettings;

    public Transform redTeamSpawn;
    public Transform blueTeamSpawn;

    private readonly Team _redTeam = new(TeamSide.Red);
    private readonly Team _blueTeam = new(TeamSide.Blue);
    private GameState _gameState = new GameState(TeamSide.None);

    private void Awake()
    {
        ServiceLocator.Instance.Register<IGameManager>(this);
    }

    private void Start()
    {
        SetupGame();
        if (gameSettings.startGameImmediately)
        {
            StartGame();
        }
    }

    public void SetupGame()
    {
        SpawnTeam(true);
        SpawnTeam(false);
    }
    
    public void StartGame()
    {
        _gameState = new GameState(gameSettings.servingSide);
        IScoreService scoreService = ServiceLocator.Instance.Get<IScoreService>();
        scoreService.StartGame(_gameState);
        SetServe();
    }

    public GameState GetGameState()
    {
        return _gameState;
    }

    private void SpawnTeam(bool red)
    {
        TeamSide teamSide = red ? TeamSide.Red : TeamSide.Blue;
        TeamInfo teamInfo = red ? gameSettings.redTeamInfo : gameSettings.blueTeamInfo;
        Transform spawn = red ? redTeamSpawn : blueTeamSpawn;
        Team team = red ? _redTeam : _blueTeam;
        
        for (int i = 0; i < teamInfo.players.Count; ++i)
        {
            Character character;
            switch (teamInfo.players[i].type)
            {
                case PlayerType.Player:
                    character = Instantiate(red ? gameSettings.redPlayerPrefab : gameSettings.bluePlayerPrefab, spawn);
                    break;
                case PlayerType.Robot:
                    character = Instantiate(gameSettings.robotPrefab, spawn);
                    break;
                case PlayerType.Trainer:
                    character = Instantiate(gameSettings.trainerPrefab, spawn);
                    break;
                default:
                    character = Instantiate(red ? gameSettings.redPlayerPrefab : gameSettings.bluePlayerPrefab, spawn);
                    break;
            }

            teamInfo.players[i].teamSide = teamSide;
            character.Init(teamInfo.players[i]);
            team.AddCharacter(character);
        }
    }

    private void SetServe()
    {
        switch (gameSettings.servingSide)
        {
            case TeamSide.Red:
                _redTeam.SetServe();
                break;
            case TeamSide.Blue:
                _blueTeam.SetServe();
                break;
        }
    }

    public Team GetRedTeam()
    {
        return _redTeam;
    }

    public Team GetBlueTeam()
    {
        return _blueTeam;
    }

    public bool IsMultiCamera()
    {
        return _redTeam.PlayerCount() > 0 && _blueTeam.PlayerCount() > 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
