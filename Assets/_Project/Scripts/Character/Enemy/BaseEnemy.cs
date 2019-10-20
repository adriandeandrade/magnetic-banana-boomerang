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
		[SerializeField] private bool requireTarget = true;
		[SerializeField] private bool useAI = true;
		[SerializeField] protected float maxDetectionDistance;
		[SerializeField] protected EnemyStates currentState;
		[SerializeField] protected List<GameObject> targets = new List<GameObject>();
		[SerializeField] protected LayerMask lineOfSightFilter;

		// Private Variables
		protected Transform target;
		protected Vector3 directionToTarget;
		protected Vector3 directionAwayFromTarget;
		protected Vector3 lastKnownTargetPosition;
		protected float distanceToTarget;
		protected bool performAI;
		protected bool targetHasBeenSeen;
		protected bool lostSightOfTarget;

		// Components
		protected PolyNavAgent agent;
		protected Player player;

		// Events

		public EnemyStates CurrentState { get => currentState; set => currentState = value; }

		private void OnEnable()
		{
			agent.OnDestinationReached += OnDestinationReached;
		}

		private void OnDisable()
		{
			agent.OnDestinationReached -= OnDestinationReached;
		}

		public override void Awake()
		{
			base.Awake();
			agent = GetComponent<PolyNavAgent>();
			player = FindObjectOfType<Player>();
		}

		public override void Start()
		{
			base.Start();
			GetTargets();
			SetTarget();
			//SetState(EnemyStates.IDLE);
		}

		public override void Update()
		{
			UpdateAnimator();

			if (!requireTarget) return;

			if (targets == null || targets.Count == 0)
			{
				Debug.LogError("Targets cannot be found. Please make sure the enemy has targets it can track.");
				return;
			}


			CalculateLastKnowPosition();
			CalculateDirectionToTarget();
			CalculateDirectionAwayFromTarget();
			CalculateDistanceToTarget();

			if (!requireTarget)
			{
				// TODO: Roaming
				return;
			}

			SetTarget();
			UpdateState();

			base.Update();


		}

		public override void UpdateAnimator()
		{
			animator.SetFloat("Horizontal", Mathf.RoundToInt(agent.velocity.x));
			animator.SetFloat("Vertical", Mathf.RoundToInt(agent.velocity.y));
			animator.SetFloat("Speed", agent.currentSpeed);
			animator.SetFloat("FacingDirection", facingDirection);
		}

		public override void GetFacingDirection()
		{
			if (agent.velocity.normalized == Vector2.zero) return; // If we are standing still return so we idle in the last direction moved.

			if (agent.velocity.normalized == new Vector2(0, -1)) facingDirection = 0f;
			if (agent.velocity.normalized == new Vector2(-1, 1)) facingDirection = 1f;
			if (agent.velocity.normalized == new Vector2(-1, -1)) facingDirection = 2f;
			if (agent.velocity.normalized == new Vector2(1, 0)) facingDirection = 3f;
			if (agent.velocity.normalized == new Vector2(-1, 0)) facingDirection = 4f;
			if (agent.velocity.normalized == Vector2.one) facingDirection = 5f;
			if (agent.velocity.normalized == new Vector2(1, -1)) facingDirection = 6f;
			if (agent.velocity.normalized == new Vector2(0, 1)) facingDirection = 7f;
		}

		public virtual void SetState(EnemyStates newState)
		{
			switch (newState)
			{
				case EnemyStates.INTERACTING:
					currentState = EnemyStates.INTERACTING;
					InitInteracting();
					break;

				case EnemyStates.MOVING:
					currentState = EnemyStates.MOVING;
					InitMoving();
					break;

				case EnemyStates.IDLE:
					currentState = EnemyStates.IDLE;
					InitIdle();
					break;
			}
		}

		public virtual void UpdateState()
		{
			switch (currentState)
			{
				case EnemyStates.INTERACTING:
					Interacting();
					break;

				case EnemyStates.MOVING:
					Moving();
					break;

				case EnemyStates.IDLE:
					Idle();
					break;
			}
		}

		public virtual void InitIdle()
		{

		}

		public virtual void InitMoving()
		{
			ExecuteMove(target.position);
		}

		public virtual void InitInteracting()
		{

		}

		public virtual void Idle()
		{
			SetState(EnemyStates.MOVING);
		}

		public virtual void Moving()
		{

		}

		public virtual void Interacting()
		{

		}

		protected void ExecuteMove(Vector3 destination)
		{
			if (agent.hasPath)
			{
				agent.Stop();
			}

			agent.SetDestination(destination);
		}

		protected virtual void OnDestinationReached()
		{
			agent.Stop();

			if (distanceToTarget <= maxDetectionDistance)
			{
				SetState(EnemyStates.INTERACTING);
			}

			//Debug.Log("Destination Reached!");
		}

		public override void TakeDamage(float amount, Vector2 damageDirection, BaseCharacter damageSender)
		{
			base.TakeDamage(amount, damageDirection);
			FloatingTextController.CreateFloatingText(amount.ToString(), transform);
		}

		public override void OnDeath()
		{
			base.OnDeath();
			Toolbox.instance.GetGameManager().WaveSpawnerInstance.AddEnemyKilled();
		}

		private bool CanSeeTarget()
		{
			RaycastHit2D hit2D = Physics2D.Linecast(transform.position, target.position);

			if (hit2D)
			{
				if (!targetHasBeenSeen && hit2D.transform == target)
				{
					targetHasBeenSeen = true;
				}

				return hit2D.transform == target;
			}
			else
			{
				return false;
			}
		}

		protected void SetTarget()
		{
			if (targets != null && targets.Count > 0)
			{
				Transform closestTarget = GetClosestTarget().transform;
				target = closestTarget;
			}
			else if (targets.Count == 1)
			{
				target = targets[0].transform;
			}
		}

		protected void GetTargets()
		{
			List<BaseCharacter> baseCharacters = new List<BaseCharacter>(FindObjectsOfType<BaseCharacter>());

			foreach (BaseCharacter character in baseCharacters)
			{
				BaseEnemy enemy = character.GetComponent<BaseEnemy>();

				if (!enemy)
				{
					targets.Add(character.gameObject);
				}
			}
		}

		protected GameObject GetClosestTarget()
		{
			if (targets.Count > 0)
			{
				return GameUtilities.FindClosestGameObject(targets, transform.position);
			}
			else
			{
				return null;
			}
		}

		private void CalculateLastKnowPosition()
		{
			lastKnownTargetPosition = target.position;
		}

		private void CalculateDirectionToTarget()
		{
			directionToTarget = lastKnownTargetPosition - transform.position;
		}

		private void CalculateDirectionAwayFromTarget()
		{
			directionAwayFromTarget = transform.position - lastKnownTargetPosition;
		}

		private void CalculateDistanceToTarget()
		{
			distanceToTarget = directionToTarget.sqrMagnitude + agent.stoppingDistance;
		}
	}
}


