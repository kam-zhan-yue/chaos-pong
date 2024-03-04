using System;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(LineRenderer))]
public abstract class Projectile : MonoBehaviour
{
    //Constants
    private const float LINE_TIME_STEP = 0.01f;
    private const float BOUNCE_DECAY = 0.95f;
 
    //Exposed Variables
    [SerializeField] public bool bounceable = true;
    
    //Protected Variables
    protected Rigidbody Rigidbody { get; private set; }

    protected ITableService TableService
    {
        get { return _tableService ??= ServiceLocator.Instance.Get<ITableService>(); }
    }
    
    //Protected Variables
    protected float timeScale = ChaosPongPhysics.DEFAULT_TIME_SCALE;
    protected float Radius { get; private set; }
    protected Vector3 velocity;
    protected Vector3 acceleration;
    protected bool simulated;
    protected CoroutineHandle bounceRoutine;
    protected TeamSide possession = TeamSide.None;
    [ReadOnly, ShowInInspector]
    protected readonly List<HitInfo> hits = new List<HitInfo>();
    
    //Private Variables
    private SphereCollider _sphereCollider;
    private LineRenderer _lineRenderer;
    private ITableService _tableService;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _lineRenderer = GetComponent<LineRenderer>();
        Rigidbody = GetComponent<Rigidbody>();
        Radius = _sphereCollider.radius * transform.localScale.x;
        Rigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (simulated)
        {
            UpdatePosition();
        }
    }

    public void SetTimeScale(float scale)
    {
        timeScale = scale;
        Recalculate();
    }
    
    private void UpdatePosition()
    {
        Vector3 adjustedVelocity = velocity * timeScale;
        Rigidbody.velocity = adjustedVelocity;
        velocity += GetAcceleration() * (Time.deltaTime * timeScale);
    }
    
    protected Vector3 GetAcceleration()
    {
        return acceleration + Physics.gravity;
    }

    private void Recalculate()
    {
        ApplyVelocity(velocity);
    }
    
    [Button]
    protected void ApplyVelocity(Vector3 otherVelocity)
    {
        Rigidbody.isKinematic = false;
        simulated = true;
        velocity = otherVelocity;
        Timing.KillCoroutines(bounceRoutine);
        if(bounceable)
            bounceRoutine = Timing.RunCoroutine(Bounce().CancelWith(gameObject));
    }

    private IEnumerator<float> Bounce()
    {
        Vector3 initial = velocity;
        Vector3 position = transform.position;
        //Calculate the time to bounce (returns time to table or ground)
        float time = TimeToBounce(initial, position);
        if (time > 0f)
        {
            DrawLineRenderer(initial, position, 10);
            yield return Timing.WaitForSeconds(time / timeScale);
            Vector3 finalPosition = SimulatePosition(initial, position, time, out _);
            transform.position = finalPosition; 

            Vector3 finalVelocity = initial + GetAcceleration() * time;
            velocity = GetBounceVelocity(finalVelocity);

            ITableService table = ServiceLocator.Instance.Get<ITableService>();
            TeamSide teamSide = table.GetTeamSide(finalPosition);
            BounceInfo bounceInfo = new(teamSide, finalVelocity);
            OnBounce(bounceInfo);
            bounceRoutine = Timing.RunCoroutine(Bounce().CancelWith(gameObject));
        }
        else
        {
            Debug.LogError("Time is negative");
        }
    }
    
    private float TimeToBounce(Vector3 initial, Vector3 position)
    {
        float time = TimeToBounce(initial.y, position.y, Radius + TableService.Height());
        //Check if the ball can bounce onto the table
        if (time >= 0f)
        {
            Vector3 finalPosition = SimulatePosition(initial, position, time, out _);
            //Return the time if the final bounce is on the table
            return !TableService.InBounds(finalPosition) ? TimeToBounce(initial.y, position.y, Radius) : time;
        }
        return TimeToBounce(initial.y, position.y, Radius);
    }

    [Button]
    private float TimeToBounce(float u, float y, float targetHeight)
    {
        float a = 0.5f * GetAcceleration().y;
        float b = u;
        float c = y - targetHeight;
        float discriminant = b * b - 4 * a * c;
        if (discriminant >= 0)
        {
            float squared = Mathf.Sqrt(discriminant);
            float ans1 = (-b - squared) / (2 * a);
            float ans2 = (-b + squared) / (2 * a);
            float time = ans1 > ans2 ? ans1 : ans2;
            return time;
        }
        return -1f;
    }
    
    protected Vector3 SimulatePosition(Vector3 initial, Vector3 position, float time, out Vector3 vel)
    {
        vel = initial + GetAcceleration() * time;
        return position + initial * time + 0.5f * GetAcceleration() * time * time;
    }

    protected void DrawLineRenderer(Vector3 initial, Vector3 position, int bounces = 1)
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
    
    private Vector3 GetBounceVelocity(Vector3 vel)
    {
        Vector3 bounceVelocity = vel;
        bounceVelocity.y *= -1f * BOUNCE_DECAY;
        return bounceVelocity;
    }
    
    protected virtual void OnBounce(BounceInfo bounceInfo)
    {
    }

    public void Launch(Vector3 target, float height, TeamSide teamSide = TeamSide.None, HitModifier hitModifier = null, bool serve = false)
    {
        if (Launch(target, height, teamSide, hitModifier, serve, out Vector3 vel))
        {
            ApplyVelocity(vel);
        }
    }

    public void ForceSetPossession(TeamSide teamSide)
    {
        possession = teamSide;
    }
    
    protected bool Launch(Vector3 target, float height, TeamSide teamSide, HitModifier hitModifier, bool serve, out Vector3 vel)
    {
        HitModifier modifier = hitModifier;
        if (hitModifier == null)
            modifier = new HitModifier();
        acceleration = modifier.ModifyAcceleration(GetAcceleration());
        if (ChaosPongHelper.CalculateLaunchVelocity(transform.position, target, height, GetAcceleration(), out Vector3 launchVelocity))
        {
            HitInfo hitInfo = new(serve, transform.position, launchVelocity, teamSide);
            possession = teamSide;
            hits.Add(hitInfo);
            vel = launchVelocity;
            return true;
        }

        acceleration = Vector3.zero;
        vel = launchVelocity;
        return false;
    }

#if UNITY_EDITOR
    [BoxGroup("Editor"), Button]
    private void SaveHitInfo()
    {
        PhysicsDebugger physicsDebugger = UnityEditor.AssetDatabase.LoadAssetAtPath<PhysicsDebugger>("Assets/Scriptable Objects/Physics Debugger.asset");
        physicsDebugger.SaveHits(hits);
    }
#endif
}
