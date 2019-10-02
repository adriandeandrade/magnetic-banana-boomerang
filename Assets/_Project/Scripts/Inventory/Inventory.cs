using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] private int skillPoints;

	private Dictionary<Item, int> items = new Dictionary<Item, int>();

	// Private Variables

	// Components

	// Properties
	public int SkillPoints { get => skillPoints; }

	// Events

	public void AddItem(Item itemToAdd, int amountToAdd)
	{
		if (CheckIfItemExists(itemToAdd))
		{
			items[itemToAdd] += amountToAdd;
		}
		else
		{
			items.Add(itemToAdd, amountToAdd);
		}

        Debug.Log("Added: " + itemToAdd.itemName);
	}

	public void RemoveItem(Item itemToRemove, int amountToRemove)
	{
		if (CheckIfItemExists(itemToRemove))
		{
			items[itemToRemove] -= amountToRemove;

			int amountLeft = GetCurrentAmount(itemToRemove);
			if (amountLeft <= 0)
			{
				items.Remove(itemToRemove);
			}
		}
	}

	public bool CheckIfItemExists(Item itemToCheck)
	{
		if (items.ContainsKey(itemToCheck))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public int GetCurrentAmount(Item itemToCheck)
	{
		if (CheckIfItemExists(itemToCheck))
		{
			int amountLeft = items[itemToCheck];
			return amountLeft;
		}
		else
		{
			return 0;
		}
	}

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
