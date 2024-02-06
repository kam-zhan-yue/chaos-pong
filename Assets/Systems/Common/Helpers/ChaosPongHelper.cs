using UnityEngine;

public static class ChaosPongHelper
{
    public static readonly LayerMask BlueSide = LayerMask.NameToLayer("BlueSide");
    public static readonly LayerMask RedSide = LayerMask.NameToLayer("RedSide");
    public static readonly Color Blue = Color.blue;
    public static readonly Color Red = Color.red;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="start">Position of the start</param>
    /// <param name="target">Position of the target</param>
    /// <param name="h">Desired max height of the projectile's trajectory</param>
    /// <param name="launchVelocity">The required launch velocity to reach the target</param>
    /// <param name="maxSpeed">Max XZ-Speed for Smash</param>
    /// <returns></returns>
    public static bool CalculateLaunchVelocity(Vector3 start, Vector3 target, float h, out Vector3 launchVelocity, float maxSpeed = 25f)
    {
        float height = h - start.y;
        float difference = target.y - start.y;
        bool valid = height >= difference;
        if (!valid)
        {
            launchVelocity = Vector3.zero;
            return false;
        }

        //If the height is under 0, then smash the ball with its max speed limit
        if (height < 0)
        {
            launchVelocity = CalculateSmashVelocity(start, target, maxSpeed);
            return true;
        }
        
        float gravity = Physics.gravity.magnitude * -1f;
        float displacementY = target.y - start.y;
        Vector3 displacementXZ = new(target.x - start.x, 0f, target.z - start.z);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2f * gravity * height);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity));
        launchVelocity = velocityXZ + velocityY * -Mathf.Sign(gravity);
        return true;
    }

    private static Vector3 CalculateSmashVelocity(Vector3 start, Vector3 target, float maxSpeed)
    {
        //Get the total elapsed time in the XZ direction
        Vector3 displacementXZ = new(target.x - start.x, 0f, target.z - start.z);
        float time = displacementXZ.magnitude / maxSpeed;
        
        //Reverse calculate y-velocity from s = ut+1/2at^2
        float displacementY = target.y - start.y;
        float gravity = Physics.gravity.magnitude * -1f;
        float velocityY = displacementY / time - 0.5f * gravity * time;
        return new Vector3(displacementXZ.x / time, velocityY, displacementXZ.z / time);
    }

    /// <summary>
    /// Gets a time that a projectile will take to hit a moving target from a moving shooter with a certain speed
    /// </summary>
    /// <param name="pShooter">Position of the shooter</param>
    /// <param name="vShooter">Velocity of the shooter</param>
    /// <param name="aShooter">Acceleration of the shooter</param>
    /// <param name="pTarget">Position of the target</param>
    /// <param name="vTarget">Velocity of the target</param>
    /// <param name="aTarget">Acceleration of the target</param>
    /// <param name="speed">Speed of the projectile</param>
    /// <returns></returns>
    public static float GetTimeToTarget(Vector3 pShooter, Vector3 vShooter, Vector3 aShooter, Vector3 pTarget, Vector3 vTarget,
        Vector3 aTarget, float speed)
    {
        //Get the position, velocity, and acceleration from the frame of reference of the target
        Vector3 p = pTarget - pShooter;
        Vector3 v = vTarget - vShooter;
        Vector3 a = aTarget - aShooter;

        float t4 = (a.x*a.x + a.y*a.y + a.z*a.z) / 4;
        float t3 = a.x*v.x + a.y*v.y + a.z*v.z;
        float t2 = v.x*v.x + p.x*a.x + v.y*v.y + p.y*a.y + v.z*v.z + p.z*a.z - speed*speed;
        float t1 = 2 * (p.x*v.x + p.y*v.y + p.z*v.z);
        float t0 = p.x*p.x + p.y*p.y + p.z*p.z;
        return 0f;
    }

    /// <summary>
    /// Gets a time that a projectile will take to hit a moving target from a moving shooter with a certain speed
    /// </summary>
    /// <param name="pShooter">Position of the shooter</param>
    /// <param name="vShooter">Velocity of the shooter</param>
    /// <param name="aShooter">Acceleration of the shooter</param>
    /// <param name="pTarget">Position of the target</param>
    /// <param name="vTarget">Velocity of the target</param>
    /// <param name="aTarget">Acceleration of the target</param>
    /// <param name="speed">Speed of the projectile</param>
    /// <returns></returns>
    public static float GetTimeToTarget(Vector3 pShooter, Vector3 pTarget, float speed)
    {
        //Get the position, velocity, and acceleration from the frame of reference of the target
        Vector3 p = pTarget - pShooter;

        float t2 = -speed*speed;
        float t0 = p.x*p.x + p.y*p.y + p.z*p.z;
        return 0f;
    }
    private static float[] FindQuarticRoots(float a, float b, float c, float d, float e)
    {
        // Your implementation of Newton's method or another numerical method goes here
        // Newton's method requires an initial guess for each root
        float[] initialGuesses = { 0.0f, 0.0f, 0.0f, 0.0f }; // You might need to adjust these

        // Specify the tolerance for convergence
        float tolerance = 0.0001f;

        // Maximum number of iterations to prevent infinite loop
        int maxIterations = 1000;

        float[] roots = new float[4];

        for (int i = 0; i < 4; i++)
        {
            roots[i] = NewtonsMethod(a, b, c, d, e, initialGuesses[i], tolerance, maxIterations);
        }

        return roots;
    }

    private static float NewtonsMethod(float a, float b, float c, float d, float e, float x0, float tolerance, int maxIterations)
    {
        float x = x0;

        for (int i = 0; i < maxIterations; i++)
        {
            float fx = a * x * x * x * x + b * x * x * x + c * x * x + d * x + e;
            float dfx = 4 * a * x * x * x + 3 * b * x * x + 2 * c * x + d;

            x = x - fx / dfx;

            if (Mathf.Abs(fx) < tolerance)
            {
                // Convergence achieved
                return x;
            }
        }

        // Return the last approximation even if the method did not converge
        return x;
    }
}