using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using PathCreation;
using MagneticBananaBoomerang.Characters;

public class BoomerangManager : BaseManager
{
	// Inspector Fields
	[SerializeField] private KeyCode shootKey = KeyCode.Mouse0;

	// Private Variables
	private bool canThrowBoomerang = true;
	private Boomerang currentBoomerangInstance; // To keep reference of the boomerang.

	private GameObject boomerangPrefab;

	// Components
	private Player player;

	public override void Initialize()
	{
		GameObject boomerangResource = Resources.Load("Boomerang") as GameObject;
		boomerangPrefab = boomerangResource;
	}

	private void Awake()
	{
		player = FindObjectOfType<Player>();
	}

	private void Update()
	{
		SpawnBoomerang();
	}

	public void SpawnBoomerang()
	{
		if (Input.GetKeyDown(shootKey) && canThrowBoomerang)
		{
			GameObject objectUnderMouse = GetObjectUnderMouse.GetObject();
			if (objectUnderMouse)
			{
				currentBoomerangInstance = Instantiate(boomerangPrefab, player.transform.position, Quaternion.identity).GetComponent<Boomerang>();
				currentBoomerangInstance.OnBoomerangPickedUpAction += OnBoomerangPickedUp;
				currentBoomerangInstance.InitializeBoomerang(this, objectUnderMouse.transform.position, objectUnderMouse);
				//print("Object found. Interacting...");
			}
			else
			{
				currentBoomerangInstance = Instantiate(boomerangPrefab, player.transform.position, Quaternion.identity).GetComponent<Boomerang>();
				currentBoomerangInstance.OnBoomerangPickedUpAction += OnBoomerangPickedUp;
				currentBoomerangInstance.InitializeBoomerang(this, Camera.main.ScreenToWorldPoint(Input.mousePosition));
				//print("Object not detected, Regular throw initialized.");
			}

			canThrowBoomerang = false;
		}
	}

	public void OnBoomerangPickedUp()
	{
		canThrowBoomerang = true;
	}
}
