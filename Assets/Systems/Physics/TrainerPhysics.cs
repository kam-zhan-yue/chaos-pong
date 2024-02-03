using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerPhysics : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Ball ball))
        {
            
        }
    }
}
