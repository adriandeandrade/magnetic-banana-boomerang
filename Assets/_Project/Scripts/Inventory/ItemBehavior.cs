using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ItemBehavior : MonoBehaviour
{
	// Inspectgor Fields
	[SerializeField] protected Item itemData;

	// Private Variables
	private Rigidbody2D rBody;
	protected Player player;
	private Collider2D col;

	// Components

	// Events

	// Properties

	private bool moving;

	private void Awake()
	{
		rBody = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
	}

	private void Start()
	{
		player = Toolbox.instance.GetGameManager().PlayerRef;
	}

	public void MoveItem(float moveSpeed)
	{
		if (!moving)
		{
			StartCoroutine(MoveToPlayerOverSpeed(moveSpeed));
			moving = true;
			col.enabled = false;
		}
	}

	public void MoveItemInRandomDirection()
	{
		rBody.AddForce(new Vector2(Random.Range(0, 10), Random.Range(0, 10)), ForceMode2D.Impulse);
	}

	IEnumerator MoveToPlayerOverSpeed(float moveSpeed)
	{
		Vector2 dir = (transform.position - player.transform.position).normalized;

		while (Vector2.Distance(transform.position, player.transform.position) >= 0.1f)
		{
			rBody.position = Vector2.MoveTowards(rBody.position, player.transform.position, moveSpeed * Time.fixedDeltaTime);
			yield return new WaitForFixedUpdate();
		}

		OnItemReachDestination();

		yield break;
	}

	public virtual void OnItemReachDestination()
	{
		if (itemData.itemType == ItemTypes.Skillpoint)
		{
			player.PlayerInventory.AddSkillPoint(1);
		}
		else
		{
			player.PlayerInventory.AddItem(itemData, 1);
		}

		Destroy(gameObject);
	}
}
