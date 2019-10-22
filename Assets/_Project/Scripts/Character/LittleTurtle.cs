using System.Collections;
using System.Collections.Generic;
using MagneticBananaBoomerang.Characters;
using UnityEngine;
using PolyNav;

public class LittleTurtle : BaseCharacter, IAICharacter
{
	// Inspector Field
	[Header("Little Turtle Settings")]
	[SerializeField] private float fleeThreshold; // How close an enemy can be before the vip starts moving away.
	[SerializeField] private float fleeDestinationSearchRadius; // The radius of the circle which the vip will pick a point within when it flees.
	[SerializeField] private float defensiveKnockbackRadius; // Anything within this radius will be knocked back when the knockback is activated.
	[SerializeField] private float circleAroundPlayerRadius; // For when the turtle picks a point to move to near the player.
	[SerializeField] private float outOfBoundsCooldown; // Amount of time it takes before the vip begins moving when it is out of range of the camera.
	[SerializeField] private float fleeCooldown; // Amount of time before the vip begins fleeing.
    [SerializeField] private float qteCooldown = 3f;
	[SerializeField] private QuickTimeEventSystem quickTimeEvent;
    [SerializeField] private Stat healthStat;
    [SerializeField] private Stat speedStat;

	// Private variables
	private EnemyStates currentState; // The vip's current state. (Same states as the enemy types.)

	private bool isFleeing = false;

	private float currentFleeTime; // Flee timer.
	private float currentOutOfRangeTime; // Out of range timer (Used for when the vip is out of view of the camera.)
    private float currentQTETime;

	// Components
	private PolyNavAgent agent;
	private Camera cam;
	private Player player;
    private StatManager statManager;

	// Properties
	public EnemyStates CurrentState { get => currentState; set => currentState = value; }
	public float FleeThreshold { get => fleeThreshold; }
	public float PickFleeDestinationRadius { get => fleeDestinationSearchRadius; }
	public float DefensiveKnockbackRadius { get => defensiveKnockbackRadius; }

	private void OnEnable()
	{
		agent.OnDestinationReached += OnDestinationReached; // Subscribe to OnDestinationReached event. Is raised when the agent reaches a given destination.
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
        statManager = FindObjectOfType<StatManager>();
        statManager.OnStatUpgraded += OnStatsUpgraded;
    }

	public override void Start()
	{
        healthStat = statManager.GetStatWithName("turtle_base_health");
        speedStat = statManager.GetStatWithName("turtle_base_speed");
        currentHealth = healthStat.currentValue;
        moveSpeed = speedStat.currentValue;
        agent.maxSpeed = moveSpeed;

		player = Toolbox.instance.GetGameManager().PlayerRef; // Get reference to player.
		quickTimeEvent.OnKeyPressedOnTime += FillHealthBar;
	}

	public override void Update()
	{
		UpdateAnimator();

        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            quickTimeEvent.StartQuickTimeEvent();
        }

        if(currentQTETime > 0)
        {
            currentQTETime -= Time.deltaTime;
        }

		base.Update();

		UpdateState();

		if (!CheckIfWithinRange())
		{
			if (currentOutOfRangeTime <= 0)
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
		currentOutOfRangeTime = outOfBoundsCooldown;
	}


	public void InitMoving()
	{

	}

	public void InitInteracting()
	{

	}

	public void Idle()
	{
		
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

		if (currentHealth / 10 <= 0.3f && currentQTETime <= 0)
		{
			quickTimeEvent.StartQuickTimeEvent();
            currentQTETime = qteCooldown;
		}

		Flee();
	}

	private void SearchForNearbyEnemies()
	{
		Collider2D[] _enemies = Physics2D.OverlapCircleAll(transform.position, fleeThreshold);

		if (_enemies != null && _enemies.Length > 0 && !isFleeing)
		{
			foreach (Collider2D col in _enemies)
			{
				if (col.GetComponent<BaseEnemy>())
				{
					Flee();
				}
			}
		}
	}

	private void FillHealthBar()
	{
		AddHealth(characterData.health.statBaseValue);
	}

	public void ApplyKnockback()
	{
		List<GameObject> otherObjects = DetectObjectsWithinRadius(transform, defensiveKnockbackRadius);

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

	private void Flee()
	{
		if (!isFleeing && currentFleeTime <= 0)
		{
			StartCoroutine(FleeTimer());

			Vector2 randomPos = GameUtilities.GetRandomPointOnCircle(fleeDestinationSearchRadius);
			agent.SetDestination(randomPos);
			SetState(EnemyStates.MOVING);
		}
	}

	private void GetRandomPointAroundPlayer()
	{
		Vector2 newPosition = Random.insideUnitCircle * circleAroundPlayerRadius + (Vector2)player.transform.position;
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

    public override void OnDeath()
    {
        Toolbox.instance.GetGameManager().GameOver();
    }

    private void OnStatsUpgraded()
    {
        currentHealth = healthStat.currentValue;
        moveSpeed = speedStat.currentValue;
        agent.maxSpeed = moveSpeed;
        RecalculateHealth(0);
    }

    public override void RecalculateHealth(float amount)
    {
        currentHealth -= amount;

        if (healthbar != null)
        {
            healthbar.fillAmount = currentHealth / healthStat.baseValue;
        }
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