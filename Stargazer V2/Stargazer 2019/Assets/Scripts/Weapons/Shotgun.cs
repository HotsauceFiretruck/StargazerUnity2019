using UnityEngine;
using UnityEngine.UI;

public class Shotgun : Equipment {

    public GameObject bulletPrefab;
    private float currentReloadTime = 0;
    private float timeBetweenTwoShots = .2f;
    private float shotsFired = 0;
    private const float MAX_RELOAD_TIME = 2.5f;
    private const float BULLET_SPEED = 10.0f;
    private const float BULLET_RANGE = 100.0f;
    private const int BULLET_PER_SHOT = 10;

    void Start() {
        this.maxAmmoCount = 10;
        this.currentAmmoCount = this.maxAmmoCount;
        this.id = "Shotgun";
    }

    public override void OnActivate() {
        if (this.currentAmmoCount > 0) {
            if (this.currentReloadTime <= 0) {

                for (int i = 0; i < BULLET_PER_SHOT; i++) {
                    int rnd = Random.Range(-5, 5);
                    int rnd2 = Random.Range(-5, 5);
                    Vector3 bulletDirection = Quaternion.Euler(this.transform.eulerAngles + new Vector3(rnd, rnd2, 0)) * Vector3.forward;

                    Vector3 position = this.transform.GetChild(0).position + bulletDirection * .1f * i;
                    GameObject bulletClone = Instantiate(bulletPrefab, position, this.transform.rotation);
                    bulletClone.transform.localScale = new Vector3(.05f, .05f, .05f);
                    Bullet bullet = bulletClone.GetComponent<Bullet>();
                    bullet.Init(bulletDirection, BULLET_SPEED, BULLET_RANGE);
                    bullet.damageValue = 5;
                }

                if (shotsFired == 0) {
                    this.currentReloadTime = timeBetweenTwoShots;
                    shotsFired = 1;
                }
                else if (shotsFired == 1) {
                    this.currentReloadTime = MAX_RELOAD_TIME;
                    shotsFired = 0;
                }

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
