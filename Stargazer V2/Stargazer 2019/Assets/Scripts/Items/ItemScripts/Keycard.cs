using UnityEngine;

public class Keycard : Item
{
    Transform orientation;

    private void Awake()
    {
        itemData = Instantiate(itemData);    
    }

    private void Start()
    {
        if (transform.parent != null)
        {
            orientation = transform.parent;
        }
    }

    public override void Activate()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(orientation.position, orientation.forward, out hitInfo, 5.0f))
        {
            Door door = hitInfo.transform.GetComponent<Door>();
            if (door != null)
            {
                door.Open();
            }
        }
    }

    public override void Deactivate()
    {
        orientation = null;
    }
}
