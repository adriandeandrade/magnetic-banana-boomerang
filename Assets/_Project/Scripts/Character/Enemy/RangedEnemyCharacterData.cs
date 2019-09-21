using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Enemy Data", menuName = "Enemies/New Ranged Enemy Data")]
public class RangedEnemyCharacterData : BaseCharacterData
{
    public GameObject projectilePrefab;
    public float fireRate;
    public float fireDelay;
}
