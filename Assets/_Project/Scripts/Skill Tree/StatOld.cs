using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Stats/New Stat")]
public class StatOld : ScriptableObject
{
    public string statName;
    public float statBaseValue;
    public int upgradeCost;
    public float upgradeAmount; // The amount that will be added to the stat.
}
