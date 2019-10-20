using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Enemy Data", menuName = "Enemies/New Ranged Enemy Data")]
public class RangedEnemyCharacterData : BaseCharacterData
{
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float fireRate;
    public float fireDelay;
    public float accuracy;
    public float damageAmount = 1f;
}
