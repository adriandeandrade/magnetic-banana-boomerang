using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using PathCreation;

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
			SpawnBoomerang();
		}
	}

	private void SpawnBoomerang()
	{
		GeneratePath();

		currentBoomerangInstance = Instantiate(boomerangPrefab, player.transform.position, Quaternion.identity);
		currentBoomerangInstance.GetComponent<Boomerang>().InitializeBoomerang(this, pathCreator);

		canThrowBoomerang = false;

		// Set the boomerang land position marker.
		boomerangLandPosition.gameObject.SetActive(true);
		boomerangLandPosition.position = pathCreator.path.localPoints[pathCreator.path.localPoints.Length - 1];
	}

	public void ResetController()
	{
		canThrowBoomerang = true;
		boomerangLandPosition.gameObject.SetActive(false);
		print("Controller reset");
	}

	private void GeneratePath()
	{
		List<Vector2> points = new List<Vector2>(); // Create an empty list to store the points of the path.

		points.Add(player.transform.position); // Set the first point to be the position of this object. (Player)
		points.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition)); // Set the second point to mouse position in world spacej.

		BezierPath bezierPath = new BezierPath(points, true, PathSpace.xy); // Create and assign a new BezierPath.
		pathCreator.bezierPath = bezierPath;
	}
}
