using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool _ghost;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public void Serve(Vector3 velocity, bool isGhost)
    {
        _rigidbody.velocity = Vector3.zero;
        _ghost = isGhost;
        _rigidbody.AddForce(velocity, ForceMode.Impulse);
    }

    public void Return(Vector3 velocity)
    {
        _rigidbody.AddForce(velocity, ForceMode.VelocityChange);
    }
}
