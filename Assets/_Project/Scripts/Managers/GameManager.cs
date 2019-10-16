using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Timer))]
public class GameManager : BaseManager
{
	// Inspector Fields
	[SerializeField] private float gameStartTime; // The amount of timer that the timer will countdown for in before the round starts.
	[SerializeField] private WaveSpawnerUI waveSpawnerUI;

	// Private Variables
	private bool gameStarted = false;
	private Player player;

	// Components
	private WaveSpawner currentWaveSpawner;

	// Properties
	public WaveSpawner WaveSpawnerInstance
	{
		get
		{
			if (currentWaveSpawner != null)
			{
				return currentWaveSpawner;
			}
			else
			{
				Debug.LogError("Wave Spawner cannot be found, please assign a wave spawner!");
				return null;
			}
		}
	}

	public Player PlayerRef
	{
		get
		{
			if (player != null)
			{
				return player;
			}
			else
			{
				player = FindObjectOfType<Player>();
				return player;
			}
		}
	}

	// Events

	private void Update()
	{
		// TODO: Remove this when done debugging!
		if (Input.GetKeyDown(KeyCode.R))
		{
			StartGame();
		}
	}

	private void Awake()
	{
		player = FindObjectOfType<Player>();
	}

	private void Start()
	{
		if (!gameStarted)
		{
			StartGame();
		}

	}

	private void StartGame()
	{
		StartCoroutine(OnGameStartRoutine());
		gameStarted = true;
	}

	private IEnumerator OnGameStartRoutine()
	{
		Timer newTimer = GameUtilities.CreateNewTimer();
		float animationFinishTime = waveSpawnerUI.ShowWaveSpawnerUI();
		yield return new WaitForSeconds(animationFinishTime + 0.5f);

		newTimer.StartTimer(gameStartTime);

		while (newTimer != null)
		{
			waveSpawnerUI.SetCountdownText(newTimer.TimeLeft);

			yield return null;
		}

		waveSpawnerUI.SetCountdownText("Round Started!");
		yield return new WaitForSeconds(0.5f);
		waveSpawnerUI.HideWaveSpawnerUI();

		GameObject waveSpawnerPrefab = Resources.Load<GameObject>("Prefabs/Components/prefab_WaveSpawner");
		WaveSpawner newWaveSpawner = Instantiate(waveSpawnerPrefab).GetComponent<WaveSpawner>();
		currentWaveSpawner = newWaveSpawner;
		newWaveSpawner.StartFirstRound(waveSpawnerUI);

		yield break;
	}

	public void GameOver()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public override void Initialize()
	{

	}
}
