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

	private void Awake()
	{
		InitializeSingleton();
		AddBoomerangManager();
	}

	private void AddBoomerangManager()
	{
		boomerangManager = gameObject.AddComponent<BoomerangManager>();
		boomerangManager.Initialize();
	}
}
