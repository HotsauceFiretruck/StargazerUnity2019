using UnityEngine;

public class MovementControl : MonoBehaviour
{
    Entity ownerEntity;
    Collider entityCollider;
    Rigidbody entityBody;

    public LayerMask groundLayer;
    public Transform orientation;
    public float jumpForce;

    void Start()
    {
        ownerEntity = GetComponent<Entity>();
        entityCollider = GetComponent<Collider>();
        entityBody = GetComponent<Rigidbody>();
    }

    void Move()
    {
        Vector3 input = new Vector3(-Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        if (input != Vector3.zero)
        {
            float inputRadians = Mathf.Atan2(input.normalized.z, input.normalized.x);
            float facingDirection = Mathf.Deg2Rad * orientation.transform.eulerAngles.y;
            float moveDirection = inputRadians - (Mathf.PI / 2) + facingDirection;
            Vector2 direction = new Vector2(Mathf.Sin(moveDirection), Mathf.Cos(moveDirection));
            ownerEntity.velocity = direction * ownerEntity.currentSpeed;
        }
        else
        {
            ownerEntity.velocity = Vector2.zero;
        }
    }

    public void ApplyForceToReachVelocity(Rigidbody rigidbody, Vector2 _velocity, float force = 1, ForceMode mode = ForceMode.VelocityChange)
    {
        if (force == 0 || _velocity.magnitude == 0)
            return;

        _velocity = _velocity + _velocity.normalized * 0.2f * rigidbody.drag;

        //force = 1 => need 1 s to reach velocity (if mass is 1) => force can be max 1 / Time.fixedDeltaTime
        force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime, rigidbody.mass / Time.fixedDeltaTime);

        if (rigidbody.velocity.magnitude == 0)
        {
            rigidbody.AddForce(new Vector3(_velocity.x, 0, _velocity.y) * force, mode);
        }
        else
        {
            Vector2 velocityProjectedToTarget = (_velocity.normalized * Vector2.Dot(_velocity, new Vector2(rigidbody.velocity.x, rigidbody.velocity.z)) / _velocity.magnitude);
            Vector2 dV = _velocity - velocityProjectedToTarget;
            rigidbody.AddForce(new Vector3(dV.x, 0, dV.y) * force, mode);
        }

    }

    void Jump()
    {
        bool isGrounded = Physics.CheckSphere(transform.position - entityCollider.bounds.extents.y * Vector3.up, 0.2f, groundLayer, QueryTriggerInteraction.Ignore);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            entityBody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }

        if (!isGrounded)
        {
            ownerEntity.currentSpeed = ownerEntity.maxSpeed * .7f;
        }
        else
        {
            ownerEntity.currentSpeed = ownerEntity.maxSpeed;
        }
    }

    void Update()
    {
        Move();
        Jump();
    }

    void FixedUpdate()
    {
        ApplyForceToReachVelocity(entityBody, ownerEntity.velocity, 0.1f, ForceMode.VelocityChange);
        ownerEntity.position = transform.position;
    }
}
