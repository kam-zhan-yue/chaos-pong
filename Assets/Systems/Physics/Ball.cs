using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool _ghost;
    private Rigidbody _rigidbody;
    private int _blueSideCollisions = 0;
    private int _redSideCollisions = 0;
    private TeamSide _teamSide;

    public bool Valid
    {
        get
        {
            return _teamSide switch
            {
                TeamSide.None => true,
                TeamSide.Blue => _blueSideCollisions == 1 && _redSideCollisions >= 1,
                TeamSide.Red => _redSideCollisions == 1 && _blueSideCollisions >= 1,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == ChaosPongHelper.BlueSide)
            _blueSideCollisions++;
        else if (collision.gameObject.layer == ChaosPongHelper.RedSide)
            _redSideCollisions++;
    }

    private void ResetValues()
    {
        _blueSideCollisions = 0;
        _redSideCollisions = 0;
    }

    public void Serve(Vector3 velocity, bool isGhost, TeamSide teamSide = TeamSide.None)
    {
        _rigidbody.velocity = Vector3.zero;
        _ghost = isGhost;
        _rigidbody.AddForce(velocity, ForceMode.Impulse);
        _teamSide = teamSide;
    }

    public void Return(Vector3 velocity)
    {
        _rigidbody.AddForce(velocity, ForceMode.VelocityChange);
    }
}
