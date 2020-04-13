using UnityEngine;
using System.Collections.Generic;

public class LaunchPad : MonoBehaviour
{

	public GameObject explosion;
	public float explosiveForce = 25;

	void Explode(Rigidbody rb)
	{
		Vector3 direction = transform.rotation * Vector3.up;
		print(direction);
		rb.AddForce(direction * explosiveForce, ForceMode.Impulse);
		GameObject explode = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
		ParticleSystem parts = explode.GetComponent<ParticleSystem>();
		float totalDuration = parts.main.duration + parts.main.startLifetime.constant;
		Destroy(explode, totalDuration);
	}

	void OnTriggerEnter(Collider other)
	{
		Rigidbody rb = other.GetComponent<Rigidbody>();

		if (rb != null)
		{
			Explode(rb);
		}
	}
}
