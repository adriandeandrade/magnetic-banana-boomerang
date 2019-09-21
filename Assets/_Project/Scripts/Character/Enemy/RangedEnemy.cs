using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class RangedEnemy : BaseEnemy
{
	// This enemy will start shooting when it moves within the minimum shoot distance. If the player moves further from the enemy
	// while it is shooting, the enemy will continue shooting until the player is outside the max shoot distance. Then repeat. 

	// Inspector Fields
	[Header("Ranged Enemy Configuration")]
	[Tooltip("The minimum distance the enemy can be from the player to start shooting.")]
	[SerializeField] private float minShootDistance;
	[Tooltip("The maximum distance the enemy can be from the player to start shooting.")]
	[SerializeField] private float maxShootDistance;
	[SerializeField] private RangedEnemyCharacterData rangedEnemyCharacterData;
	[SerializeField] private GameObject[] targets;

	// [SerializeField] private float moveSpeed; /* MOVE SPEED IS IMPLEMENTED IN BaseCharacterMovement */

	[HideInInspector] public Transform currentTarget;

	// Private Variables
	private bool canShoot;
	private bool useNearestAsTarget = true;
	private float currentFireDelay;
	private float currentFireRateTime;

	private void OnEnable()
	{
        Boomerang.OnHitEnemy += SwitchTargetToPlayer; 
	}

	public override void Awake()
	{
		base.Awake();
		targets[0] = GameObject.FindGameObjectWithTag("Player");
		targets[1] = GameObject.FindGameObjectWithTag("VIP");
	}

	public override void Start()
	{
		base.Start();
		currentTarget = FindObjectOfType<Player>().transform;
		SetState(EnemyStates.IDLE);
		agent.stoppingDistance = minShootDistance;
	}

	public override void Update()
	{
		base.Update();

		if (currentFireRateTime <= 0)
		{
			canShoot = true;
		}
		else
		{
			currentFireRateTime -= Time.deltaTime;
			canShoot = false;
		}

		if (useNearestAsTarget)
		{
			SetTarget();
		}
	}

	public override void InitIdleState()
	{
		if (currentTarget != null)
		{
			SetState(EnemyStates.MOVING);
		}
	}

	public override void InitMovingState()
	{
		agent.SetDestination(currentTarget.transform.position);
	}

	public override void InitAttackState()
	{
		//currentFireDelay = rangedEnemyCharacterData.fireDelay;
	}

	public override void UpdateIdleState()
	{

	}

	public override void UpdateMovingState()
	{
		if (HasReachedDestination())
		{
			if (GetDistanceToTarget() <= agent.stoppingDistance + minShootDistance)
			{
				SetState(EnemyStates.ATTACKING);
			}
		}
		else
		{
			agent.SetDestination(currentTarget.position);
		}
	}

	public override void UpdateAttackState()
	{
		if (GetDistanceToTarget() >= agent.stoppingDistance + maxShootDistance)
		{
			SetState(EnemyStates.IDLE);
		}
		else
		{
			if (canShoot)
			{
				GameObject projectile = Instantiate(rangedEnemyCharacterData.projectilePrefab, transform.position, Quaternion.identity);
				Vector3 direction = (currentTarget.position - transform.position).normalized;

				projectile.GetComponent<BaseProjectile>().ShootProjectile(currentTarget.position, 3f);
				Debug.Log("Shot");
				currentFireRateTime = rangedEnemyCharacterData.fireRate;
			}
		}
	}

	private float GetDistanceToTarget()
	{
		return Vector3.Distance(currentTarget.position, transform.position) + agent.stoppingDistance;
	}

	private GameObject GetClosestTarget()
	{
		return GameUtilities.FindClosestGameObject(targets, transform.position);
	}

	private void SetTarget()
	{
		currentTarget = GetClosestTarget().transform;
	}

    private void SwitchTargetToPlayer()
    {
        useNearestAsTarget = false;
        currentTarget = FindObjectOfType<Player>().transform;
    }

	private bool HasReachedDestination()
	{
		if (!agent.pathPending)
		{
			if (agent.remainingDistance <= agent.stoppingDistance)
			{
				if (!agent.hasPath)
				{
					return true;
				}
			}
		}
		return false;
	}
}
