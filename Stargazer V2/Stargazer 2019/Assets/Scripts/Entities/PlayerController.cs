using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Entity {

	public Camera playerView;
	private Collider playerCollider;
	private Rigidbody playerBody;

	private float scopedFOV = 30f;
	private float normalFOV = 95f;
	private float jumpHeight = 2.0f;
	private float yRotation = 0.0f;
	private bool isScoped = false;

	private EquipAction equipAction;
	private Inventory inventory;
	public GameObject weaponPrefab;
	public TimeManager timeManager;

	void Start() {
		playerView.enabled = true;
		playerView.transform.position = transform.position + Vector3.up * .5f;
		playerView.transform.eulerAngles = this.direction = transform.eulerAngles;
		playerView.fieldOfView = 95f;

		playerBody = GetComponent<Rigidbody>();
		playerCollider = GetComponent<Collider>();
		equipAction = GetComponent<EquipAction>();
		inventory = GetComponent<Inventory>();

		this.currentSpeed = this.maxSpeed;

		if (weaponPrefab != null) {
			Equipment weapon = Instantiate(weaponPrefab, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<Equipment>();
			equipAction.OnEquip(weapon, playerView.transform);
		}
		Cursor.lockState = CursorLockMode.Locked;
		position = transform.position;
	}

	void Update() {
		this.MoveControl();
		this.JumpControl();
		this.RotatePerspective();
		this.InteractionControl();
		this.InventoryControl();
		this.Scope();
		this.SlowMo();
	}

	void FixedUpdate() {
		if (this.velocity != Vector3.zero) {
			playerBody.MovePosition(playerBody.position + this.velocity * Time.deltaTime);
			position = transform.position;
		}
	}

	void MoveControl() {
		Vector3 input = new Vector3(-Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
		if (input != Vector3.zero) {
			float inputDirection = Mathf.Atan2(input.normalized.z, input.normalized.x);
			float facingDirection = Mathf.Deg2Rad * transform.eulerAngles.y;
			float moveDirection = inputDirection - (Mathf.PI / 2) + facingDirection;
			Vector3 direction = new Vector3(Mathf.Sin(moveDirection), 0.0f, Mathf.Cos(moveDirection));
			this.velocity = direction * currentSpeed;
		}
		else {
			this.velocity = Vector3.zero;
		}
	}

	void JumpControl() {
		bool isGrounded = Physics.CheckSphere(transform.position - playerCollider.bounds.extents.y * Vector3.up, 0.2f, this.groundLayer, QueryTriggerInteraction.Ignore);

		if (Input.GetButtonDown("Jump") && isGrounded) {
			playerBody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
		}

		if (!isGrounded) {
			currentSpeed = this.maxSpeed * .7f;
		}
		else {
			currentSpeed = this.maxSpeed;
		}
	}

	void InteractionControl() {

		if (Input.GetKeyDown(KeyCode.E)) {
			Ray ray = new Ray(playerView.transform.position, playerView.transform.forward);
			RaycastHit hitInfo;

			if (Physics.Raycast(ray, out hitInfo, 5, LayerMask.NameToLayer("Item"))) {
				if (hitInfo.transform.tag == "Equipment") {
					Equipment item = hitInfo.transform.GetComponent<Equipment>();
					if (inventory == null || this.equipment == null || !inventory.Store(hitInfo.transform.gameObject)) {
						if (this.equipment != null) {
							this.equipAction.OnDrop(this.equipment);
						}

						this.equipAction.OnEquip(item, playerView.transform);
					}
				}
			}
			else if (!Physics.Raycast(ray, out hitInfo, 3) && this.equipment != null) {
				this.equipAction.OnDrop(this.equipment);
			}
		}

		if (Input.GetMouseButton(0) && this.equipment != null) {
			this.equipment.OnActivate();
		}
	}

	void InventoryControl() {
		if (inventory != null) {
			for (int i = 1; i < inventory.maxItems + 1; i++) {
				if (Input.GetKeyDown("" + i)) {
					GameObject item = inventory.GetItem(i - 1);
					if (item != null) {
						if (this.equipment != null) {
							Equipment itemToStore = this.equipment;
							this.equipAction.OnDrop(this.equipment);
							inventory.Store(itemToStore.gameObject);
						}
						this.equipAction.OnEquip(item.GetComponent<Equipment>(), playerView.transform);
					}
				}
			}
		}
	}

	void RotatePerspective() {
		float mouseX = Input.GetAxis("Mouse X") * this.turnSpeed * 20 * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * this.turnSpeed * 20 * Time.deltaTime;

		yRotation = Mathf.Clamp(yRotation - mouseY, -90f, 60f);

		playerView.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
		transform.Rotate(Vector3.up * mouseX);

		this.direction = playerView.transform.rotation * Vector3.forward;
	}

	public override void Death() {
		playerView.transform.parent = null;
		print("YOU DIED!");
		Destroy(this.gameObject);
	}

	void Scope() {
		if (Input.GetMouseButtonDown(1)) {
			isScoped = !isScoped;

			if (isScoped) {
				playerView.fieldOfView = scopedFOV;
			}
			else {
				playerView.fieldOfView = normalFOV;
			}
		}
	}

	void SlowMo() {
		if (Input.GetMouseButtonDown(2)) {
			timeManager.DoSlowmotion();
		}
	}
}
