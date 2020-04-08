using UnityEngine;
using UnityEngine.UI;

public class Rifle : Equipment {

    public GameObject bulletPrefab;
    private float currentReloadTime = 0;
    private const float MAX_RELOAD_TIME = .1f;
    private const float BULLET_SPEED = 10.0f;
    private const float BULLET_RANGE = 100.0f;

    void Start() {
        this.maxAmmoCount = 50;
        this.currentAmmoCount = this.maxAmmoCount;
        this.id = "Rifle";
    }

    public override void OnActivate() {
        if (this.currentAmmoCount > 0) {
            if (this.currentReloadTime <= 0) {

                Vector3 bulletDirection = transform.rotation * Vector3.forward;
                Vector3 position = this.transform.GetChild(0).position + bulletDirection * .2f;

                GameObject bulletClone = Instantiate(bulletPrefab, position, this.transform.rotation) as GameObject;
                bulletClone.transform.localScale = new Vector3(0.05f, .05f, .05f);
                Bullet bullet = bulletClone.GetComponent<Bullet>();
                bullet.Init(bulletDirection, BULLET_SPEED, BULLET_RANGE);
                bullet.damageValue = 8;

                this.currentReloadTime = MAX_RELOAD_TIME;
                if (ownerEntity.transform.tag != "Entity") {
                    this.currentAmmoCount--;
                }
            }
        }
    }

    void Update() {
        if (!(this.currentReloadTime <= 0)) {
            this.currentReloadTime -= Time.deltaTime;
        }
    }
}
