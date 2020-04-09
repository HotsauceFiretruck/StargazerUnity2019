using UnityEngine;

public class RocketBoot : Equipment
{
    RocketBootData data;
    MovementControl entityMovement;
    Rigidbody entityBody;
    bool isEquipped = false;

    public Transform orientation;

    void Awake()
    {
        //Create a clone of itemData
        itemData = Instantiate(itemData);
    }

    void Start()
    {
        data = (RocketBootData)itemData;
    }

    public override void OnEquipped()
    {
        isEquipped = true;
        data = (RocketBootData)itemData;
        entityMovement = data.ownerEntity.GetComponent<MovementControl>();
        entityBody = data.ownerEntity.GetComponent<Rigidbody>();
        if (orientation == null) orientation = transform.parent;
    }

    public override void OnUnequipped()
    {
        isEquipped = false;
        entityMovement.enabled = true;
        if (orientation != null) orientation = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEquipped)
        {
            if (data.currentReloadTime <= 0)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (entityBody != null)
                    {
                        if (entityMovement != null)
                        {
                            //Disable entity movement component
                            entityMovement.enabled = false;
                        }

                        Vector3 input = new Vector3(-Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

                        if (input != Vector3.zero)
                        {
                            float inputRadians = Mathf.Atan2(input.normalized.z, input.normalized.x);
                            float facingDirection = Mathf.Deg2Rad * orientation.transform.eulerAngles.y;
                            float moveDirection = inputRadians - (Mathf.PI / 2) + facingDirection;

                            Vector2 direction = new Vector2(Mathf.Sin(moveDirection), Mathf.Cos(moveDirection));

                            entityBody.AddForce(new Vector3(direction.x, 0, direction.y) * data.boostForce, ForceMode.Impulse);

                            data.currentReloadTime = data.maxBoostReloadTime;
                        }

                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            entityBody.AddForce(Vector3.up * data.boostForce, ForceMode.Impulse);
                            data.currentReloadTime = data.maxBoostReloadTime;
                        }
                    }
                }
                else
                {
                    entityMovement.enabled = true;
                }
            }
            else
            {
                entityMovement.enabled = true;
            }
        }

        if (data.currentReloadTime > 0)
        {
            data.currentReloadTime -= Time.deltaTime;
            if (data.currentReloadTime < 0)
            {
                data.currentReloadTime = 0;
            }
        }
    }
}
