using System.Collections;
using System.Collections.Generic;
using MagneticBananaBoomerang.Characters;
using UnityEngine;
using PolyNav;

public class VIP1 : BaseCharacter, IAICharacter
{
	// Inspector Field
	[SerializeField] private EnemyStates currentState;
	[SerializeField] private float fleeThreshold; // The closest an enemy can be before the turtle flees.
	[SerializeField] private float fleeDestinationRadius;
	[SerializeField] private float knockbackRange;
	[SerializeField] private float dodgeRadius;
	[SerializeField] private float dodgeCooldown;
	[SerializeField] private float outOfRangeMoveCooldown;
	[SerializeField] private float fleeCooldown;

	// Private variables
	//private Transform dodgeTarget; // The target which the vip will focus dodging incoming projectiles from.
	private RangedEnemy dodgeTarget;
	private bool isFleeing = false;
	private bool isDodging = false;
	private float currentDodgeTime;
	private float currentFleeTime;
	private float currentOutOfRangeTime;
	private List<BaseEnemy> enemies = new List<BaseEnemy>();

	// Components
	private PolyNavAgent agent;
	private Camera cam;
	private Player player;

	// Properties
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
		cam = Camera.main;
	}

	public override void Start()
	{
		base.Start();
		player = Toolbox.instance.GetGameManager().PlayerRef;
	}

	public override void Update()
	{
		UpdateAnimator();

		base.Update();

		UpdateState();
		SetTarget();
	}

	public void SetState(EnemyStates newState)
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

	public void UpdateState()
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

		SearchForNearbyEnemies();
	}


	public void InitIdle()
	{
		currentOutOfRangeTime = outOfRangeMoveCooldown;
	}


	public void InitMoving()
	{

	}

	public void InitInteracting()
	{

	}

	public void Idle()
	{
		if(!CheckIfWithinRange())
		{
			if(currentOutOfRangeTime <= 0)
			{
				// Move back to player.
				GetRandomPointAroundPlayer();
			}
			else
			{
				currentOutOfRangeTime -= Time.deltaTime;
			}
		}
	}

	public void Moving()
	{

	}

	public void Interacting()
	{

	}

	public override void TakeDamage(float amount, Vector2 damageDirection)
	{
		base.TakeDamage(amount, damageDirection);
		Flee();
	}

	private void SearchForNearbyEnemies()
	{
		enemies.Clear();

		Collider2D[] _enemies = Physics2D.OverlapCircleAll(transform.position, fleeThreshold);

		if (enemies != null && _enemies.Length > 0 && !isFleeing)
		{
			foreach (Collider2D col in _enemies)
			{
				if (col.GetComponent<BaseEnemy>())
				{
					enemies.Add(col.GetComponent<BaseEnemy>());
					Flee();
				}
			}
		}
	}

	private void Dodge()
	{
		if (enemies.Count > 0 && enemies != null)
		{
			dodgeTarget = enemies[0].GetComponent<RangedEnemy>();

			if (dodgeTarget)
			{
				RaycastHit2D hit = Physics2D.Linecast(transform.position, dodgeTarget.transform.position);
				if (hit)
				{
					StartCoroutine(DodgeTimer());
					dodgeTarget.OnShoot -= Dodge;
				}
			}
		}
	}

	public void ApplyKnockback()
	{
		List<GameObject> otherObjects = DetectObjectsWithinRadius(transform, knockbackRange);

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

	protected List<GameObject> DetectObjectsWithinRadius(Transform centerPoint, float detectionRadius)
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll(centerPoint.position, detectionRadius);
		List<GameObject> otherObjects = new List<GameObject>();

		if (cols.Length > 0)
		{
			foreach (Collider2D col in cols)
			{
				if (col.gameObject != null && col != this)
				{
					otherObjects.Add(col.gameObject);
				}
			}
		}

		return otherObjects; // Return empty list.
	}

	private void SetTarget()
	{
		RangedEnemy[] es = FindObjectsOfType<RangedEnemy>();
		enemies.AddRange(es);
	}

	private void Flee()
	{
		if (!isFleeing)
		{
			StartCoroutine(FleeTimer());

			Vector2 randomPos = GameUtilities.GetRandomPointOnCircle(fleeDestinationRadius);
			agent.SetDestination(randomPos);
			SetState(EnemyStates.MOVING);
		}
	}

	private void GetRandomPointAroundPlayer()
	{
		Vector2 newPosition = Random.insideUnitCircle * 3f + (Vector2)player.transform.position;
		agent.SetDestination(newPosition);
		SetState(EnemyStates.MOVING);
	}

	private bool CheckIfWithinRange()
	{
		Vector3 screenPoint = cam.WorldToViewportPoint(transform.position);

		bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
		return onScreen;
	}

	private void OnDestinationReached()
	{
		if (isFleeing) isFleeing = false;
		SetState(EnemyStates.IDLE);
	}

	private IEnumerator DodgeTimer()
	{
		currentDodgeTime = dodgeCooldown;
		isDodging = true;

		while (currentDodgeTime > 0)
		{
			currentDodgeTime -= Time.deltaTime;
			yield return null;
		}

		isDodging = false;
		yield break;
	}

	private IEnumerator FleeTimer()
	{
		currentFleeTime = fleeCooldown;
		isFleeing = true;

		while (currentFleeTime > 0)
		{
			currentFleeTime -= Time.deltaTime;
			yield return null;
		}

		isFleeing = false;
		yield break;
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
}