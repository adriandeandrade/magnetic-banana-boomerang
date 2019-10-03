using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSpawnerUI : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] private GameObject waveSpawnerUI;
	[SerializeField] private TextMeshProUGUI countdownText;
	[SerializeField] private Animator animator;

	// Components
	private WaveSpawner waveSpawner;

	public void SetCountdownText(float text)
	{
		countdownText.SetText(text.ToString("f0"));
	}

	public void SetCountdownText(string text)
	{
		countdownText.SetText(text);
	}

	public float ShowWaveSpawnerUI()
	{
		if (waveSpawnerUI != null)
		{
			waveSpawnerUI.SetActive(true);
			AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

			if (clipInfo.Length > 0 && clipInfo != null)
			{
				return clipInfo[0].clip.length;
			}
		}

		return 0;
	}

	public void HideWaveSpawnerUI()
	{
		animator.Play("wavespawnerui-hide");
	}

	public void HideUI() // Called by animation event.
	{
		waveSpawnerUI.SetActive(false);
	}
}
