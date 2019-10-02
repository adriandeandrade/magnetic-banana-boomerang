using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
	// Inspector Fields
	[Header("Chest Settings")]
	[HideInInspector] public List<Item> possibleItemSpawns = new List<Item>();
	[SerializeField] private int minAmountItemSpawns = 2;
	[SerializeField] private int maxAmountItemSpawns = 4;
	[SerializeField] private bool debug;

	// Private Variables
	private Animator animator;
	private bool isOpened = false;

	public override void Awake()
	{
		base.Awake();
		animator = GetComponent<Animator>();
	}

	public override void Interact()
	{
		if (!isOpened)
		{
			StartCoroutine(SpawnItems());
		}
	}

	private IEnumerator SpawnItems()
	{
		if (!debug)
		{
			isOpened = true;
		}

		animator.SetTrigger("Open");

		yield return new WaitForSeconds(0.2f);

		int amountOfSpawns = Random.Range(minAmountItemSpawns, maxAmountItemSpawns);

		for (int i = 0; i < amountOfSpawns; i++)
		{
			Item itemToSpawn = PickRandomItem();
			GameObject newItem = Instantiate(itemToSpawn.itemPrefab, transform.position, Quaternion.identity);
			newItem.GetComponent<ItemBehavior>().MoveItemInRandomDirection();
			yield return null;
		}

	}

	private Item PickRandomItem()
	{
		return possibleItemSpawns[Random.Range(0, possibleItemSpawns.Count)];
	}

}
