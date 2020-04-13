using UnityEngine;
public class MovementControl : MonoBehaviour
{
    Entity ownerEntity;
    Collider entityCollider;
    Rigidbody entityBody;

    public LayerMask groundLayer;
    public LayerMask wallLayer;
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

    void Jump()
    {
        bool isGrounded = Physics.CheckSphere(transform.position - entityCollider.bounds.extents.y * Vector3.up, 0.2f, groundLayer, QueryTriggerInteraction.Ignore);
        if (!isGrounded) isGrounded = Physics.CheckSphere(transform.position - entityCollider.bounds.extents.y * Vector3.up, 0.2f, wallLayer, QueryTriggerInteraction.Ignore);

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
        if (ownerEntity.velocity != Vector2.zero)
        {
            entityBody.MovePosition(entityBody.position + new Vector3(ownerEntity.velocity.x, 0, ownerEntity.velocity.y) * Time.deltaTime);
            ownerEntity.position = transform.position;
        } else
        {
            entityBody.velocity = new Vector3(0, entityBody.velocity.y, 0);
        }
    }
}
