using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaosPongPhysics : MonoBehaviour, IPhysicsService
{
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private LineRenderer line;
    [SerializeField] private int frameIterations = 100;
    [SerializeField] private Gradient validLine;
    [SerializeField] private Gradient invalidLine;
    [SerializeField] private Transform obstaclesParent;

    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    private readonly Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();
    private Ball _ghostBall;

    private void Awake()
    {
        ServiceLocator.Instance.Register<IPhysicsService>(this);
    }

    private void Start()
    {
        CreatePhysicsScene();
        CreateGhostBall();
    }

    private void CreatePhysicsScene()
    {
        _simulationScene =
            SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();

        foreach (Transform obj in obstaclesParent)
        {
            GameObject ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            if (ghostObj.TryGetComponent(out Renderer render))
                render.enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
            if (!ghostObj.isStatic) _spawnedObjects.Add(obj, ghostObj.transform);
        }
    }

    private void CreateGhostBall()
    {
        Ball ghostObj = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        ghostObj.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene);
        _ghostBall = ghostObj;
    }

    // private void Update()
    // {
    //     foreach (var item in _spawnedObjects) {
    //         item.Value.position = item.Key.position;
    //         item.Value.rotation = item.Key.rotation;
    //     }
    // }

    public void Projection(Vector3 position, Vector3 velocity, TeamSide side = TeamSide.None)
    {
        _ghostBall.transform.position = position;

        _ghostBall.Serve(velocity, true);

        line.positionCount = frameIterations;

        for (int i = 0; i < frameIterations; ++i)
        {
            _physicsScene.Simulate(Time.fixedDeltaTime);
            line.SetPosition(i, _ghostBall.transform.position);
        }

        line.colorGradient = _ghostBall.Valid ? validLine : invalidLine;
        //move the ghost object back to the start
    }

    public void HideProjection()
    {
        line.positionCount = 0;
    }

    public void ServeBall(Vector3 position, Vector3 velocity, TeamSide teamSide = TeamSide.None)
    {
        Ball ball = Instantiate(ballPrefab, position, Quaternion.identity);
        ball.Serve(velocity, false);
    }
}