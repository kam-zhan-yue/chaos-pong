using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public Pong pong;
    public Transform target;

    public float h = 25;

    [Button]
    private void Launch()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ChaosPongHelper.CalculateLaunchVelocity(pong.transform.position, target.transform.position, h,
                    out Vector3 velocity))
            {
                pong.ApplyVelocity(velocity);
            }
        }
    }
}
