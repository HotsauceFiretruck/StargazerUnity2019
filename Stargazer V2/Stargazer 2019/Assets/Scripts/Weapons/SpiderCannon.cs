using UnityEngine;

public class SpiderCannon : Equipment {

	public GameObject bulletPrefab;
	private float currentReloadTime = 0;
	private const float MAX_RELOAD_TIME = .1f;
	private const float BULLET_SPEED = 20.0f;
	private const float BULLET_RANGE = 100.0f;

	private const float ROTATE_SPEED = 1.5f; //Rotations per Second

	private Transform mainBarrel;

	void Start() {
		this.maxAmmoCount = 10000;
		this.currentAmmoCount = this.maxAmmoCount;
		this.id = "SpiderCannon";

		mainBarrel = this.transform.GetChild(0);
	}

	public override void OnActivate() {
		if (this.currentAmmoCount > 0) {
			if (this.currentReloadTime <= 0) {
				for (int i = 0; i < 5; i++) {
					Vector3 bulletDirection = transform.rotation * Vector3.forward;
					Vector3 position = mainBarrel.GetChild(i).GetChild(0).position + bulletDirection * .2f;

					GameObject bulletClone = Instantiate(bulletPrefab, position, this.transform.rotation) as GameObject;
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
		mainBarrel.Rotate(Vector3.up, ROTATE_SPEED * 360 * Time.deltaTime);
	}

	void Update() {
		if (!(this.currentReloadTime <= 0)) {
			this.currentReloadTime -= Time.deltaTime;
		}
	}
}
