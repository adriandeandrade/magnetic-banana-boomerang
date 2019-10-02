using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(WaveSpawnerUI))]
public class WaveSpawner : MonoBehaviour
{
	[Header("Wave Settings")]
	[SerializeField] private List<Wave> waves; // The waves that will be spawned in this section.
	[SerializeField] private GameObject enemyPrefab; // Debug
	[Tooltip("These are locations where enemies can spawn from. Create transforms in the hierarchy assign them in the inspector.")]
	[SerializeField] private List<Transform> spawnPoints;
	[SerializeField] private float timeBetweenWaves = 5f;
	[SerializeField] private Timer timer;

	// Private Variables
	private int waveIndex = 0;
	private int enemiesToKill;
	private int enemiesKilled;
	private bool isCountingDown = false;

	// Components
	private WaveSpawnerUI waveSpawnerUI;

	// Properties
	public int EnemiesKilled { set => enemiesKilled = value; }
	public int WaveIndex { get => waveIndex; }

	private void OnEnable()
	{
		timer.OnTimerEnd += OnCountdownFinished;
	}

	private void Awake()
	{
		waveSpawnerUI = GetComponent<WaveSpawnerUI>();
	}

	private void Update()
	{
		if(isCountingDown)
		{
			waveSpawnerUI.SetCountdownText(timer.TimeLeft);
		}
	}

	public void StartNextWave()
	{
		waveSpawnerUI.ToggleWaveSpawnerUI(true);
		timer.StartTimer(timeBetweenWaves);
		isCountingDown = true;
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

	private void OnCountdownFinished()
	{
		StartCoroutine(SpawnWave());
		waveSpawnerUI.ToggleWaveSpawnerUI(false);
		isCountingDown = false;
	}

	public Transform GetRandomSpawnPoint()
	{
		Transform spawnPoint = spawnPoints[Random.Range(1, (spawnPoints.Count - 1))];
		return spawnPoint;
	}

	public void AddEnemyKilled()
	{
		if (enemiesKilled + 1 >= enemiesToKill)
		{
			// Wave has been beat.
			if (!HasWonSection())
			{
				StartNextWave();
			}
			else
			{
				// Tell the player they won using the gamemanager.
				Toolbox.instance.GetGameManager().GameOver();
			}
		}
		else
		{
			enemiesKilled += 1;
		}

	}

	public bool HasWonSection()
	{
		return waveIndex == waves.Count;
	}
}
