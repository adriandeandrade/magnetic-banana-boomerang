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

    // Private Variables

    // Components
    private WaveSpawner waveSpawner;

	private void Awake()
	{
        waveSpawner = GetComponent<WaveSpawner>();

        if(waveSpawnerUI != null)
        {
            ToggleWaveSpawnerUI(false);
        }
        else
        {
            Debug.LogError("Wave Spawner UI Panel not referenced. Please assign a Wave Spawner UI Panel.");
        }
	}

	private void Update()
	{

	}

    public void SetCountdownText(float text)
    {
        countdownText.SetText(text.ToString("f0"));
    }

	public void ToggleWaveSpawnerUI(bool showState)
	{
		if (waveSpawnerUI != null)
		{
			waveSpawnerUI.SetActive(showState);
		}
	}
}
