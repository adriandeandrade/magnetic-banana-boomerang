using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

namespace MagneticBananaBoomerang.Characters
{
	[RequireComponent(typeof(PolyNavAgent))]
	public class BaseEnemy : BaseCharacter, IAICharacter
	{
		// Inspector Field
		[Header("Enemy Settings")]
		[SerializeField] protected EnemyStates currentState;

		// Components
		protected PolyNavAgent agent;
		protected Player player;

		public EnemyStates CurrentState { get => currentState; set => currentState = value; }

		public override void Awake()
		{
			base.Awake();
			agent = GetComponent<PolyNavAgent>();
			player = FindObjectOfType<Player>();
		}

		public override void Start()
		{
			base.Start();
			//SetState(EnemyStates.IDLE);
		}

		public override void Update()
		{
			base.Update();
			UpdateState();
		}

		public virtual void SetState(EnemyStates newState)
		{
			switch (newState)
			{
				case EnemyStates.ATTACKING:
					currentState = EnemyStates.ATTACKING;
					InitAttackState();
					break;

				case EnemyStates.MOVING:
					currentState = EnemyStates.MOVING;
					InitMovingState();
					break;

				case EnemyStates.IDLE:
					currentState = EnemyStates.IDLE;
					InitIdleState();
					break;
			}
		}

		public virtual void UpdateState()
		{
			switch (currentState)
			{
				case EnemyStates.ATTACKING:
					UpdateAttackState();
					break;

				case EnemyStates.MOVING:
					UpdateMovingState();
					break;

				case EnemyStates.IDLE:
					UpdateIdleState();
					break;
			}
		}

		public virtual void InitIdleState()
		{
			
		}

		public virtual void InitMovingState()
		{
			
		}

		public virtual void InitAttackState()
		{
			
		}

		public virtual void UpdateIdleState()
		{

		}

		public virtual void UpdateMovingState()
		{
			if (player != null)
			{
				Vector2 currentPosition = transform.position;
				Vector2 directionToPlayer = player.transform.position - transform.position;

				float distanceSqrToTarget = directionToPlayer.sqrMagnitude;

				if (distanceSqrToTarget <= agent.stoppingDistance + 1)
				{
					SetState(EnemyStates.ATTACKING);
				}

				agent.SetDestination(player.transform.position);
				Vector2 target = agent.nextPoint;
				Vector2 direction = (target - (Vector2)transform.position).normalized;
				velocity = direction;
			}
			else
			{
				SetState(EnemyStates.IDLE);
			}
		}

		public virtual void UpdateAttackState()
		{
			Vector2 currentPosition = transform.position;
			Vector2 directionToPlayer = player.transform.position - transform.position;

			float distanceSqrToTarget = directionToPlayer.sqrMagnitude;

			if (distanceSqrToTarget > agent.stoppingDistance + 1)
			{
				SetState(EnemyStates.MOVING);
			}
		}

	}
}


