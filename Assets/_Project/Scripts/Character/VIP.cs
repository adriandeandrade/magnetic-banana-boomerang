﻿using System.Collections;
using System.Collections.Generic;
using MagneticBananaBoomerang.Characters;
using UnityEngine;

public class VIP : BaseEnemy
{
	// Inspector Fields
	[Header("VIP Configuration")]
	[SerializeField] private float wanderDelay;
	public float maxRadiusFromTarget;
	public float minRadiusFromTarget;

	// [SerializeField] private float moveSpeed; /* MOVE SPEED IS IMPLEMENTED IN BaseCharacterMovement */

	[HideInInspector] public Transform target;
	[HideInInspector] public Vector3 currentDestination;

	// Private Variables
	private float currentWanderDelayTime;

	// Extra States
	EnemyStates wanderState = (EnemyStates)4; // WANDER IS NOT IN OUR BASE ENEMY STATE ENUM SO WE HAVE TO ADD IT AT RUNTIME

	public override void Start()
	{
		base.Start();
		target = FindObjectOfType<Player>().transform;
	}

	public override void SetState(EnemyStates newState)
	{
		base.SetState(newState);

		switch (newState)
		{
			case (EnemyStates)4:
				currentState = wanderState;
				InitWanderState();
				break;
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();

		switch (currentState)
		{
			case (EnemyStates)4:
				UpdateWanderState();
				break;
		}
	}

	public override void InitIdleState()
	{
        currentWanderDelayTime = wanderDelay;
	}

	public void InitWanderState()
	{
		Vector3 nextWanderPoint = GetRandomDestination();
		agent.SetDestination(nextWanderPoint);
	}

	public override void UpdateIdleState()
	{
		if (currentWanderDelayTime <= 0)
		{
			currentWanderDelayTime = 0;
			SetState(wanderState); // Goto the wander state. // Pick destination to wander too.
		}
		else
		{
			currentWanderDelayTime -= Time.deltaTime;
		}
	}

	public void UpdateWanderState()
	{
		if(HasReachedDestination())
        {
            SetState(EnemyStates.IDLE);
        }
	}

    public bool HasReachedDestination()
    {
        if(!agent.pathPending)
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                if(!agent.hasPath)
                {
                    return true;
                }
            }
        }
        return false;
    }

	private Vector3 GetRandomDestination()
	{
		Vector3 targetPos = target.transform.position;
		float randRad = Random.Range(minRadiusFromTarget, maxRadiusFromTarget);
		float randAngle = Random.Range(0, 2 * Mathf.PI);

		// Convert to polar to cartesian coordinate
		float x = randRad * Mathf.Cos(randAngle);
		float y = randRad * Mathf.Sin(randAngle);
		Vector3 dest = new Vector3(x, y, 0);

		return targetPos + dest;
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
}
