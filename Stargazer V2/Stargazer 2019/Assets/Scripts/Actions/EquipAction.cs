using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipAction : MonoBehaviour {
	//Holding angle is in degrees
	public float holdingDistance;
	public float droppingDistance;

	private Entity ownerEntity;

	void Awake() {
		ownerEntity = GetComponent<Entity>();
	}

    public void OnEquip(Equipment item, Transform transform) {
        item.ownerEntity = ownerEntity;
        item.transform.parent = transform;
        ownerEntity.equipment = item;

        Rigidbody body = item.GetComponent<Rigidbody>();
        body.isKinematic = true;
        body.useGravity = false;
        body.detectCollisions = false;

        item.transform.localPosition = Vector3.forward * holdingDistance;

        item.transform.eulerAngles = transform.eulerAngles;
        item.transform.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    public void OnDrop(Equipment item) {
        //Player special rule: if drop then turn visible
        if (ownerEntity.gameObject.tag == "Player") {
            MeshRenderer[] renders = item.transform.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer r in renders) {
                r.enabled = true;
            }
        }

        item.transform.gameObject.layer = LayerMask.NameToLayer("Item");

        Rigidbody body = item.GetComponent<Rigidbody>();
        body.isKinematic = false;
        body.useGravity = true;
        body.detectCollisions = true;

        body.AddForce(item.transform.rotation * Vector3.forward * 4, ForceMode.Impulse);

        item.ownerEntity = null;
        item.transform.parent = null;
        ownerEntity.equipment = null;
    }
}
