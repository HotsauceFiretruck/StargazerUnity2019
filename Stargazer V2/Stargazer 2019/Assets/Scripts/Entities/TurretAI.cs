using UnityEngine;

class TurretAI : MonoBehaviour
{
    private bool activate = false;
    private readonly float maxReloadTime = .1f;
    private float currentReloadTime = 0;

    public GameObject ammoType;
    private readonly float ammoRange = 100;
    private readonly float ammoSpeed = 30;
    private readonly float damageModifier = 1;
    private float maxHealth;

    private Transform exit;
    public Transform targetRef;
    private Transform center;

    private void Start()
    {
        maxHealth = GetComponent<Health>().entityHealth;

        center = transform.GetChild(0);
        exit = center.GetChild(0);
    }

    public void ItsHighNoon()
    {
        activate = true;
        MeshRenderer r = center.GetChild(1).GetComponent<MeshRenderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        r.material.color = Color.red;
    }

    private void Deactivate()
    {
        activate = false;
        MeshRenderer r = center.GetChild(1).GetComponent<MeshRenderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        r.material.color = Color.white;
    }

    private void SoAnywaysIStartBlasting()
    {
        //Turn towards target
        Quaternion q = Quaternion.LookRotation(targetRef.position - center.position);

        center.rotation = Quaternion.Slerp(center.rotation, q, 75 * Time.deltaTime);

        //Blasting
        if (currentReloadTime <= 0)
        {
            Vector3 bulletDirection = center.rotation * Vector3.forward;
            Vector3 position = exit.position + bulletDirection * .2f;

            GameObject ammoClone = Instantiate(ammoType, position, center.rotation);
            Bullet bullet = ammoClone.GetComponent<Bullet>();
            bullet.Init(bulletDirection, ammoSpeed, ammoRange);
            bullet.SetDamageModifier(damageModifier);

            currentReloadTime = maxReloadTime;
        }
    }

    private void Update()
    {
        if (targetRef != null)
        {
            if (activate)
            {
                SoAnywaysIStartBlasting();

                if (Vector3.Distance(targetRef.position, transform.position) >= 55)
                {
                    Deactivate();
                }

            } else if (GetComponent<Health>().entityHealth != maxHealth || Vector3.Distance(targetRef.position, transform.position) <= 10)
            {
                Transform parent = transform.parent;
                if (parent != null)
                {
                    TurretAI[] turrets = parent.GetComponentsInChildren<TurretAI>();
                    foreach( TurretAI t in turrets)
                    {
                        t.ItsHighNoon();
                    }
                }
            }

            if (!(currentReloadTime <= 0))
            {
                currentReloadTime -= Time.deltaTime;
            }
        }
    }
}
