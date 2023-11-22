using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public float speed = 30;
    public Transform serveHolder;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // only let the local player control the racket.
        // don't control other player's rackets
        if (isLocalPlayer)
        {
            float vertical = Input.GetAxisRaw("Vertical");
            float horizontal = Input.GetAxisRaw("Horizontal");
            _rb.velocity = new Vector3(horizontal * speed * Time.fixedDeltaTime, 0f, vertical * speed * Time.fixedDeltaTime);
        }
    }

    public override void OnStartLocalPlayer()
    {
        
    }

    public override void OnStopLocalPlayer()
    {
        
    }
}
