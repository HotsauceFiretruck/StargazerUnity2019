using UnityEngine;

public class Sniper : RangeWeapon
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

    public override void OnEquipped()
    {
        data = (RangeWeaponData)itemData;
        data.ownerEntity.gameObject.AddComponent<ScopeControl>();
    }

    public override void OnUnequipped()
    {
        ScopeControl scope = data.ownerEntity.gameObject.GetComponent<ScopeControl>();
        scope.Reset();
        Destroy(scope);
    }

    public override void Activate()
    {
        if (data.currentAmmoCount > 0)
        {
            if (data.currentReloadTime <= 0)
            {

                Vector3 bulletDirection = transform.rotation * Vector3.forward;
                Vector3 position = this.transform.GetChild(0).position + bulletDirection * .2f;

                GameObject bulletClone = (GameObject)Instantiate(data.ammoType, position, this.transform.rotation) as GameObject;
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
