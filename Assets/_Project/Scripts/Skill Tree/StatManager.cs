using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
	// Inspector Fields
	public List<Stat> startingStats = new List<Stat>();

    public System.Action OnStatUpgraded;
    private void Awake()
    {
        InitializeStats();
    }

	private void InitializeStats()
	{
		foreach(Stat stat in startingStats)
        {
            stat.InitStat();
        }
	}

	public void UpgradeStat(string statName)
	{
        Stat stat = GetStatWithName(statName);
        stat.UpgradeStat();
        stat.IncrementCost(2);

        if(OnStatUpgraded != null)
        {
            OnStatUpgraded.Invoke();
        }
	}

    public Stat GetStatWithName(string statName)
    {
        foreach(Stat stat in startingStats)
        {
            if(statName.Equals(stat.lookupName))
            {
                return stat;
            }
        }

        return new Stat();
    }
}
