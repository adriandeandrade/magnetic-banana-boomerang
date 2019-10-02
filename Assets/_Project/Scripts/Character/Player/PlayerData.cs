using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Character Data/Player/New Player Data")]
public class PlayerData : BaseCharacterData
{
    public float baseBoomerangDamage;
    public float secondaryAttackDamage;
    public float quickTimeActivationTime;
}
