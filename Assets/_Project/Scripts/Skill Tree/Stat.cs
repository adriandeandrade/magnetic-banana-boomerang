[System.Serializable]
public class Stat
{
    public string statTitle;
    public string lookupName;
    public int baseValue;
    public int baseCost;
    public float upgradeValue;
    public StatTarget statTarget;

    public int currentCost;
    public float currentValue;

    public void InitStat()
    {
        currentCost = baseCost;
        currentValue = baseValue;
    }

    public void IncrementCost(int newCost)
    {
        currentCost += newCost;
    }
    public void UpgradeStat()
    {
        currentValue += upgradeValue;
    }
}

public enum StatTarget
{
    GORILLA,
    TURTLE
}
