using System.Collections;
using UnityEngine;

public class MovementAI : MonoBehaviour
{

    public float turnDistance = 1;
    public float maxAvoidForce = 10.0f;
    const float pathUpdateMoveThreshold = 1f;
    const float minPathUpdateTime = 3f;
    private Vector3 targetPosition;
    private Entity parent;

    private bool isUpdatePathStarted;
    Path path;

    void Start()
    {
        parent = GetComponent<Entity>();
        isUpdatePathStarted = false;
    }

    public void OnPathFound(Vector3[] waypoints, bool success)
    {
        if (success)
        {
            this.path = new Path(waypoints, transform.position, turnDistance);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    public void NavigateTo(Vector3 position)
    {
        targetPosition = position;
        if (!isUpdatePathStarted)
        {
            isUpdatePathStarted = true;
            StartCoroutine(UpdatePath());
        }
    }

    public void EndNavigation()
    {
        //If update path haven't end yet, end it.
        if (isUpdatePathStarted)
        {
            StopAllCoroutines();
            isUpdatePathStarted = false;
            parent.currentSpeed = 0.0f;
        }
    }

    IEnumerator UpdatePath()
    {

        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }

        PathRequestManager.RequestPath(new PathRequest(transform.position, targetPosition, OnPathFound));
        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = targetPosition;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if ((targetPosition - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, targetPosition, OnPathFound));
                targetPosOld = targetPosition;
            }
        }
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        //transform.LookAt(path.lookPoints[0]);

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    parent.currentSpeed = 0.0f;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {

                Vector3 targetPosition = path.lookPoints[pathIndex];
                Vector2 velocity2D = SteeringBehavior.GetVelocity(new Vector2(targetPosition.x, targetPosition.z),
                                            transform.position, parent.velocity, parent.maxSpeed, parent.turnSpeed, maxAvoidForce);
                parent.currentSpeed = velocity2D.magnitude;
                velocity2D.Normalize();

                Quaternion currentRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(velocity2D.x, 0, velocity2D.y)), Time.deltaTime * parent.currentSpeed);

                parent.direction = currentRotation.eulerAngles;

                //sin == x and cos == y ??? they are flipped but works somehow. 
                float dx = Mathf.Sin(parent.direction.y * Mathf.Deg2Rad);
                float dy = Mathf.Cos(parent.direction.y * Mathf.Deg2Rad);

                parent.velocity = parent.currentSpeed * new Vector2(dx, dy);
            }

            yield return null;
        }
    }

    // public void OnDrawGizmos() {
    // 	if (path != null) {
    // 		path.DrawWithGizmos ();
    // 	}
    // }
}
