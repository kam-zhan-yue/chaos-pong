using System;
using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using MEC;
using Sirenix.OdinInspector;
using SuperMaxim.Messaging;
using UnityEngine;

public class Pong : Projectile
{
    //Private Variables
    private PongState _pongState = PongState.Idle;
    private PongModifier _pongModifier = new();

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Character character))
        {
            Debug.Log($"Collided with {character.gameObject.name}");
            if (_pongModifier.deadly)
            {
                //Score for the player hitting
                Score(possession);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Character character))
        {
            if (_pongModifier.deadly)
            {
                //Score for the player hitting
                Score(possession);
            }
        }
    }

    public void ResetModifier()
    {
        Debug.Log("Reset Modifier");
        _pongModifier.Reset();
    }

    public void SetModifier(PongModifier pongModifier)
    {
        Debug.Log($"Set Modifier Deadly: {pongModifier.deadly}");
        _pongModifier = pongModifier;
    }

    private void ApplyServe(Vector3 serveVelocity, float nextHeight, TeamSide targetSide)
    {
        Rigidbody.isKinematic = false;
        simulated = true;
        velocity = serveVelocity;
        Timing.KillCoroutines(bounceRoutine);
        bounceRoutine = Timing.RunCoroutine(SimulatedBounce(nextHeight, targetSide).CancelWith(gameObject));
    }

    private IEnumerator<float> SimulatedBounce(float height, TeamSide targetSide)
    {
        Vector3 initial = velocity;
        Vector3 position = transform.position;
        //Calculate the time to bounce (returns time to table or ground)
        float time = TimeToBounce(initial, position);
        DrawLineRenderer(initial, position, 2);
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

    protected override void OnBounce(BounceInfo bounceInfo)
    {
        base.OnBounce(bounceInfo);
        // Debug.Log($"State: {_pongState} Possession: {possession}");
        switch (_pongState)
        {
            //If serving, check whether can turn into returning
            case PongState.Serving:
                //If landed on the target side, change to returnable
                if (bounceInfo.teamSide == possession)
                {
                    _pongState = PongState.Returnable;
                }
                break;
            //If returning, check whether can turn into returnable
            case PongState.Returning:
                if (bounceInfo.teamSide == possession)
                {
                    _pongState = PongState.Returnable;
                }
                break;
        }
        
        //If bounced on none, award points accordingly
        if(bounceInfo.teamSide == TeamSide.None)
        {
            //Give the  point to the team who is unable to hit the ball
            Score(ChaosPongHelper.GetOppositeSide(possession));
        }
    }

    private void Score(TeamSide teamSide)
    {
        if (_pongState != PongState.Scored)
        {
            PublishScore(teamSide);
        }
    }

    private void PublishScore(TeamSide teamSide)
    {
        Debug.Log($"Publish Score {teamSide}");
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

    public void Toss()
    {
        Vector3 tossVelocity = Vector3.up * ChaosPongHelper.TOSS_SPEED;
        ApplyVelocity(tossVelocity);
    }

    public void Serve(TeamSide teamSide, float height)
    {
        //5th March 2024 - Use Return until serve logic is finalised
        Return(teamSide, height);
        
        //Find a point that can be served on
        // Vector3 point = ServiceLocator.Instance.Get<ITableService>().GetServePoint(teamSide, transform.position);
        // //Hit the ball to that point and pass in the next height
        // if (Launch(point, height, teamSide, null, true, out Vector3 vel))
        // {
        //     ApplyServe(vel, ChaosPongHelper.SERVE_BOUNCE_HEIGHT, ChaosPongHelper.GetOppositeSide(teamSide));
        // }
        //Set state to serving
        // _pongState = PongState.Serving;
    }

    public bool CanReturn(TeamSide teamSide)
    {
        return possession == teamSide && _pongState == PongState.Returnable || _pongState == PongState.Idle;
    }

    public void Return(TeamSide teamSide, float height, HitModifier hitModifier = null)
    {
        //Only return if the team has possession of the ball
        if (CanReturn(teamSide))
        {
            TeamSide opposite = ChaosPongHelper.GetOppositeSide(teamSide);
            LaunchAtSide(height, opposite, hitModifier);
            _pongState = PongState.Returning;
        }
    }

    [Button]
    private void LaunchAtSide(float height, TeamSide teamSide, HitModifier hitModifier = null, bool serve = false)
    {
        Vector3 point = TableService.GetRandomPoint(teamSide);
        Launch(point, height, teamSide, hitModifier, serve);
    }

    public void DebugHitInfo(HitInfo hitInfo)
    {
        transform.position = hitInfo.position;
        possession = hitInfo.teamSide;
        ApplyVelocity(hitInfo.velocity);
    }

    private IEnumerator<float> DestroyRoutine()
    {
        yield return Timing.WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
