using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class Player : BaseCharacter
{
	// Inspector Fields
	[SerializeField] private PlayerData playerData;

	// Private Variables
	private Inventory inventory;

	// Properties;
	public Inventory PlayerInventory
	{
		get
		{
			if (inventory != null)
			{
				return inventory;
			}
			else
			{
				inventory = GetComponent<Inventory>();
				return inventory;
			}
		}
	}

	public override void Awake()
	{
		base.Awake();
		inventory = GetComponent<Inventory>();
	}

	public override void Update()
	{
		HandleMovement();
		base.Update();

		// Debug: TODO: Remove this!
		if(Input.GetKeyDown(KeyCode.P))
		{
			TakeDamage(5f, Vector2.zero);
		}
	}

	private void HandleMovement()
	{
		// Get input
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		// Clamp the magnitude so we cant move faster diagonally.
		Vector2 vel = new Vector2(horizontal, vertical);
		vel = Vector2.ClampMagnitude(vel, 1);

		base.Update();

		if (!knockback.IsKnockback)
		{
			Move(vel);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Boomerang>())
		{
			Boomerang boomerang = other.GetComponent<Boomerang>();
			boomerang.Pickup();

			print("Triggered boomerang.");
		}
	}
}
