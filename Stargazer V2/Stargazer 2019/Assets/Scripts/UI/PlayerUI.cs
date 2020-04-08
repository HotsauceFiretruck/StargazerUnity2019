using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	public Entity player;
	public Health playerHealth;
	public Inventory playerInventory;

	private Text txtAmmo;
	private Text txtHealth;
	private Text txtInventory;

	private string inventoryDisp = "";

	public void Start() {
		txtAmmo = GameObject.Find("PlayerUI/AmmoCounter").GetComponent<Text>();
		txtInventory = GameObject.Find("PlayerUI/Inventory").GetComponent<Text>();
		txtHealth = GameObject.Find("PlayerUI/HealthCounter").GetComponent<Text>();
	}

	public void Ammo() {
		if (player.equipment != null) {
			txtAmmo.text = "Ammo: " + player.equipment.GetAmmoCount();
		}
		else {
			txtAmmo.text = "No weapon selected";
		}
	}

	public void Health() {
		if (playerHealth != null) {
			txtHealth.text = "Health: " + playerHealth.entityHealth;
		}
	}

	public void Inventory() {
		if (playerInventory != null && playerInventory.isUpdateSyncUI) {
			inventoryDisp = "";
			for (int i = 0; i < playerInventory.maxItems; i++) {
				if (playerInventory.items[i] != null)
					inventoryDisp += i + 1 + ": " + playerInventory.items[i].GetComponent<Equipment>().id + " | ";
			}
			playerInventory.isUpdateSyncUI = false;
		}
		txtInventory.text = "Inventory: " + inventoryDisp;
	}

	public void Update() {
		Ammo();
		Health();
		Inventory();
	}
}
