using UnityEngine;

public class Bullet : MonoBehaviour
{

    private float speed;
    private float range;
    private Vector3 velocity;
    private Rigidbody bulletBody;
    private Vector3 originalPosition;

    private const int DEFAULT_DAMAGE_VALUE = 5;
    private float damageModifier = 1;

    public GameObject initParticle;
    public GameObject destroyParticle;

    void Start()
    {
        bulletBody = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 direction, float speed, float range)
    {
        GameObject explode = Instantiate(initParticle, transform.position, transform.rotation) as GameObject;
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
            bulletBody.MovePosition(bulletBody.position + this.velocity * Time.fixedDeltaTime);
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

        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            health.ChangeHealthBy(DEFAULT_DAMAGE_VALUE * damageModifier);
        }
        if (other.tag != "Equipment") Death();
    }
}