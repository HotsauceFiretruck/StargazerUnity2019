using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiderBossHealthText : MonoBehaviour
{
    public GameObject healthText;
    public SpiderAI spiderAI;
    public Health spiderHealth;
    bool activated = false;
    float health = 0;

    void Start()
    {
        healthText.SetActive(false);
        spiderAI.spiderActivated += AwakenMyMaster;
    }

    void AwakenMyMaster()
    {
        healthText.SetActive(true);
        activated = true;
    }

    private void Update()
    {
        if (activated)
        {
            if (spiderHealth != null)
            {
                if (health != spiderHealth.entityHealth)
                {
                    health = spiderHealth.entityHealth;
                    healthText.GetComponent<Text>().text = "Mechanical Spider Health: " + health;
                }
            }
        }
    }

}
