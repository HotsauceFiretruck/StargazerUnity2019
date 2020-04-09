using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAI : Entity
{

	[SerializeField]
	private Transform targetRef;
	private bool targetDetected;
	private bool targetCanBeSeen;
	private bool commitT = false;

	public GameObject explosion;
	public float detectionRange = 50;
	public float explodeRange = 4;
	public float explosiveRadius = 10;
	public float explosiveForce = 25;
	public int damage = 20;

	private MovementAI movementAI;

	void Start()
	{
		this.direction = this.transform.eulerAngles;
		this.position = this.transform.position;
		this.currentSpeed = 0.0f;

		targetDetected = targetCanBeSeen = false;

		movementAI = GetComponent<MovementAI>();
	}

	void CheckStatus()
	{

		Vector3 dir = targetRef.position - transform.position;
		RaycastHit hitInfoCenter;
		float distApart = .3f;
		float x;
		float z;
		float radians = Mathf.Atan2(dir.z, dir.x);

		RaycastHit hitInfoLeft;
		x = distApart * Mathf.Cos(radians + Mathf.PI / 2);
		z = distApart * Mathf.Sin(radians + Mathf.PI / 2);
		Ray raySpreadLeft = new Ray(position + new Vector3(x, 0, z), dir);
		bool left = Physics.Raycast(raySpreadLeft, out hitInfoLeft);

		RaycastHit hitInfoRight;
		x = distApart * Mathf.Cos(radians - Mathf.PI / 2);
		z = distApart * Mathf.Sin(radians - Mathf.PI / 2);
		Ray raySpreadRight = new Ray(position + new Vector3(x, 0, z), dir);
		bool right = Physics.Raycast(raySpreadRight, out hitInfoRight);

		Ray rayCanBeSeen = new Ray(position, dir);
		bool center = Physics.Raycast(rayCanBeSeen, out hitInfoCenter);

		if (left && right && center)
		{
			if (hitInfoCenter.transform.tag == "Player" &&
				hitInfoLeft.transform.tag == "Player" &&
				hitInfoRight.transform.tag == "Player")
			{
				targetCanBeSeen = true;
			}
			else
			{
				targetCanBeSeen = false;
			}
		}
		else
		{
			targetCanBeSeen = false;
		}

		//if target is within the spotting range then activate target spotted
		if (Vector3.Distance(targetRef.position, transform.position) >= detectionRange)
		{
			targetDetected = false;
		}
		else
		{
			targetDetected = true;
		}

		if (Vector3.Distance(targetRef.position, transform.position) >= explodeRange)
		{
			commitT = false;
		}
		else
		{
			commitT = true;
		}
	}

	void Explode()
	{
		GameObject explode = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
		ParticleSystem parts = explode.GetComponent<ParticleSystem>();
		float totalDuration = parts.main.duration + parts.main.startLifetime.constant;
		Destroy(explode, totalDuration);

		Collider[] colliders = Physics.OverlapSphere(transform.position, explosiveRadius);
		for (int i = 0; i < colliders.Length; i++)
		{
			Transform otherTransform = colliders[i].transform;
			if (otherTransform.tag != "Entity" && otherTransform.tag != "Player") continue;
			if (otherTransform == transform) continue;

			Rigidbody body = otherTransform.GetComponent<Rigidbody>();
			Health health = otherTransform.GetComponent<Health>();
			if (body != null)
			{
				body.AddExplosionForce(explosiveForce, transform.position, explosiveRadius, 5, ForceMode.Impulse);
			}
			if (health != null)
			{
				health.ChangeHealthBy(damage);
			}
		}
	}

	public override void Death()
	{
		Explode();
		Destroy(this.gameObject);
	}

	void AIBehavior()
	{
		//If target is spotted...
		if (targetDetected)
		{
			if (movementAI != null)
			{
				if (!targetCanBeSeen || !commitT)
				{
					movementAI.NavigateTo(targetRef.position);
				}
				else
				{
					movementAI.EndNavigation();
				}
			}

			if (commitT)
			{
				Death();
			}
		}
	}

	void Update()
	{
		if (targetRef != null)
		{
			CheckStatus();
			AIBehavior();
		}
	}

	private void FixedUpdate()
	{
		if (this.currentSpeed != 0.0f)
		{
			transform.Translate(Vector3.forward * this.currentSpeed * Time.fixedDeltaTime);
			this.position = transform.position;
		}

		transform.eulerAngles = this.direction;
	}
}
