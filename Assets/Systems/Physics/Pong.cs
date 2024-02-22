using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ChaosPong.Common;
using MEC;
using Sirenix.OdinInspector;
using SuperMaxim.Messaging;
using Unity.VisualScripting;
using UnityEngine;

public class Pong : MonoBehaviour
{
    //Constants
    private const float LINE_TIME_STEP = 0.01f;
    private const float BOUNCE_DECAY = 0.95f;
    
    //Private Variables
    private Vector3 _velocity;
    private SphereCollider _sphereCollider;
    private LineRenderer _lineRenderer;
    private Rigidbody _rigidbody;
    private ITableService _tableService;
    private float _radius;
    private TeamSide _possession = TeamSide.None;

    private float Bottom => transform.position.y - _radius;

    private CoroutineHandle _bounceRoutine;

    private bool _simulated = false;

    public static Action<HitInfo> onHit;
    public static Action<BounceInfo> onBounce;
    
    [ReadOnly, ShowInInspector]
    private readonly List<HitInfo> _hits = new List<HitInfo>();

    private PongState _pongState = PongState.Idle;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _lineRenderer = GetComponent<LineRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _tableService = ServiceLocator.Instance.Get<ITableService>();
        _radius = _sphereCollider.radius * transform.localScale.x;
        _rigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (_simulated)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        _rigidbody.velocity = _velocity;
        _velocity += Physics.gravity * Time.deltaTime;
    }
    
    [Button]
    private void ApplyVelocity(Vector3 velocity)
    {
        _rigidbody.isKinematic = false;
        _simulated = true;
        _velocity = velocity;
        Timing.KillCoroutines(_bounceRoutine);
        _bounceRoutine = Timing.RunCoroutine(Bounce().CancelWith(gameObject));
    }

    private IEnumerator<float> Bounce()
    {
        Vector3 initial = _velocity;
        Vector3 position = transform.position;
        //Calculate the time to bounce (returns time to table or ground)
        float time = TimeToBounce(initial, position);
        DrawLineRenderer(initial, position, 10);
        if (time > 0f)
        {
            yield return Timing.WaitForSeconds(time);
            Vector3 finalPosition = SimulatePosition(initial, position, time, out _);
            transform.position = finalPosition;

            Vector3 finalVelocity = initial + Physics.gravity * time;
            _velocity = GetBounceVelocity(finalVelocity);

            ITableService table = ServiceLocator.Instance.Get<ITableService>();
            TeamSide teamSide = table.GetTeamSide(finalPosition);
            BounceInfo bounceInfo = new(teamSide, finalVelocity);
            OnBounce(bounceInfo);
            _bounceRoutine = Timing.RunCoroutine(Bounce().CancelWith(gameObject));
        }
        else
        {
            Debug.LogError("Time is negative");
        }
    }
    
    private void ApplyServe(Vector3 velocity, float nextHeight, TeamSide targetSide)
    {
        _rigidbody.isKinematic = false;
        _simulated = true;
        _velocity = velocity;
        Timing.KillCoroutines(_bounceRoutine);
        _bounceRoutine = Timing.RunCoroutine(SimulatedBounce(nextHeight, targetSide).CancelWith(gameObject));
    }

    private IEnumerator<float> SimulatedBounce(float height, TeamSide targetSide)
    {
        Vector3 initial = _velocity;
        Vector3 position = transform.position;
        //Calculate the time to bounce (returns time to table or ground)
        float time = TimeToBounce(initial, position);
        DrawLineRenderer(initial, position, 10);
        if (time > 0f)
        {
            yield return Timing.WaitForSeconds(time);
            Vector3 finalPosition = SimulatePosition(initial, position, time, out _);
            transform.position = finalPosition;
            LaunchAtSide(height, targetSide);
        }
        else
        {
            Debug.LogError("Time is negative");
        }
    }

    private void OnBounce(BounceInfo bounceInfo)
    {
        // Debug.Log($"State: {_pongState} Possession: {_possession}");
        switch (_pongState)
        {
            //If serving, check whether can turn into returning
            case PongState.Serving:
                //If landed on the target side, change to returnable
                if (bounceInfo.teamSide == _possession)
                {
                    _pongState = PongState.Returnable;
                }
                break;
            //If returning, check whether can turn into returnable
            case PongState.Returning:
                if (bounceInfo.teamSide == _possession)
                {
                    _pongState = PongState.Returnable;
                }
                break;
        }
        
        //If bounced on none, award points accordingly
        if (_pongState != PongState.Scored && bounceInfo.teamSide == TeamSide.None)
        {
            if (_possession == TeamSide.Blue)
            {
                PublishScore(TeamSide.Red);
            }
            else if (_possession == TeamSide.Red)
            {
                PublishScore(TeamSide.Blue);
            }
        }
        onBounce?.Invoke(bounceInfo);
    }

    private void PublishScore(TeamSide teamSide)
    {
        ScorePayload payload = new ScorePayload
        {
            TeamSide = teamSide
        };
        Messenger.Default.Publish(payload);
        _pongState = PongState.Scored;
        Timing.RunCoroutine(DestroyRoutine().CancelWith(gameObject));
    }

    private float TimeToBounce(Vector3 initial, Vector3 position)
    {
        float time = TimeToBounce(initial.y, position.y, _radius + _tableService.Height());
        //Check if the ball can bounce onto the table
        if (time >= 0f)
        {
            Vector3 finalPosition = SimulatePosition(initial, position, time, out _);
            //Return the time if the final bounce is on the table
            return !_tableService.InBounds(finalPosition) ? TimeToBounce(initial.y, position.y, _radius) : time;
        }
        return TimeToBounce(initial.y, position.y, _radius);
    }

    [Button]
    private float TimeToBounce(float u, float y, float targetHeight)
    {
        float a = 0.5f * Physics.gravity.y;
        float b = u;
        float c = y - targetHeight;
        float discriminant = b * b - 4 * a * c;
        if (discriminant >= 0)
        {
            float squared = Mathf.Sqrt(discriminant);
            //todo check whether can hit table. if not, then hit the ground instead
            float ans1 = (-b - squared) / (2 * a);
            float ans2 = (-b + squared) / (2 * a);
            float time = ans1 > ans2 ? ans1 : ans2;
            return time;
        }
        return -1f;
    }

    private void DrawLineRenderer(Vector3 initial, Vector3 position, int bounces = 1)
    {
        int length = 0;
        List<Vector3> positions = new List<Vector3>();
        Vector3 v = initial;
        Vector3 p = position;
        
        for (int b = 0; b < bounces; ++b)
        {
            float time = TimeToBounce(v, p);
            int steps = (int)(time / LINE_TIME_STEP);
            length += steps + 1;
            for (int s = 0; s < steps; ++s)
            {
                Vector3 stepPosition = SimulatePosition(v, p, LINE_TIME_STEP * s, out _);
                positions.Add(stepPosition);
            }
            
            Vector3 finalPosition = SimulatePosition(v, p, time, out Vector3 finalVelocity);
            positions.Add(finalPosition);

            v = GetBounceVelocity(finalVelocity);
            p = finalPosition;
        }

        _lineRenderer.positionCount = length;
        for (int i = 0; i < positions.Count; ++i)
        {
            _lineRenderer.SetPosition(i, positions[i]);
        }
    }

    private Vector3 GetBounceVelocity(Vector3 velocity)
    {
        Vector3 bounceVelocity = velocity;
        bounceVelocity.y *= -1f * BOUNCE_DECAY;
        return bounceVelocity;
    }

    public void Toss()
    {
        Vector3 tossVelocity = Vector3.up * ChaosPongHelper.TOSS_SPEED;
        ApplyVelocity(tossVelocity);
    }

    public void Serve(TeamSide teamSide, float height)
    {
        //Find a point that can be served on
        Vector3 point = ServiceLocator.Instance.Get<ITableService>().GetServePoint(teamSide, transform.position);
        //Hit the ball to that point and pass in the next height
        if (Launch(point, height, teamSide, true, out Vector3 velocity))
        {
            ApplyServe(velocity, ChaosPongHelper.SERVE_BOUNCE_HEIGHT, ChaosPongHelper.GetOppositeSide(teamSide));
        }
        //Set state to serving
        _pongState = PongState.Serving;
    }

    public void Return(TeamSide teamSide, float height)
    {
        //Only return if the team has possession of the ball
        if (_possession == teamSide && _pongState == PongState.Returnable || _pongState == PongState.Idle)
        {
            TeamSide opposite = ChaosPongHelper.GetOppositeSide(teamSide);
            LaunchAtSide(height, opposite);
            _pongState = PongState.Returning;
        }
    }

    private Vector3 SimulatePosition(Vector3 initial, Vector3 position, float time, out Vector3 velocity)
    {
        velocity = initial + Physics.gravity * time;
        return position + initial * time + 0.5f * Physics.gravity * time * time;
    }

    [Button]
    private void LaunchAtSide(float height, TeamSide teamSide, bool serve = false)
    {
        Vector3 point = _tableService.GetRandomPoint(teamSide);
        Launch(point, height, teamSide, serve);
    }

    private void Launch(Vector3 target, float height, TeamSide teamSide = TeamSide.None, bool serve = false)
    {
        if (Launch(target, height, teamSide, serve, out Vector3 velocity))
        {
            ApplyVelocity(velocity);
        }
    }

    private bool Launch(Vector3 target, float height, TeamSide teamSide, bool serve, out Vector3 velocity)
    {
        if (ChaosPongHelper.CalculateLaunchVelocity(transform.position, target, height, out Vector3 launchVelocity))
        {
            HitInfo hitInfo = new(serve, transform.position, launchVelocity, teamSide);
            _possession = teamSide;
            onHit?.Invoke(hitInfo);
            _hits.Add(hitInfo);
            velocity = launchVelocity;
            return true;
        }

        velocity = launchVelocity;
        return false;
    }

    public void DebugHitInfo(HitInfo hitInfo)
    {
        transform.position = hitInfo.position;
        _possession = hitInfo.teamSide;
        ApplyVelocity(hitInfo.velocity);
    }

    private IEnumerator<float> DestroyRoutine()
    {
        yield return Timing.WaitForSeconds(5f);
        Destroy(gameObject);
    }

    #if UNITY_EDITOR
    [BoxGroup("Editor"), Button]
    private void SaveHitInfo()
    {
        PhysicsDebugger physicsDebugger = UnityEditor.AssetDatabase.LoadAssetAtPath<PhysicsDebugger>("Assets/Scriptable Objects/Physics Debugger.asset");
        physicsDebugger.SaveHits(_hits);
    }
    #endif
}
