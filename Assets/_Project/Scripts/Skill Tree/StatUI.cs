using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(StatManager))]
public class StatUI : MonoBehaviour
{
	// Insepctor Fields
	[SerializeField] private GameObject statUpgradeButtonPrefab; // The button we will spawn.
	[SerializeField] private GameObject statsParentObject; // This is the object that holds all the ui elements.
	[SerializeField] private GameObject statsUIPanel; // This is where we will spawn our buttons into.
	[SerializeField] private TextMeshProUGUI skillPointAmountText;
	[SerializeField] private StatManager statManager;

	// Private Variables
	private bool menuOpen = false;

	// Components

	// Events

	// Properties

	private void Update()
	{
		if(menuOpen)
		{
			UpdateUpgradeMenu();

			if(Input.GetKeyDown(KeyCode.Escape))
			{
				CloseUpgradeMenu();
			}
		}
	}

	public void OpenUpgradeMenu()
	{
		menuOpen = true;
		statsParentObject.SetActive(true);
		skillPointAmountText.SetText(Toolbox.instance.GetGameManager().PlayerRef.PlayerInventory.SkillPoints.ToString());
		Time.timeScale = 0;
	}

	public void CloseUpgradeMenu()
	{
		menuOpen = false;
		statsParentObject.SetActive(false);
		Time.timeScale = 1;
	}

	private void UpdateUpgradeMenu()
	{
		skillPointAmountText.SetText(Toolbox.instance.GetGameManager().PlayerRef.PlayerInventory.SkillPoints.ToString());
	}

	public void InitializeStatsUI(Dictionary<Stat, float> _stats)
	{
		ClearCurrentButtons();

		foreach (KeyValuePair<Stat, float> stat in _stats)
		{
			GameObject newUpgradeButton = Instantiate(statUpgradeButtonPrefab);
			newUpgradeButton.transform.parent = statsUIPanel.transform;

			Button newButtonComponent = newUpgradeButton.GetComponent<Button>();
			newButtonComponent.onClick.AddListener(delegate { UpgradeStat(stat.Key); });

			TextMeshProUGUI buttonText = newUpgradeButton.GetComponentInChildren<TextMeshProUGUI>();
			buttonText.SetText("Upgrade: " + stat.Key.statName);
		}
	}

	public void UpgradeStat(Stat statToUpgrade)
	{
		statManager.UpgradeStat(statToUpgrade);
	}

	private void ClearCurrentButtons()
	{
		if (statsUIPanel.transform.childCount > 0)
		{
			foreach (Transform button in statsUIPanel.transform)
			{
				Destroy(button);
			}
		}
	}
}
