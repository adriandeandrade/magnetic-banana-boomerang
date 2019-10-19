using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class CreateNewLevelDataWizard : EditorWindow
{
	private static string levelFilePath = "Assets/_Project/Game Data/Levels";
	private enum EditorStates { Init, Editing, Finished }
	private EditorStates editorState = EditorStates.Init;

	// Section Specific Settings
	private int amountOfWaves;
	int waveIndex;

	// Wave Specific Settings
	private int numberOfEnemiesToSpawn;
	private float timeBetweenSpawns;
	private List<GameObject> enemyTypes = new List<GameObject>();

	private bool folderCreated; // To keep track of it whether or not we created the folder. 
	private List<bool> showWaveSettings = new List<bool>(); // Used to keep track of which foldout is open and which is closed.
	private string levelName; // The name of the level.
	private string folderName; // The name of the folder which will hold all the level components.
	private string newPath; // Path that gets created when we create a new level.

	// Window Variables
	float buttonWidth = 100f;
	float buttonHeight = 25f;
	float windowHeight;
	float windowWidth;
	Rect windowSize;

	Wave waveData;

	[MenuItem("Adrian's Tools/Create New Level")]
	private static void Init()
	{
		CreateNewLevelDataWizard window = (CreateNewLevelDataWizard)EditorWindow.GetWindow(typeof(CreateNewLevelDataWizard));
	}

	private void OnGUI()
	{
		var window = (CreateNewLevelDataWizard)EditorWindow.GetWindow(typeof(CreateNewLevelDataWizard));
		GUI.backgroundColor = Color.grey;

		windowSize = window.position;
		windowHeight = windowSize.size.y;
		windowWidth = windowSize.size.x;

		UpdateEditorState();
	}

	private void UpdateEditorState()
	{
		var window = (CreateNewLevelDataWizard)EditorWindow.GetWindow(typeof(CreateNewLevelDataWizard));

		switch (editorState)
		{
			case EditorStates.Init:
				InitState();
				break;

			case EditorStates.Editing:
				EditingState();
				break;

			case EditorStates.Finished:
				FinishedState();
				break;
		}

		// Exit Button
		if (GUI.Button(new Rect((windowWidth / 2) - buttonWidth / 2, windowHeight - (buttonHeight + 10f), buttonWidth, buttonHeight), "Exit"))
		{
			window.Close();
		}
	}

	private void InitState()
	{
		GUILayout.Label("Input a number of waves and press the Begin Editing button", EditorStyles.boldLabel);
		levelName = EditorGUILayout.TextField("Level Name", levelName);
		amountOfWaves = EditorGUILayout.IntField("Amount of Waves", amountOfWaves);

		if (GUI.Button(new Rect((windowWidth / 2) - buttonWidth / 2, windowHeight - (buttonHeight * 3), buttonWidth, buttonHeight), "Begin Editing"))
		{
			if (amountOfWaves == 0)
			{
				EditorUtility.DisplayDialog("Error!", "Amount of waves must be at least 1.", "Ok");
				return;
			}

			if (string.IsNullOrEmpty(levelName))
			{
				EditorUtility.DisplayDialog("Error!", "No level name entered. Please input a level name.", "OK");
				return;
			}
			else
			{
				CreateFolder();
				editorState = EditorStates.Editing;
				waveIndex = 1;
			}
		}
	}

	private void EditingState()
	{
		var window = (CreateNewLevelDataWizard)EditorWindow.GetWindow(typeof(CreateNewLevelDataWizard));

		GUILayout.Label("Input values for each field and press the Save button to configure the next wave.", EditorStyles.boldLabel);
		numberOfEnemiesToSpawn = EditorGUILayout.IntField("Enemy Count", numberOfEnemiesToSpawn);
		GUILayout.Space(2f);
		timeBetweenSpawns = EditorGUILayout.FloatField("Enemy Spawn Delay", timeBetweenSpawns);

		int newEnemyTypeCount = Mathf.Max(0, EditorGUILayout.IntField("Amount of Enemy Types", enemyTypes.Count)); // The amount of types we want in the wave.

		while (newEnemyTypeCount < enemyTypes.Count) // Resize the list so it displays the correct object fields corresponding to the amount of types we want.
		{
			enemyTypes.RemoveAt(enemyTypes.Count - 1);
		}

		while (newEnemyTypeCount > enemyTypes.Count)
		{
			enemyTypes.Add(null);
		}

		for (int i = 0; i < enemyTypes.Count; i++)
		{
			enemyTypes[i] = (GameObject)EditorGUILayout.ObjectField(enemyTypes[i], typeof(GameObject), false);
		}

		if (GUI.Button(new Rect((windowWidth / 2) - buttonWidth / 2, windowHeight - (buttonHeight * 3), buttonWidth, buttonHeight), "Save"))
		{
			if (waveIndex + 1 > amountOfWaves)
			{
				editorState = EditorStates.Finished;
				CreateDataObjects();
			}
			else
			{
				waveIndex++;
				CreateDataObjects();
				numberOfEnemiesToSpawn = 0;
				timeBetweenSpawns = 0;
				window.Repaint();
				
			}
		}
	}

	private void FinishedState()
	{
		GUILayout.Label("Press Exit", EditorStyles.boldLabel);
		AssetDatabase.SaveAssets();
	}

	private void CreateFolder()
	{
		folderName = levelName + " Level Data";
		newPath = levelFilePath + "/" + folderName + "/";
		AssetDatabase.CreateFolder(levelFilePath, folderName);

		EditorUtility.FocusProjectWindow();
		AssetDatabase.SaveAssets();

		Debug.Log("Level Folder Created");
	}

	private void CreateDataObjects()
	{
		waveData = null;
		waveData = ScriptableObject.CreateInstance<Wave>();
		AssetDatabase.CreateAsset(waveData, newPath + waveIndex + " Data.Asset");
		SetDataFields();
		AssetDatabase.SaveAssets();
	}

	private void SetDataFields()
	{
		waveData.enemyTypes = enemyTypes;
		waveData.timeBetweenSpawns = timeBetweenSpawns;
		waveData.numberOfEnemiesToSpawn = numberOfEnemiesToSpawn;
	}
}
