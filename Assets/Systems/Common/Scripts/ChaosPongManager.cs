using Kuroneko.UtilityDelivery;
using Sirenix.OdinInspector;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaosPongManager : MonoBehaviour, IGameManager
{
    public GameSettings gameSettings;

    public Transform redTeamSpawn;
    public Transform blueTeamSpawn;

    private readonly Team _redTeam = new(TeamSide.Red);
    private readonly Team _blueTeam = new(TeamSide.Blue);

    private GameState _gameState = new(TeamSide.None, new Team(TeamSide.Red), new Team(TeamSide.Blue));
    private int TotalPoints => _gameState.RedPoints.Value + _gameState.BluePoints.Value;

    private void Awake()
    {
        ServiceLocator.Instance.Register<IGameManager>(this);
    }

    private void Start()
    {
        if (gameSettings.setupGameImmediately)
        {
            SetupGame();
            if (gameSettings.startGameImmediately)
            {
                StartGame();
            }
        }
        else
        {
            ShowSetup();
        }
        Messenger.Default.Subscribe<EventPayload>(OnEvent);
        Messenger.Default.Subscribe<ScorePayload>(OnScore);
    }

    public GameSettings GameSettings() => gameSettings;

    private void ShowSetup()
    {
        IConnectorService connectorService = ServiceLocator.Instance.Get<IConnectorService>();
        connectorService?.ShowSetup();
        ICameraService cameraService = ServiceLocator.Instance.Get<ICameraService>();
        cameraService?.ShowSetup();
    }

    public void SetupGame()
    {
        SpawnTeam(true);
        SpawnTeam(false);
        ICameraService cameraService = ServiceLocator.Instance.Get<ICameraService>();
        cameraService?.SetupGame();
    }
    
    [Button]
    public void StartGame()
    {
        _gameState = new GameState(gameSettings.servingSide, _redTeam, _blueTeam);
        IConnectorService connectorService = ServiceLocator.Instance.Get<IConnectorService>();
        connectorService?.StartGame(_gameState);
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
        
        for (int i = 0; i < teamInfo.players.Length; ++i)
        {
            Character character;
            if (teamInfo.players[i].config != null && teamInfo.players[i].config.player != null)
            {
                character = Instantiate(teamInfo.players[i].config.player, spawn);
            }
            else
            {
                switch (teamInfo.players[i].type)
                {
                    case CharacterType.Player:
                        character = Instantiate(red ? gameSettings.redPlayerPrefab : gameSettings.bluePlayerPrefab, spawn);
                        break;
                    case CharacterType.Robot:
                        character = Instantiate(gameSettings.robotPrefab, spawn);
                        break;
                    case CharacterType.Trainer:
                        character = Instantiate(gameSettings.trainerPrefab, spawn);
                        break;
                    default:
                        character = Instantiate(red ? gameSettings.redPlayerPrefab : gameSettings.bluePlayerPrefab, spawn);
                        break;
                }
            }

            teamInfo.players[i].teamSide = teamSide;
            character.Init(teamInfo.players[i]);
            team.AddCharacter(character);
        }
    }

    private void SetServe()
    {
        TeamSide server = ChaosPongHelper.GetServer(TotalPoints, gameSettings.servingSide);
        switch (server)
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

    private void OnEvent(EventPayload payload)
    {
        if (payload.gameEvent == GameEvent.StartRound)
        {
            SetServe();
        }
    }
    
    private void OnScore(ScorePayload payload)
    {
        // Debug.Log($"Point for: {payload.TeamSide}");
        if (payload.TeamSide == TeamSide.Blue)
            _gameState.BluePoint();
        else if (payload.TeamSide == TeamSide.Red)
            _gameState.RedPoint();
        Messenger.Default.Publish(new EventPayload(GameEvent.StartRound));
    }

    [Button]
    private void PrintPoints()
    {
        Debug.Log($"Red: {_gameState.RedPoints} Blue: {_gameState.BluePoints}");
    }
}
