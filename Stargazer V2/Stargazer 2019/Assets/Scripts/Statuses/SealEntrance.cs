using UnityEngine;

public class SealEntrance : MonoBehaviour
{
    public Transform trackingTarget;
    public GameObject explosion;

    private void Update()
    {
        if (trackingTarget == null)
        {
            GameObject explode = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
            explode.transform.localScale = Vector3.one * 4;
            ParticleSystem parts = explode.GetComponent<ParticleSystem>();
            float totalDuration = parts.main.duration + parts.main.startLifetime.constant;
            Destroy(explode, totalDuration);

            Destroy(this.gameObject);
        }
    }

}
