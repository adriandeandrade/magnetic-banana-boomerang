using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class Trap : MonoBehaviour
{
	[SerializeField] private Color activatedColor;
	[SerializeField] private float activeTime;

	private Color originalColor;
	private float currentActiveTime;
	private bool active = false;

	private SpriteRenderer spriteRenderer;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		originalColor = spriteRenderer.color;
	}

	private void Update()
	{
        if(active)
        {
            if(currentActiveTime <= 0)
            {
                Deactivate();
                return;
            }

            currentActiveTime -= Time.deltaTime;
        }
	}

	public void Activate()
	{
		StartTimer();
		spriteRenderer.color = activatedColor;
        KnockbackEnemiesInPath();
	}

	private void Deactivate()
	{
		active = false;
        spriteRenderer.color = originalColor;
	}

    private void KnockbackEnemiesInPath()
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2f);

		if (cols.Length > 0)
		{
			foreach (Collider2D col in cols)
			{
				BaseEnemy enemy = col.GetComponent<BaseEnemy>();

				if (enemy != null)
				{
					Vector2 dir = col.transform.position - transform.position;
					enemy.knockback.ApplyKnockback(dir, Color.red);
				}
			}
		}
	}

	private void StartTimer()
	{
		currentActiveTime = activeTime;
		active = true;
	}
}
