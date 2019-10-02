using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractObjects : MonoBehaviour
{
	[SerializeField] private float maxAttractDistance; // How fat an item can be to be attracted to the player.
	[SerializeField] private float itemAttractSpeed; // How fast the item will move towards the player.

	// Update is called once per frame
	void FixedUpdate()
	{
		Collider2D[] nearbyItems = Physics2D.OverlapCircleAll(transform.position, maxAttractDistance);

		foreach (Collider2D item in nearbyItems)
		{
			ItemBehavior itemComponent = item.GetComponent<ItemBehavior>();

			if (itemComponent)
			{
				itemComponent.MoveItem(itemAttractSpeed);
			}
		}
	}
}
