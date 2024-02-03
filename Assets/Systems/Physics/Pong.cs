using System.Collections.Generic;
using MEC;
using UnityEngine;

public class Pong : MonoBehaviour
{
    private const float LINE_TIME_STEP = 0.01f;
    [SerializeField] private Vector3 initialVelocity;

    private Vector3 _velocity;
    private SphereCollider _sphereCollider;
    private LineRenderer _lineRenderer;
    private float _radius;

    private float Bottom => transform.position.y - _radius;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _velocity = initialVelocity;
        _radius = _sphereCollider.radius * transform.localScale.y;
        Timing.RunCoroutine(Bounce());
    }

    private IEnumerator<float> Bounce()
    {
        Vector3 initial = _velocity;
        Vector3 position = transform.position;
        float time = TimeToBounce(initial.y, position.y);
        DrawLineRenderer(initial, position);
        if (time > 0f)
        {
            yield return Timing.WaitForSeconds(time);
            Transform ballTransform = transform;
            Vector3 bounceTransform = ballTransform.position;
            bounceTransform.y = _radius;
            ballTransform.position = bounceTransform;

            Vector3 finalVelocity = initial + Physics.gravity * time;
            Vector3 bounceVelocity = finalVelocity;
            bounceVelocity.y *= -1f;
            _velocity = bounceVelocity;

            Timing.RunCoroutine(Bounce());
        }
        else
        {
            Debug.LogError("Time is negative");
        }
    }

    private float TimeToBounce(float u, float y)
    {
        float a = 0.5f * Physics.gravity.y;
        float b = u;
        float c = y - _radius;
        float ans1 = (-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
        float ans2 = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
        float time = ans1 > ans2 ? ans1 : ans2;
        return time;
    }

    private void DrawLineRenderer(Vector3 initial, Vector3 position)
    {
        float time = TimeToBounce(initial.y, position.y);
        int steps = (int)(time / LINE_TIME_STEP);
        _lineRenderer.positionCount = steps + 1;
        for (int i = 0; i < steps; ++i)
        {
            Vector3 stepPosition = SimulatePosition(initial, position, LINE_TIME_STEP * i, out _);
            _lineRenderer.SetPosition(i, stepPosition);
        }
        
        Vector3 finalPosition = SimulatePosition(initial, position, time, out _);
        _lineRenderer.SetPosition(steps, finalPosition);
    }

    private Vector3 SimulatePosition(Vector3 initial, Vector3 position, float time, out Vector3 velocity)
    {
        velocity = initial + Physics.gravity * time;
        return position + initial * time + 0.5f * Physics.gravity * time * time;
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position += _velocity * Time.deltaTime;
        _velocity += Physics.gravity * Time.deltaTime;
    }
}
