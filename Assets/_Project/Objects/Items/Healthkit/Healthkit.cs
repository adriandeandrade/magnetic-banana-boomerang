using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthkit : ItemBehavior
{
	[SerializeField] private float healAmount = 2f;

	public override void OnItemReachDestination()
	{
		if (player.GetCurrentHealth() >= player.GetMaxHealth())
		{
			player.PlayerInventory.AddItem(itemData, 1);
		}
		else
		{
			player.AddHealth(healAmount);
		}

		Destroy(gameObject);
	}
}
