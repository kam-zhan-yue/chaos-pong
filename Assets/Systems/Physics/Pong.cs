using System;
using System.Collections.Generic;
using ChaosPong.Common;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

public class Pong : MonoBehaviour
{
    //Constants
    private const float LINE_TIME_STEP = 0.01f;
    private const float BOUNCE_DECAY = 0.95f;
    
    //Exposed Variables
    [SerializeField] private Vector3 initialVelocity;

    //Private Variables
    private Vector3 _velocity;
    private SphereCollider _sphereCollider;
    private LineRenderer _lineRenderer;
    private Rigidbody _rigidbody;
    private ITableService _tableService;
    private float _radius;

    private float Bottom => transform.position.y - _radius;

    private CoroutineHandle _bounceRoutine;

    private bool _simulated = false;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _lineRenderer = GetComponent<LineRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _tableService = ServiceLocator.Instance.Get<ITableService>();
        // _velocity = initialVelocity;
        // _radius = _sphereCollider.radius * transform.localScale.y;
        // Timing.RunCoroutine(Bounce());
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
        // transform.position += _velocity * Time.deltaTime;
        _rigidbody.velocity = _velocity;
        // Vector3 position = _rigidbody.position + _velocity * Time.deltaTime;
        // _rigidbody.MovePosition(position);
        _velocity += Physics.gravity * Time.deltaTime;
    }
    
    [Button]
    public void ApplyVelocity(Vector3 velocity)
    {
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

            // TeamSide teamSide = table.GetTeamSide(finalPosition);
            // Debug.Log($"Landed on {teamSide}");
            _bounceRoutine = Timing.RunCoroutine(Bounce().CancelWith(gameObject));
        }
        else
        {
            Debug.LogError("Time is negative");
        }
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

    private Vector3 SimulatePosition(Vector3 initial, Vector3 position, float time, out Vector3 velocity)
    {
        velocity = initial + Physics.gravity * time;
        return position + initial * time + 0.5f * Physics.gravity * time * time;
    }

    [Button]
    public void LaunchAtSide(TeamSide teamSide, float height)
    {
        Debug.Log($"Try Launch at {teamSide} at {height}");
        Vector3 point = _tableService.GetRandomPoint(teamSide);
        if (ChaosPongHelper.CalculateLaunchVelocity(transform.position, point, height, out Vector3 velocity))
        {
            ApplyVelocity(velocity);
        }
    }

    [Button]
    public void LaunchAtTarget(Transform target, float height)
    {
        if (ChaosPongHelper.CalculateLaunchVelocity(transform.position, target.position, height, out Vector3 velocity))
        {
            ApplyVelocity(velocity);
        }
    }
}
