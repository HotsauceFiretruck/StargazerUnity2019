using UnityEngine;

public class OrientationControl : MonoBehaviour
{
    Entity ownerEntity;
    float yAngle = 0;

    public Transform orientation;

    void Start()
    {
        ownerEntity = GetComponent<Entity>();
    }

    void RotatePerspective()
    {
        float mouseX = Input.GetAxis("Mouse X") * ownerEntity.turnSpeed * 20 * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * ownerEntity.turnSpeed * 20 * Time.deltaTime;
        if (mouseX != 0 || mouseY != 0)
        {
            yAngle = Mathf.Clamp(yAngle - mouseY, -90f, 60f);
            orientation.localRotation = Quaternion.Euler(yAngle, orientation.localEulerAngles.y + mouseX, 0f);

            ownerEntity.direction = orientation.rotation * Vector3.forward;
        }
    }

    void Update()
    {
        RotatePerspective();
    }
}
