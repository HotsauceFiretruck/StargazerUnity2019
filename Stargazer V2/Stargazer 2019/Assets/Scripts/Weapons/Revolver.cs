using UnityEngine;
using UnityEngine.UI;

public class Revolver : Equipment {

    public GameObject bulletPrefab;
    private float currentReloadTime = 0;
    private const float MAX_RELOAD_TIME = .6f;
    private const float BULLET_SPEED = 10f;
    private const float BULLET_RANGE = 100.0f;

    void Start() {
        this.maxAmmoCount = 20;
        this.currentAmmoCount = this.maxAmmoCount;
        this.id = "Revolver";
    }

    public override void OnActivate() {
        if (this.currentAmmoCount > 0) {
            if (this.currentReloadTime <= 0) {

                Vector3 bulletDirection = transform.rotation * Vector3.forward;
                Vector3 position = this.transform.GetChild(0).position + bulletDirection * .2f;

                GameObject bulletClone = Instantiate(bulletPrefab, position, this.transform.rotation) as GameObject;
                Bullet bullet = bulletClone.GetComponent<Bullet>();
                bullet.Init(bulletDirection, BULLET_SPEED, BULLET_RANGE);
                bullet.damageValue = 10;

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
