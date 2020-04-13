using UnityEngine;

enum Stage
{
    One,
    Two,
    Three
}

public class SpiderAI : Entity
{
    public Transform targetRef;
    public GameObject weaponPrefab;
    public GameObject deathExplosion;
    private EquipAction equipAction;
    private Transform centerTransform;
    private Stage currentStage = Stage.One;
    private float time = 0;
    public int maxInterval = 4;
    private bool activate = false;
    public delegate void OnSpiderActivated();
    public OnSpiderActivated spiderActivated;

    void Start()
    {
        GameObject weaponObject = Instantiate(weaponPrefab);
        weaponObject.transform.localScale = Vector3.one * 2;
        SpiderCannon weapon = weaponObject.GetComponent<SpiderCannon>();
        centerTransform = transform.GetChild(0);

        equipAction = GetComponent<EquipAction>();
        equipAction.EquipItem(weapon, centerTransform);
    }

    void StageOne()
    {
        Quaternion q = Quaternion.LookRotation(targetRef.position - centerTransform.position);

        centerTransform.rotation = Quaternion.RotateTowards(centerTransform.rotation, q, this.turnSpeed * Time.deltaTime);

        equipments[EquipmentType.Weapon].Activate();
    }

    void StageTwo()
    {
        Quaternion q = Quaternion.LookRotation(targetRef.position - centerTransform.position);

        centerTransform.rotation = Quaternion.RotateTowards(centerTransform.rotation, q, this.turnSpeed * Time.deltaTime);

        equipments[EquipmentType.Weapon].ActivateSecondary();
    }

    void StageThree()
    {
        //points upward and shoots
        if (Vector3.Distance(centerTransform.eulerAngles, Vector3.right * 270) > 1f)
        {
            centerTransform.rotation = Quaternion.RotateTowards(centerTransform.rotation, Quaternion.Euler(Vector3.right * 270), turnSpeed * Time.deltaTime);
        } else
        {
           equipments[EquipmentType.Weapon].ActivateTertiary();
        }
    }

    private void Update()
    {   
        if (targetRef != null)
        {
            if (!activate)
            {
                if (Vector3.Distance(targetRef.position, transform.position) <= 40.0f)
                {
                    activate = true;
                    if (spiderActivated != null)
                    {
                        spiderActivated.Invoke();
                    }
                }
            }
            else
            {
                time += Time.deltaTime;

                if ((int)time % maxInterval == 0)
                {
                    if (Physics.Raycast(transform.position, targetRef.position - transform.position, out RaycastHit hit))
                    {
                        if (hit.transform.CompareTag("Player"))
                        {
                            if (Vector3.Distance(targetRef.position, transform.position) <= 20.0f)
                            {
                                currentStage = Stage.One;
                            }
                            else
                            {
                                currentStage = Stage.Two;
                            }
                        }
                        else
                        {
                            currentStage = Stage.Three;
                        }
                    }
                    else
                    {
                        currentStage = Stage.Three;
                    }
                }

                if (currentStage == Stage.One) StageOne();
                if (currentStage == Stage.Two) StageTwo();
                if (currentStage == Stage.Three) StageThree();
            }
        }
    }

    public override void Death()
    {
        if (deathExplosion != null)
        {
            GameObject explode = Instantiate(deathExplosion, transform.position, transform.rotation) as GameObject;
            explode.transform.localScale = Vector3.one * 9;
            ParticleSystem parts = explode.GetComponent<ParticleSystem>();
            float totalDuration = parts.main.duration + parts.main.startLifetime.constant;
            Destroy(explode, totalDuration);
        }

        if (equipments[EquipmentType.Weapon] != null)
        {
            equipments[EquipmentType.Weapon].transform.localScale = Vector3.one / 5;
            this.equipAction.UnequipItem(equipments[EquipmentType.Weapon]);
        }

        Destroy(this.gameObject);
    }
}
