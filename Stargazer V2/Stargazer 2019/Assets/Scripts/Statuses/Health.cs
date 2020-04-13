using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    public float entityHealth = 10;
    public GameObject optionalExplosionEffect;

    public void SetHealth(int health)
    {
        this.entityHealth = health;
    }

    public void ChangeHealthBy(float healthValue)
    {
        this.entityHealth = Mathf.Max(entityHealth - healthValue, 0);
    }


    //The reason why I put check health here instead of change health by is to prevent "chain" death for bomber
    void Update()
    {
        if (this.entityHealth <= 0)
        {
            if (gameObject != null)
            {
                Entity entity = this.gameObject.GetComponent<Entity>();
                if (entity != null)
                {
                    if (entity.transform.CompareTag("Player"))
                    {
                        GameManager.lastLevelDeath = SceneManager.GetActiveScene().buildIndex;
                        SceneManager.LoadScene(7);
                    }
                     
                    entity.Death();
                } else
                {
                    if (optionalExplosionEffect != null)
                    {
                        GameObject explode = Instantiate(optionalExplosionEffect, transform.position, transform.rotation) as GameObject;
                        explode.transform.localScale = transform.localScale * 2;
                        ParticleSystem parts = explode.GetComponent<ParticleSystem>();
                        float totalDuration = parts.main.duration + parts.main.startLifetime.constant;
                        Destroy(explode, totalDuration);
                    }

                    Destroy(gameObject);

                }
            }
        }
    }
}