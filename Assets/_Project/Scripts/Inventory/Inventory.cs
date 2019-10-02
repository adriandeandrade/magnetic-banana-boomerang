using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Inspector Fields
    [SerializeField] private int skillPoints;

    // Private Variables
    
    // Components

    // Properties
    public int SkillPoints { get => skillPoints; }

    // Events

    public void AddSkillPoint(int amountToAdd)
    {
        skillPoints += amountToAdd;
        // TODO: UI for showing we a skill point. 
    }

    public void RemoveSkillPoint(int amountToRemove)
    {
        skillPoints -= amountToRemove;
    }
}
