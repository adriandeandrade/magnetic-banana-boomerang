using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enjoii.Curves;

public class PlayerBoomerang : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] private GameObject boomerangPrefab;
	[SerializeField] private BezierCurve boomerangPath;

	private Vector2[] bezierPoints = new Vector2[3];

	public bool DoUpdatePoints { get => updatePoints; set => updatePoints = value; }

	bool updatePoints = true;

	private void Update()
	{
		if (updatePoints)
		{
			UpdatePoints();
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			SpawnBoomerang(bezierPoints);
		}
	}

	private void SpawnBoomerang(Vector2[] points)
	{
		GameObject boomerang = Instantiate(boomerangPrefab, transform.position, Quaternion.identity);
		boomerang.GetComponent<Boomerang>().StartPath(boomerangPath);
		updatePoints = false;
	}

	private void UpdatePoints()
	{
		boomerangPath.points[0] = transform.position;

		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 point1 = Vector2.zero;

		// If the position of the mouse is to the left of the player then move the second point to be on the left and vise versa.
		if (mousePos.x > transform.position.x)
			point1 = new Vector2(transform.position.x + 2, transform.position.y + 1);
		else if (mousePos.x < transform.position.x)
			point1 = new Vector2(transform.position.x - 2, transform.position.y + 1);

		boomerangPath.points[1] = point1;
		boomerangPath.points[2] = mousePos; // Set the endpoint to be the mouse position.
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(boomerangPath.points[0], 1f);
		Gizmos.DrawWireSphere(boomerangPath.points[1], 1f);
		Gizmos.DrawWireSphere(boomerangPath.points[2], 1f);
	}
}
