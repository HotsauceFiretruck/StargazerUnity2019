using UnityEngine;
using UnityEngine.UI;

public class SMG : RangeWeapon
{

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
            int rnd = Random.Range(-1, 1);
            int rnd2 = Random.Range(-2, 2);

            if (data.currentReloadTime <= 0)
            {

                Vector3 bulletDirection = Quaternion.Euler(this.transform.eulerAngles + new Vector3(rnd, rnd2, 0)) * Vector3.forward;
                Vector3 position = this.transform.GetChild(0).position + bulletDirection * .2f;

                GameObject bulletClone = (GameObject)Instantiate(data.ammoType, position, this.transform.rotation) as GameObject;
                bulletClone.transform.localScale = new Vector3(0.5f, .5f, .5f);
                Bullet bullet = bulletClone.GetComponent<Bullet>();
                bullet.Init(bulletDirection, data.ammoSpeed, data.ammoRange);
                bullet.SetDamageModifier(data.damageModifier);

                data.currentReloadTime = data.maxReloadTime;
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
