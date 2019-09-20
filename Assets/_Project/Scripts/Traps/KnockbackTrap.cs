using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class KnockbackTrap : Trap
{
	public override void Activate()
	{
		InitializeTimer();
		spriteRenderer.color = activatedColor;
		ApplyKnockback();
	}

	public override void Deactivate()
	{
		active = false;
		spriteRenderer.color = originalColor;
	}

	private void ApplyKnockback()
	{
		List<BaseEnemy> detectedEnemies = DetectEnemiesWithinRadius(transform, range);

		if (detectedEnemies.Count > 0)
		{
			foreach (BaseEnemy enemy in detectedEnemies)
			{   
				Vector2 dir = enemy.transform.position - transform.position;
				enemy.knockback.ApplyKnockback(dir, Color.red);
			}
		}
	}
}
