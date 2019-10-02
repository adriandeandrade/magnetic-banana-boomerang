using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] private List<Stat> startingStats = new List<Stat>();
	[SerializeField] private StatUI statsUI;
	public Dictionary<Stat, float> stats = new Dictionary<Stat, float>();

	// Private Variables
	private Player player;

	// Components

	// Events

	// Properties

	private void Start()
	{
		InitializeStats();
		player = Toolbox.instance.GetGameManager().PlayerRef;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Tab))
		{
			statsUI.OpenUpgradeMenu();
		}
	}

	private void InitializeStats()
	{
		if (startingStats != null && startingStats.Count > 0)
		{
			foreach (Stat stat in startingStats)
			{
				if (!stats.ContainsKey(stat))
				{
					stats.Add(stat, stat.statBaseValue);
					Debug.Log("Added: " + stat.statName + " to stat dictionary.");
				}
				else
				{
					Debug.LogWarning("Stat already exists.");
				}
			}

			statsUI.InitializeStatsUI(stats);
		}
	}

	public void UpgradeStat(Stat statToUpgrade)
	{
		if (CheckIfStatExists(statToUpgrade))
		{
			if (FindObjectOfType<Inventory>().SkillPoints >= statToUpgrade.upgradeCost)
			{
				float currentStatValue = stats[statToUpgrade];

				stats[statToUpgrade] += statToUpgrade.upgradeAmount;
				Debug.Log("Upgraded: " + statToUpgrade.statName + " from: " + currentStatValue.ToString() + " to: " + stats[statToUpgrade]);

				player.PlayerInventory.RemoveSkillPoint(statToUpgrade.upgradeCost);
			}
		}
	}

	public float GetStatValue(Stat statToGet)
	{
		return stats[statToGet];
	}

	private bool CheckIfStatExists(Stat statToCheck)
	{
		if (stats.ContainsKey(statToCheck)) return true;
		return false;
	}
}
