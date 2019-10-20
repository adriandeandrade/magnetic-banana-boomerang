using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickTimeEventSystem : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] private List<QTEKey> possibleQTEKeys; /* W = 0, A = 1, S = 2, D = 3 */
	[SerializeField] private GameObject quickTimeEventUI;
	[SerializeField] private Image timerBar;
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
		int randomIndex = Random.Range(0, possibleQTEKeys.Count);
		quickTimeEventKeycode = possibleQTEKeys[randomIndex].associatedKeycode;

		possibleQTEKeys[randomIndex].SetQTEKey();
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

				ResetQTE();
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

		ResetQTE();
		yield break;
	}

	public void OnQTEAnimationFinished()
	{
		StartCoroutine(QuickTimeEvent());
	}

	private void ResetQTE()
	{
		foreach (QTEKey key in possibleQTEKeys)
		{
			key.ResetQTEKey();
		}

		isInQuicktimeEvent = false;
		timerBar.fillAmount = 1;
		quickTimeEventUI.SetActive(false);
		Time.timeScale = 1f;
	}

	[System.Serializable]
	public struct QTEKey
	{
		public KeyCode associatedKeycode;
		public Sprite regularKey;
		public Sprite highlightedKey;
		public Image keyImage;

		public void SetQTEKey()
		{
			keyImage.sprite = highlightedKey;
		}

		public void ResetQTEKey()
		{
			keyImage.sprite = regularKey;
		}
	}
}
