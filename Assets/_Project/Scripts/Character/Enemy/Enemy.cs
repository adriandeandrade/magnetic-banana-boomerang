using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class Enemy : MonoBehaviour
{
	// Inspector Fields
	[Header("Enemy Configuration")]
	[SerializeField] private bool requireTarget = false;
	[SerializeField] private bool useAI = true;
	[SerializeField] private Transform target;
	[SerializeField] private float moveableRadius = 20f;
	[SerializeField] private float inViewRadius = 10f;
	[SerializeField] private float searchSpeed;
	[SerializeField] private float moveSpeed;


	// Private Variables
	private Vector3 directionToTarget;
	private Vector3 directionAwayFromTarget;
	private Vector3 lastKnownTargetPosition;
	private float distanceToTarget;
	private bool performAI;
	private bool targetHasBeenSeen;
	private bool lostSightOfTarget;

	// Components
	private PolyNavAgent agent;
    

	private void Awake()
	{
        agent = GetComponent<PolyNavAgent>();
        
	}

	private void Update()
	{
		if (!useAI)
		{
			return;
		}
		else
		{
			PerformCalculations();
		}
	}

	private void PerformCalculations()
	{
		if (!target && requireTarget)
		{
			return;
		}

		CalculateLastKnowPosition();
		CalculateDirectionToTarget();
		CalculateDirectionAwayFromTarget();
		CalculateDistanceToTarget();

		if (!requireTarget)
		{
			// TODO: Roaming
		}
		else if (HasTargetInSight())
		{

		}
	}

	private bool HasTargetInSight()
	{
		if (moveableRadius > 0 && distanceToTarget > moveableRadius)
		{
			performAI = false;
		}
		else
		{
			performAI = true;
		}

		if (inViewRadius > 0 && distanceToTarget > inViewRadius)
		{
			return false;
		}

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

	/* private IEnumerator FindTarget(Vector3 lastKnowPosition)
	{
		lostSightOfTarget = true;
		while (lostSightOfTarget)
		{
			Vector3 directionToLastKnownPosition = lastKnowPosition - transform.position;
		}
	} */

	private void MoveTowards(Vector3 direction)
	{
		direction.z = 0;

		float speed = moveSpeed;
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
		distanceToTarget = directionToTarget.sqrMagnitude;
	}
}
