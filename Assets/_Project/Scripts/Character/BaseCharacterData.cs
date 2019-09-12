using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character Data/New Character Data")]
public class BaseCharacterData : ScriptableObject
{
    [Header("Settings")]
    public string characterName;
    public CharacterType characterType;

    [Header("Stats")]
    public float maxHealth;
    // TODO: Add more stats.
}
