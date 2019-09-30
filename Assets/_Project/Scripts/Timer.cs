using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
	private float timeLeft;
	private bool startTimer;

	public float TimeLeft { get => timeLeft; }

	//Events
	public event System.Action OnTimerEnd;

	public void StartTimer(float _timerAmount)
	{
		startTimer = true;
		timeLeft = _timerAmount;
	}

	private void Update()
	{
		if (startTimer)
		{
			timeLeft -= Time.deltaTime;

			if (timeLeft < 0)
			{
				startTimer = false;
				if (OnTimerEnd != null)
				{
					OnTimerEnd.Invoke();
				}
			}
		}
	}

}
