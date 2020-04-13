using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private float speed;
    private float range;
    private Vector3 velocity;
    private Rigidbody body;
    private Vector3 originalPosition;

    private const int DEFAULT_DAMAGE_VALUE = 20;
    private float damageModifier = 1;

    public GameObject initParticle;
    public GameObject destroyParticle;

    public float explosiveRadius = 5;
    public float explosiveForce = 5;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 direction, float speed, float range)
    {
        GameObject explode = Instantiate(initParticle, transform.position, transform.rotation) as GameObject;
        explode.transform.localScale = Vector3.one * 2;
        ParticleSystem parts = explode.GetComponent<ParticleSystem>();
        float totalDuration = parts.main.duration + parts.main.startLifetime.constant;
        Destroy(explode, totalDuration);

        this.speed = speed;
        this.velocity = this.speed * direction;
        this.originalPosition = transform.position;
        this.range = range;
    }

    public void SetDamageModifier(float value)
    {
        damageModifier = value;
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(this.originalPosition, transform.position) < range)
        {
            body.MovePosition(body.position + this.velocity * Time.fixedDeltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Death()
    {
        GameObject explode = Instantiate(destroyParticle, transform.position, transform.rotation) as GameObject;
        explode.transform.localScale = Vector3.one * 2;
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
                health.ChangeHealthBy(5);
            }
        }

        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.ChangeHealthBy(DEFAULT_DAMAGE_VALUE * damageModifier);
        }

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.rotation * Vector3.forward * 5, ForceMode.Impulse);
        }
        if (other.tag != "Equipment") Death();
    }
}
