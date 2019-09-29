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
	[SerializeField] private float fleeDestinationRadius; // The radius that the 
	[SerializeField] private float dodgeRadius;
	[SerializeField] private float dodgeCooldown;
	[SerializeField] private float fleeCooldown;

	// Private variables
	//private Transform dodgeTarget; // The target which the vip will focus dodging incoming projectiles from.
	private RangedEnemy1 dodgeTarget;
	private bool isFleeing = false;
	private bool isDodging = false;
	private float currentDodgeTime;
	private float currentFleeTime;
	private List<BaseEnemy> enemies = new List<BaseEnemy>();

	// Components
	private PolyNavAgent agent;

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
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
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
			dodgeTarget = enemies[0].GetComponent<RangedEnemy1>();

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

	private void SetTarget()
	{
		if (enemies.Count > 0 && enemies != null)
		{
			dodgeTarget = enemies[0].GetComponent<RangedEnemy1>();
			dodgeTarget.OnShoot += Dodge;
		}
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
}
























/* public GameObject target;
public float maxRadius;
public float minRadius;
public float speed;

public Vector3 currentDest;

enum State
{
None,
Enroute,
Ponder
}
private State state;
Rigidbody2D body;
// Start is called before the first frame update
void Start ()
{
body = GetComponent<Rigidbody2D> ();
state = State.None;
StartCoroutine ("Ponder");
}

// Update is called once per frame
void Update ()
{
if (state == State.Enroute)
{
	if (Vector3.Distance (transform.position, currentDest) < 0.3)
	{
		state = State.Ponder;
		body.velocity = Vector3.zero;
		StartCoroutine ("Ponder");
	}
	else
	{

		Vector3 direction = (currentDest - transform.position).normalized;
		body.velocity = direction * speed * Time.deltaTime;
	}
}
if (Vector3.Distance(currentDest, target.transform.position) > maxRadius)
{
	currentDest = genRandomDest();
	state = State.Enroute;
}
}

IEnumerator Ponder()
{
float randWait = Random.Range(1f, 2f);
print(randWait);
yield return new WaitForSeconds(randWait);
if (state != State.Enroute)
{
	currentDest = genRandomDest();
	state = State.Enroute;
}
}

Vector3 genRandomDest()
{
Vector3 targetPos = target.transform.position;
float randRad = Random.Range (minRadius, maxRadius);
float randAngle = Random.Range (0, 2 * Mathf.PI);
// Convert to polar to cartesian coordinate
float x = randRad * Mathf.Cos(randAngle);
float y = randRad * Mathf.Sin(randAngle);
Vector3 dest = new Vector3(x, y, 0);
return targetPos + dest;
} */

