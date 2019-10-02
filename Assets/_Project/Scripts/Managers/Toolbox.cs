using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolbox : MonoBehaviour
{
	#region Singleton
	public static Toolbox instance;

	private void InitializeSingleton()
	{
		if (instance == null)
			instance = this;
		else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

	#endregion

	// Managers
	private BoomerangManager boomerangManager;
	private GameManager gameManager;

	private void Awake()
	{
		InitializeSingleton();

		gameManager = GetComponent<GameManager>();
		gameManager.Initialize();

		AddManagers();
	}

	private void AddManagers()
	{
		boomerangManager = gameObject.AddComponent<BoomerangManager>();
		boomerangManager.Initialize();
	}

	public BoomerangManager GetBoomerangManager()
	{
		return boomerangManager;
	}

	public GameManager GetGameManager()
	{
		return gameManager;
	}
}
