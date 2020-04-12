using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SpiderCannonData", menuName = "Item/SpiderCannonData", order = 0)]
public class SpiderCannonData : RangeWeaponData
{
    [Space]
    public int maxNumberOfCannonBalls = 100;
    public int currentNumberOfCannonBalls = 100;
    public float maxCannonReloadTime = 10.0f;
    public float maxBombardReloadTime = .1f;
    public float cannonBallSpeed = 3.0f;
    public float cannonBallRange = 500.0f;

    [NonSerialized]
    public float currentCannonReloadTime = 0;
    [NonSerialized]
    public float currentBombardReloadTime = 0;

    public float rotateSpeed = .1f; //Rotations per Second

    public GameObject cannonBallType;
}
