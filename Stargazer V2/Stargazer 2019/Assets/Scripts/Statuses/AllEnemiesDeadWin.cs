using UnityEngine;

public class AllEnemiesDeadWin : MonoBehaviour
{
    public Transform enemiesGroup;
    public GameObject keyCard;
    public Transform orientation;

    // Update is called once per frame
    void Update()
    {
        if (enemiesGroup.childCount == 0)
        {
            print(orientation.position);
            print(orientation.position + orientation.forward);
            Instantiate(keyCard, orientation.position + Vector3.up * .5f + orientation.forward * 2, 
                Quaternion.Euler(orientation.eulerAngles + Vector3.up * 90));
            Destroy(this);
        }
    }
}
