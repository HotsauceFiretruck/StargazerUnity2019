using UnityEngine;

class SteeringBehavior
{

    //Returns the seek velocity
    private static Vector2 Seek(Vector2 targetPos, Vector3 currentPos, Vector2 currentVel, float maxSpeed)
    {
        Vector2 currentPos2D = new Vector2(currentPos.x, currentPos.z);
        Vector2 desiredVelocity = (targetPos - currentPos2D).normalized * maxSpeed;
        return desiredVelocity - currentVel;
    }

    private static Vector2 Avoidance(Vector3 currentPos, Vector2 currentVel, float maxSpeed, float maxAvoidForce)
    {
        float distAhead = currentVel.magnitude / maxSpeed;
        Vector2 currentPos2D = new Vector2(currentPos.x, currentPos.z);

        RaycastHit hitInfo;
        Vector2 dir2D = currentVel.normalized;
        Vector2 ahead = currentPos2D + dir2D * distAhead;

        Vector2 avoidForce = Vector2.zero;

        Debug.DrawRay(currentPos, new Vector3(dir2D.x, 0, dir2D.y) * distAhead * 5, Color.red);
        if (Physics.Raycast(currentPos, new Vector3(dir2D.x, 0, dir2D.y), out hitInfo, distAhead * 5))
        {
            if (hitInfo.transform.tag == "Entity")
            {
                avoidForce.x = ahead.x - hitInfo.transform.position.x;
                avoidForce.y = ahead.y - hitInfo.transform.position.z;
                avoidForce = avoidForce.normalized * maxAvoidForce;
            }
        }
        return avoidForce;
    }

    public static Vector2 GetVelocity(Vector2 targetPos, Vector3 currentPos, Vector2 currentVel, float maxSpeed, float maxTurnSpeed, float maxAvoidForce)
    {
        Vector2 seekForce = Seek(targetPos, currentPos, currentVel, maxSpeed);
        Vector2 avoidForce = Avoidance(currentPos, currentVel, maxSpeed, maxAvoidForce);

        Vector2 steering = seekForce + avoidForce;

        steering = Vector2.ClampMagnitude(steering, maxTurnSpeed);

        return Vector2.ClampMagnitude(currentVel + steering, maxSpeed);
    }
}
