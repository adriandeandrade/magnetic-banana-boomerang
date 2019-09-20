using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class SpikeTrap : Trap
{
	// Inspector Fields
    [Tooltip("Object which holds the spikes graphics.")]
	[SerializeField] private GameObject spikeSprites;
    [Tooltip("The amount of damage the spikes will deal.")]
    [SerializeField] private float spikeDamageAmount;

	public override void Activate()
	{
		InitializeTimer();
        spikeSprites.SetActive(true);
        ApplyDamage();
	}

	public override void Deactivate()
	{
		active = false;
		spikeSprites.SetActive(false);
	}

    private void ApplyDamage()
	{
		List<BaseEnemy> detectedEnemies = DetectEnemiesWithinRadius(transform, range);

		if (detectedEnemies.Count > 0)
		{
			foreach (BaseEnemy enemy in detectedEnemies)
			{   
				Vector2 dir = enemy.transform.position - transform.position;
                enemy.GetComponent<IDamageable>().TakeDamage(spikeDamageAmount, dir);
			}
		}
	}
}
