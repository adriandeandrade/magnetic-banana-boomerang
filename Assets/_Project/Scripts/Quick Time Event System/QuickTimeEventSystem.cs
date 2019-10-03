using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickTimeEventSystem : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] private List<KeyCode> possibleQuickTimeEventKeys;
	[SerializeField] private GameObject quickTimeEventUI;
	[SerializeField] private Image timerBar;
	[SerializeField] private TextMeshProUGUI keyText;
	[SerializeField] private float waitForKeyTime; // The amount of the time you have to press the key in.

	// Private Variables
	private bool isInQuicktimeEvent = false;
	private KeyCode quickTimeEventKeycode; // The key that will need to be pressed for the quicktime event.

	// Events
	public event System.Action OnKeyNotPressedInTime;
	public event System.Action OnKeyPressedOnTime;

	public void StartQuickTimeEvent()
	{
		if (!isInQuicktimeEvent)
		{
			SetRandomKey();
			quickTimeEventUI.SetActive(true);
			isInQuicktimeEvent = true;
			Time.timeScale = 0.3f;
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			StartQuickTimeEvent();
		}
	}

	private void SetRandomKey()
	{
		quickTimeEventKeycode = possibleQuickTimeEventKeys[Random.Range(0, possibleQuickTimeEventKeys.Count)];

		switch (quickTimeEventKeycode)
		{
			case KeyCode.W:
				keyText.SetText("W");
				break;

			case KeyCode.A:
				keyText.SetText("A");
				break;

			case KeyCode.S:
				keyText.SetText("S");
				break;

			case KeyCode.D:
				keyText.SetText("D");
				break;
		}
	}

	private IEnumerator QuickTimeEvent()
	{
		float currentWaitTime = waitForKeyTime;

		while (currentWaitTime > 0)
		{
			if (Input.GetKeyDown(quickTimeEventKeycode))
			{
				Debug.Log("We pressd the key before the timer ended!");

				if (OnKeyPressedOnTime != null)
				{
					OnKeyPressedOnTime.Invoke();
				}

				quickTimeEventUI.SetActive(false);
				isInQuicktimeEvent = false;
				Time.timeScale = 1f;

				yield break;
			}

			currentWaitTime -= Time.unscaledDeltaTime;
			timerBar.fillAmount = currentWaitTime / waitForKeyTime;

			yield return null;
		}

		Debug.Log("Key not pressed on time.");

		if (OnKeyNotPressedInTime != null)
		{
			OnKeyNotPressedInTime.Invoke();
		}

		quickTimeEventUI.SetActive(false);
		isInQuicktimeEvent = false;
		Time.timeScale = 1f;

		yield break;
	}

	public void OnQTEAnimationFinished()
	{
		StartCoroutine(QuickTimeEvent());
	}
}
