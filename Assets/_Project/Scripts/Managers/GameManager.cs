using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : BaseManager
{
	// Inspector Fields
	[SerializeField] private GameObject countdownPanel;
	[SerializeField] private TextMeshProUGUI countDownText;
	[SerializeField] private WaveSpawner waveSpawner;
	[SerializeField] private float waveStartDelay;

	private Timer timer;
	private bool countingDown;

	private void Awake()
	{
		timer = new Timer();
	}

	private void Start()
	{
		StartTimer();
	}

	private void StartTimer()
	{
		timer = gameObject.AddComponent<Timer>();
		timer.OnTimerEnd += StartRound;
		timer.StartTimer(2f);
		countingDown = true;

		countdownPanel.SetActive(true);
	}

	private void Update()
	{
		if(countingDown)
		{
			countDownText.SetText(timer.TimeLeft.ToString("f0"));
		}
	}

	private void StartRound()
	{
		StartCoroutine(RoundStartRoutine());
		countingDown = false;
	}

	private IEnumerator RoundStartRoutine()
	{
		countDownText.SetText("Round Start");
		yield return new WaitForSeconds(waveStartDelay);

		countdownPanel.SetActive(false);
		waveSpawner.StartWave();
		Destroy(timer);
	}

	public override void Initialize()
	{

	}
}
