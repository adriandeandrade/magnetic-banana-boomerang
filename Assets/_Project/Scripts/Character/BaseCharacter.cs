using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MagneticBananaBoomerang.Characters
{
	[RequireComponent(typeof(Knockback))]
	public abstract class BaseCharacter : BaseCharacterMovement, IDamageable
	{
		// Inspector Fields
		[SerializeField] protected BaseCharacterData characterData;
		[SerializeField] protected Color damageColor = Color.red;
		[SerializeField] protected Image healthbar;

		// Private Fields
		protected float currentHealth;

		// Components
		[HideInInspector]
		public Knockback knockback;

		public override void Awake()
		{
			base.Awake();
			knockback = GetComponent<Knockback>();
		}

		public override void Update()
		{
			base.Update();
		}

		public virtual void Start()
		{
			currentHealth = characterData.health.statBaseValue;
		}

		public virtual void TakeDamage(float amount, Vector2 damageDirection)
		{
			RecalculateHealth(amount);

			if (currentHealth <= 0)
			{
				OnDeath();
				return;
			}

			knockback.ApplyKnockback(damageDirection, damageColor);
		}

		public virtual void OnDeath()
		{
			Destroy(gameObject);
		}

		public virtual void RecalculateHealth(float amount)
		{
			currentHealth -= amount;

			if (healthbar != null)
			{
				healthbar.fillAmount = currentHealth / characterData.health.statBaseValue;
			}
		}

		public virtual void AddHealth(float amount)
		{
			currentHealth += amount;

			if (healthbar != null)
			{
				healthbar.fillAmount = currentHealth / characterData.health.statBaseValue;
			}
		}

		public float GetCurrentHealth()
		{
			return currentHealth;
		}

		public float GetMaxHealth()
		{
			return characterData.health.statBaseValue;
		}

		public virtual void TakeDamage(float amount, Vector2 damageDirection, BaseCharacter damageSender)
		{

		}
	}
}

