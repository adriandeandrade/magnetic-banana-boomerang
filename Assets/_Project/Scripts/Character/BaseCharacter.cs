using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagneticBananaBoomerang.Characters
{
    [RequireComponent(typeof(Knockback))]
	public abstract class BaseCharacter : BaseCharacterMovement, IDamageable
	{
		// Inspector Fields
		[SerializeField] protected BaseCharacterData characterData;
		[SerializeField] protected Color damageColor = Color.red;

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

		public virtual void Start()
		{
            currentHealth = characterData.maxHealth;
		}

		public void TakeDamage(float amount, Vector2 damageDirection)
		{
			RecalculateHealth(amount);

			if (currentHealth <= 0)
			{
				Destroy(gameObject);
				return;
			}

			//knockback.ApplyKnockback(damageDirection, damageColor);
		}

		public virtual void RecalculateHealth(float amount)
		{
			currentHealth -= amount;
			// Update UI here
		}
	}
}

