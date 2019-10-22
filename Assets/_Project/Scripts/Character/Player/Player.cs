using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class Player : BaseCharacter
{
	// Inspector Fields
	[SerializeField] private PlayerData playerData;
    [SerializeField] private Stat healthStat;
    [SerializeField] private Stat speedStat;

	// Private Variables
	private Inventory inventory;
	private StatManager statManager;

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

	public StatManager PlayerStats
	{
		get
		{
			if(statManager != null)
			{
				return statManager;
			}
			else
			{
				statManager = GetComponent<StatManager>();
				return statManager;
			}
		}
	}

	public override void Awake()
	{
		base.Awake();
		inventory = GetComponent<Inventory>();
		statManager = GetComponent<StatManager>();
        statManager.OnStatUpgraded += OnStatsUpgraded;

    }

    public override void Start()
    {
        healthStat = statManager.GetStatWithName("gorilla_base_health");
        speedStat = statManager.GetStatWithName("gorilla_base_speed");
        currentHealth = healthStat.currentValue;
        moveSpeed = speedStat.currentValue;
    }

    public override void Update()
	{
		HandleMovement();
		base.Update();
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

    public override void OnDeath()
    {
        Toolbox.instance.GetGameManager().GameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Boomerang>())
		{
			Boomerang boomerang = other.GetComponent<Boomerang>();
			boomerang.Pickup();

			//print("Triggered boomerang.");
		}
	}
    private void OnStatsUpgraded()
    {
        currentHealth = healthStat.currentValue;
        moveSpeed = speedStat.currentValue;
        RecalculateHealth(0);
    }

    public override void RecalculateHealth(float amount)
    {
        currentHealth -= amount;

        if (healthbar != null)
        {
            healthbar.fillAmount = currentHealth / healthStat.baseValue;
        }
    }

	public PlayerData GetPlayerData()
	{
		return playerData;
	}
}
