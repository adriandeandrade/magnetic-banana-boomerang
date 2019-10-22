using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave Spawning/Waves/New Wave")]
public class Wave : ScriptableObject
{
    [Tooltip("Place prefabs of the enemies you want to spawn in the wave.")]
    public List<GameObject> enemyTypes;
    [Tooltip("How many enemies you would to spawn during the wave.")]
    public int numberOfEnemiesToSpawn;
    [Tooltip("How long to wait before spawning a new enemy.")]
    public float timeBetweenSpawns;
    public int rewardAmount;

    public GameObject GetRandomEnemyType()
    {
        GameObject enemy = enemyTypes[Random.Range(1, (enemyTypes.Count - 1))];
        return enemyTypes[0];
    }
}
