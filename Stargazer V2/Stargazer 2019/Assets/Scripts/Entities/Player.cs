using UnityEngine;

public class Player : Entity
{
    public Camera playerView;

    private EquipAction itemAction;
    public GameObject weaponPrefab;

    void Start()
    {
        playerView.enabled = true;
        playerView.transform.position = transform.position + Vector3.up * .5f;
        playerView.transform.eulerAngles = this.direction = transform.eulerAngles;
        playerView.fieldOfView = 95f;

        itemAction = GetComponent<EquipAction>();

        this.currentSpeed = this.maxSpeed;

        if (weaponPrefab != null)
        {
            GameObject weaponObject = (GameObject)Instantiate(weaponPrefab);
            RangeWeapon weapon = weaponObject.GetComponent<RangeWeapon>();
            itemAction.EquipItem(weapon, playerView.transform);
        }
        Cursor.lockState = CursorLockMode.Locked;
        position = transform.position;
    }

    public override void Death()
    {
        playerView.transform.parent = null;
        print("YOU DIED!");
        Destroy(this.gameObject);
    }

}
