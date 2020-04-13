using UnityEngine;

public class Shotgun : RangeWeapon
{
    private float timeBetweenTwoShots = .2f;
    private float shotsFired = 0;
    private const int BULLET_PER_SHOT = 5;

    RangeWeaponData data;

    void Awake()
    {
        //Create a clone of itemData
        itemData = Instantiate(itemData);
    }

    void Start()
    {
        data = (RangeWeaponData)itemData;
    }


    public override void Activate()
    {
        if (data.currentAmmoCount > 0)
        {
            if (data.currentReloadTime <= 0)
            {

                for (int i = 0; i < BULLET_PER_SHOT; i++)
                {
                    int rnd = Random.Range(-5, 5);
                    int rnd2 = Random.Range(-5, 5);
                    Vector3 bulletDirection = Quaternion.Euler(this.transform.eulerAngles + new Vector3(rnd, rnd2, 0)) * Vector3.forward;

                    Vector3 position = this.transform.GetChild(0).position + bulletDirection * .1f * i;
                    GameObject bulletClone = (GameObject)Instantiate(data.ammoType, position, this.transform.rotation);
                    bulletClone.transform.localScale = new Vector3(.5f, .5f, .5f);
                    Bullet bullet = bulletClone.GetComponent<Bullet>();
                    bullet.Init(bulletDirection, data.ammoSpeed, data.ammoRange);
                    bullet.SetDamageModifier(data.damageModifier);
                }

                if (shotsFired == 0)
                {
                    data.currentReloadTime = timeBetweenTwoShots;
                    shotsFired = 1;
                }
                else if (shotsFired == 1)
                {
                    data.currentReloadTime = data.maxReloadTime;
                    shotsFired = 0;
                }

                if (data.ownerEntity.transform.tag != "Entity")
                {
                    data.currentAmmoCount--;
                }
            }
        }
    }

    void Update()
    {
        if (!(data.currentReloadTime <= 0))
        {
            data.currentReloadTime -= Time.deltaTime;
        }
    }
}
