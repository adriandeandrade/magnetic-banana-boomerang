using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
	// Inspector Fields
	public List<Stat> startingStats = new List<Stat>();
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

        Debug.Log("Stat init");
	}

	public void UpgradeStat(string statName)
	{
        Stat stat = GetStatWithName(statName);
        stat.UpgradeStat();
        stat.IncrementCost(2);
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
