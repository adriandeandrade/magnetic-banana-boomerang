using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

[RequireComponent(typeof(Knockback))]
public class KnockbackTrap : Trap
{
	public override void Activate()
	{
		InitializeTimer();

		if (spriteRenderer != null)
		{
			spriteRenderer.color = activatedColor;
		}

		ApplyKnockback();
	}

	public override void Deactivate()
	{
		active = false;
		if (spriteRenderer != null)
		{
			spriteRenderer.color = originalColor;
		}
	}

	private void ApplyKnockback()
	{
		List<GameObject> otherObjects = DetectObjectsWithinRadius(transform, range);

		if (otherObjects.Count > 0)
		{
			foreach (GameObject otherObject in otherObjects)
			{
				Vector2 dir = otherObject.transform.position - transform.position;
				BaseCharacter knockbackable = otherObject.GetComponent<BaseCharacter>();

				if (knockbackable != null)
				{
					Debug.Log("Tried to knocback: " + otherObject.name);
					knockbackable.knockback.ApplyKnockback(dir, Color.red);
				}
			}
		}
	}
}
