using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timer))]
[RequireComponent(typeof(WaveSpawnerUI))]
public class WaveSpawner : MonoBehaviour
{
	[Header("Wave Spawner Settings Settings")]
	[Tooltip("List of waves the spawner will spawn. This also determines the amount of waves that will spawn in this section.")]
	[SerializeField] private List<Wave> waves; // The waves that will be spawned in this section.
	[Tooltip("List of empty transforms that the spawner will be able to choose from to spawn an enemy at. Create these empty transforms as children of this object.")]
	[SerializeField] private List<Transform> spawnPoints;
	[Tooltip("The time between waves. This time will be used for looting chests and exploring or setting up traps.")]
	[SerializeField] private float intermissionTime = 20f;
	[Tooltip("The time that it takes for a round to start.")]
	[SerializeField] private float roundStartTime = 5f;

	[SerializeField] private GameObject enemyPrefab; // Debug

	// Private Variables
	private int waveIndex = 0; // The current position in the list of waves. Used to determind what wave data to reference.
	private int enemiesToKill; // The amount of enemies that need to be killed for the wave to end.
	private int enemiesKilled; // The amount of enemies that have been killed from the current wave.

	// Components
	private WaveSpawnerUI waveSpawnerUI;

	// Properties
	public int EnemiesKilled { set => enemiesKilled = value; }
	public int WaveIndex { get => waveIndex; }

	/// <summary>Called by the GameManager when the game or section starts.</summary>>
	public void StartFirstRound(WaveSpawnerUI uiToUpdate)
	{
		waveSpawnerUI = uiToUpdate; // Sets the waveSpawnerUI to be the one we passed in from the GameManager.
		OnCountdownFinished(); // Called when the countdown is finished. In this case its called to start the game.
	}

	/// <summary>Handles the spawning of enemies and incrementing the wave index.</summary>>
	private IEnumerator SpawnWave()
	{
		Wave wave = waves[waveIndex]; // Get the current wave data at the current index.
		enemiesToKill = wave.numberOfEnemiesToSpawn;

		for (int i = 0; i < wave.numberOfEnemiesToSpawn; i++) // Spawn the enemies.
		{
			//SpawnEnemy(wave.GetRandomEnemyType());
			SpawnEnemy(enemyPrefab);
			yield return new WaitForSeconds(1f / wave.timeBetweenSpawns);
		}

		waveIndex++; // We increment the wave index here.
	}

	private void SpawnEnemy(GameObject enemy)
	{
		Instantiate(enemy, PickRandomEnemySpawnPoint().position, Quaternion.identity); // Spawn an enemy at a random spawnpoint.
	}

	/// <summary>Used to increment the enemies killed. Also used to notify the GameManager that we reached the end of the waves and to end the section.</summary>>
	public void AddEnemyKilled()
	{
		enemiesKilled += 1;

		if (enemiesKilled >= enemiesToKill)
		{
			if (!AllWavesCleared())
			{
				enemiesKilled = 0;
				enemiesToKill = 0;

				StartCoroutine(OnWaveEndedRoutine());
			}
			else
			{
				Toolbox.instance.GetGameManager().GameOver();
				Destroy(gameObject);
			}
		}
	}

	/// <summary>This coroutine runs when the wave ends. It handles updating the ui based on what state the game is in.</summary>>
	private IEnumerator OnWaveEndedRoutine()
	{
		Timer newTimer = GameUtilities.CreateNewTimer(); // Create a new timer.
		float animationFinishTime = waveSpawnerUI.ShowWaveSpawnerUI();
		yield return new WaitForSeconds(animationFinishTime + 0.5f);

		newTimer.StartTimer(intermissionTime);

		// TODO: Reward player here. (Add to skillpoints)

		while (newTimer != null) // Run the countdown until the timer is null. (The timer gets destroyed automatically when it reaches 0.)
		{
			waveSpawnerUI.SetCountdownText("Intermission: " + newTimer.TimeLeft);
			yield return null;
		}

		waveSpawnerUI.SetCountdownText("Round Starting!");
		yield return new WaitForSeconds(0.5f);

		Timer newTimer2 = GameUtilities.CreateNewTimer(); // Create a new timer.
		newTimer2.StartTimer(roundStartTime);

		while (newTimer2 != null) // Run the countdown until the timer is null. (The timer gets destroyed automatically when it reaches 0.)
		{
			waveSpawnerUI.SetCountdownText(newTimer2.TimeLeft); 
			yield return null;
		}

		waveSpawnerUI.SetCountdownText("Round Started!");
		yield return new WaitForSeconds(0.5f);

		OnCountdownFinished();
		yield break;
	}

	/// <summary>Called when the countdown for the start of a wave is finished.</summary>>
	private void OnCountdownFinished()
	{
		StartCoroutine(SpawnWave());
		waveSpawnerUI.HideWaveSpawnerUI();
	}

	/// <summary>Picks a random spawnpoint from a list of empty transforms.</summary>>
	private Transform PickRandomEnemySpawnPoint()
	{
		Transform spawnPoint = spawnPoints[Random.Range(1, spawnPoints.Count)];
		return spawnPoint;
	}

	/// <summary>When called, will return true or false depending if the wave index is equal to the amount of waves.</summary>>
	private bool AllWavesCleared()
	{
		return waveIndex == waves.Count;
	}
}
