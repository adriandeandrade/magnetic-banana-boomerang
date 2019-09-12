using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using PathCreation;
using MagneticBananaBoomerang.Characters;

public class BoomerangManager : BaseManager
{
	// Inspector Fields

	private bool canThrowBoomerang = true;
	private GameObject currentBoomerangInstance; // To keep reference of the boomerang.

	private GameObject boomerangPrefab;
	private GameObject boomerangLandPointPrefab;
	private Transform boomerangLandPosition;

	// Components
	private PathCreator pathCreator;
	private Player player;

	public override void Initialize()
	{
		GameObject boomerangResource = Resources.Load("Prefabs/Boomerang") as GameObject;
		GameObject landPointResource = Resources.Load("Prefabs/BoomerangLandPoint") as GameObject;

		boomerangPrefab = boomerangResource;
		boomerangLandPointPrefab = landPointResource;

		boomerangLandPosition = Instantiate(boomerangLandPointPrefab).transform;
	}

	private void OnEnable()
	{
		Boomerang.OnPickedUp += ResetController;
	}

	public PathCreator GetPathCreator()
	{
		return pathCreator;
	}

	private void OnDisable()
	{
		Boomerang.OnPickedUp -= ResetController;
	}

	private void Awake()
	{
		if (pathCreator == null)
		{
			pathCreator = FindObjectOfType<PathCreator>();
		}

		player = FindObjectOfType<Player>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && canThrowBoomerang)
		{
			SpawnBoomerang(player.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}

	public void SpawnBoomerang(Vector2 point1, Vector2 point2)
	{
		GameObject objectHit = GetObjectUnderMouse.GetObject(); // Get which object is under the mouse when we spawn the boomerang.

		GeneratePath(point1, point2); // Generate the path for the boomerang to move along.

		currentBoomerangInstance = Instantiate(boomerangPrefab, player.transform.position, Quaternion.identity); // Spawn the boomerang.

		if (objectHit)
		{
			Interactable interactable = objectHit.GetComponent<Interactable>(); // Here we check if we hit something that is interactable.
			if (interactable != null)
			{
				currentBoomerangInstance.GetComponent<Boomerang>().InitializeBoomerang(this, pathCreator, interactable.gameObject); // If it is then initialize the boomerang with a reference to the interactable we hit.
			}
			else
			{
				currentBoomerangInstance.GetComponent<Boomerang>().InitializeBoomerang(this, pathCreator); // If not then just throw the boomerang regularlly.
			}
		}
		else
		{
			currentBoomerangInstance.GetComponent<Boomerang>().InitializeBoomerang(this, pathCreator);
		}

		canThrowBoomerang = false;

		// Set the boomerang land position marker.
		boomerangLandPosition.gameObject.SetActive(true);
		boomerangLandPosition.position = pathCreator.path.localPoints[pathCreator.path.localPoints.Length - 1]; // Set the boomerang land point to the final point in the path.

	}

	public void ResetController()
	{
		canThrowBoomerang = true;
		boomerangLandPosition.gameObject.SetActive(false);
		print("Controller reset");
	}

	private void GeneratePath(Vector2 point1, Vector2 point2)
	{
		List<Vector2> points = new List<Vector2>(); // Create an empty list to store the points of the path.

		points.Add(point1); // Set the first point to be the position of this object. (Player)
		points.Add(point2); // Set the second point to mouse position in world spacej.

		BezierPath bezierPath = new BezierPath(points, false, PathSpace.xy); // Create and assign a new BezierPath.
		pathCreator.bezierPath = bezierPath;
	}
}
