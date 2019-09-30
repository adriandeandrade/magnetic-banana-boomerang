using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
	[Header("Wave Settings")]
	[Tooltip("Each wave is configurable.")]
	[SerializeField] private List<Wave> waves;
    [SerializeField] private GameObject enemyPrefab;
	[Tooltip("These are locations where enemies can spawn from. Create transforms in the hierarchy assign them in the inspector.")]
	[SerializeField] private List<Transform> spawnPoints;
	[SerializeField] private float timeBetweenWaves = 5f;

	// Private Variables
	private int waveIndex = 0;
	private int enemiesToKill;
    private float countdown = 2f;

	private void Update()
	{
		if (waveIndex == waves.Count)
		{

		}

		//UpdateCountDown();

        if(Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(SpawnWave());
        }
	}

	public void StartWave()
	{
		StartCoroutine(SpawnWave());
	}

	IEnumerator SpawnWave()
	{
		Wave wave = waves[waveIndex];
		enemiesToKill = wave.numberOfEnemiesToSpawn;

		for (int i = 0; i < wave.numberOfEnemiesToSpawn; i++)
		{
			//SpawnEnemy(wave.GetRandomEnemyType());
			SpawnEnemy(enemyPrefab);
			yield return new WaitForSeconds(1f / wave.timeBetweenSpawns);
		}

		waveIndex++;
	}

	private void SpawnEnemy(GameObject enemy)
	{
		Instantiate(enemy, GetRandomSpawnPoint().position, Quaternion.identity);
	}

	private void UpdateCountDown()
	{
		if (countdown <= 0f)
		{
			StartCoroutine(SpawnWave());
			countdown = timeBetweenWaves;
		}

		countdown -= Time.deltaTime;
	}

	public Transform GetRandomSpawnPoint()
	{
		Transform spawnPoint = spawnPoints[Random.Range(1, (spawnPoints.Count - 1))];
		return spawnPoint;
	}
}
