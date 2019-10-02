using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Timer))]
public class GameManager : BaseManager
{
	// Inspector Fields
	[SerializeField] private GameObject countdownPanel;
	[SerializeField] private TextMeshProUGUI countDownText;
	[SerializeField] private WaveSpawner waveSpawner;
	[SerializeField] private float gameStartTime; // The amount of timer that the timer will countdown for in before the round starts.
	[SerializeField] private Timer timer;

	// Private Variables
	private Player player;

	// Components

	// Properties
	public WaveSpawner WaveSpawnerInstance
	{
		get
		{
			if (waveSpawner != null)
			{
				return waveSpawner;
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

	private void OnEnable()
	{
		timer.OnTimerEnd += StartGame;
	}

	private void OnDisable()
	{
		timer.OnTimerEnd -= StartGame;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			StartGame();
		}
	}

	private void Awake()
	{
		player = FindObjectOfType<Player>();
	}

	private void StartGame()
	{
		StartCoroutine(GameStartRoutine());
	}

	private IEnumerator GameStartRoutine()
	{
		Debug.Log("Game Starting in " + gameStartTime);
		yield return new WaitForSeconds(gameStartTime);

		waveSpawner.StartNextWave();
	}

	public void GameOver()
	{
		Debug.Log("Game has ended. All waves have been cleared.");
	}

	public override void Initialize()
	{

	}
}
