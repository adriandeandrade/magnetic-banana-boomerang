using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class RangedEnemy1 : BaseEnemy
{
	// Inspector Field
	[Header("Ranged Enemy Configuration")]
	[SerializeField] private float minAttackDistance;
	[SerializeField] private float maxAttackDistance;
	[SerializeField] private RangedEnemyCharacterData rangedEnemyCharacterData;

	// Private Variables
	private bool isShooting = false;
	private bool doShooting = false;
	private float currentFireDelay;
	private float currentFireRateTime;

	// Properties

	// Components 

	// Events
	public event System.Action OnShoot;

	public override void Start()
	{
		base.Start();
		SetState(EnemyStates.IDLE);
	}

	public override void Update()
	{
		base.Update();
	}

	public override void InitIdle()
	{

	}

	public override void InitMoving()
	{
		doShooting = false;
		ExecuteMove(target.position);
	}

	public override void InitInteracting()
	{

	}

	public override void Idle()
	{
		SetState(EnemyStates.MOVING);
	}

	public override void Moving()
	{
		if (distanceToTarget <= maxAttackDistance * maxAttackDistance)
		{
			if (!isShooting)
			{
				doShooting = true;
				StartCoroutine(Shoot());
			}
		}

		ExecuteMove(target.position);
	}

	public override void Interacting()
	{
		if (!isShooting)
		{
			doShooting = true;
			StartCoroutine(Shoot());
		}

		if (distanceToTarget > maxAttackDistance * maxAttackDistance)
		{
			SetState(EnemyStates.MOVING);
		}
	}

	private void ShootProjectile()
	{
		GameObject projectile = Instantiate(rangedEnemyCharacterData.projectilePrefab, transform.position, Quaternion.identity);
		projectile.GetComponent<BaseProjectile>().ShootProjectile(target.position, 10f);

		if(OnShoot != null)
		{
			OnShoot.Invoke();
		}
	}

	private IEnumerator Shoot()
	{
		isShooting = true;

		while (doShooting)
		{
			ShootProjectile();
			yield return new WaitForSeconds(rangedEnemyCharacterData.fireRate);
		}

		isShooting = false;
		yield break;
	}
}


