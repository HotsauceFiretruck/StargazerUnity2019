using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public int entityHealth = 10;

	public void SetHealth(int health) {
		this.entityHealth = health;
	}

	public void ChangeHealthBy(int healthValue) {
		this.entityHealth = Mathf.Max(entityHealth - healthValue, 0);
	}

	//The reason why I put check health here instead of change health by is to prevent "chain" death for bomber
	void Update() {
		if (this.entityHealth <= 0) {
			this.gameObject.GetComponent<Entity>().Death();
		}
	}
}