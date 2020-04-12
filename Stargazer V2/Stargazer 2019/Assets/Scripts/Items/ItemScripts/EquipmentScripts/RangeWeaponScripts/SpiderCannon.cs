using UnityEngine;

public class SpiderCannon : RangeWeapon
{
    private Transform mainBarrel;
    private int numberOfBarrels;

    SpiderCannonData data;
    float time = 0;

    void Awake()
    {
        //Create a clone of itemData
        itemData = Instantiate(itemData);
    }

    void Start()
    {
        data = (SpiderCannonData)itemData;

        mainBarrel = this.transform.GetChild(0);
        numberOfBarrels = mainBarrel.childCount - 1;

        time = 0;
    }

    public override void Activate()
    {
        if (data.currentAmmoCount > 0)
        {
            if (data.currentReloadTime <= 0)
            {
                for (int i = 0; i < numberOfBarrels; i++)
                {
                    Vector3 bulletDirection = transform.rotation * Vector3.forward;
                    Vector3 position = mainBarrel.GetChild(i).GetChild(0).position + bulletDirection;

                    GameObject bulletClone = Instantiate(data.ammoType, position, this.transform.rotation) as GameObject;
                    Bullet bullet = bulletClone.GetComponent<Bullet>();
                    bullet.Init(bulletDirection, data.ammoSpeed, data.ammoRange);

                    data.currentReloadTime = data.maxReloadTime;
                    if (data.ownerEntity.transform.tag == "Player")
                    {
                        data.currentAmmoCount--;
                    }
                }
            }
        }
        mainBarrel.Rotate(Vector3.up, data.rotateSpeed * 360 * Time.deltaTime);
    }

    public override void ActivateSecondary()
    {
        if (data.currentNumberOfCannonBalls > 0)
        {
            if (data.currentCannonReloadTime <= 0)
            {
                Vector3 cannonDirection = transform.rotation * Vector3.forward;
                Vector3 position = mainBarrel.GetChild(numberOfBarrels).GetChild(0).position + cannonDirection;

                GameObject cannonBallClone = (GameObject)Instantiate(data.cannonBallType, position, this.transform.rotation);
                CannonBall cannonBall = cannonBallClone.GetComponent<CannonBall>();
                cannonBall.Init(cannonDirection, data.cannonBallSpeed, data.cannonBallRange);

                data.currentCannonReloadTime = data.maxCannonReloadTime;
                if (data.ownerEntity.transform.tag == "Player")
                {
                    data.currentNumberOfCannonBalls--;
                }
            }
        }
    }

    public override void ActivateTertiary()
    {
        if (data.currentAmmoCount > 0)
        {
            if (data.currentBombardReloadTime <= 0)
            {
                for (int i = 0; i < numberOfBarrels; i++)
                {
                    Transform barrel = mainBarrel.GetChild(i).GetChild(0);

                    Vector3 bulletDirection = barrel.position - mainBarrel.position;
                    Vector3 position = barrel.position + bulletDirection;

                    GameObject bulletClone = Instantiate(data.ammoType, position, this.transform.rotation) as GameObject;
                    bulletClone.GetComponent<Rigidbody>().useGravity = true;
                    Bullet bullet = bulletClone.GetComponent<Bullet>();
                    bullet.Init(bulletDirection, 5*Mathf.Sin(time) + 10, data.ammoRange);

                    data.currentBombardReloadTime = data.maxBombardReloadTime;
                    if (data.ownerEntity.transform.tag == "Player")
                    {
                        data.currentAmmoCount--;
                    }
                }
            }
        }
        mainBarrel.Rotate(Vector3.up, data.rotateSpeed * 360 * Time.deltaTime);
        time += Time.deltaTime;
    }

    void Update()
    {
        if (!(data.currentReloadTime <= 0))
        {
            data.currentReloadTime -= Time.deltaTime;
        }

        if (!(data.currentBombardReloadTime <= 0))
        {
            data.currentBombardReloadTime -= Time.deltaTime;
        }

        if (!(data.currentCannonReloadTime <= 0))
        {
            data.currentCannonReloadTime -= Time.deltaTime;
        }
    }
}
