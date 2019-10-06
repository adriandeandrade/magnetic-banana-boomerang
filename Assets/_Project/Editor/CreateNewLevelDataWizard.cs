using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class CreateNewLevelDataWizard : EditorWindow
{
	private static string levelFilePath = "Assets/_Project/Game Data/Levels";

	// Section Specific Settings
	private int amountOfWaves;

	// Wave Specific Settings
	private int numberOfEnemiesToSpawn;
	private float timeBetweenSpawns;
	[SerializeField] private List<GameObject> enemyTypes = new List<GameObject>();

	private bool folderCreated;
	private List<bool> showWaveSettings = new List<bool>();
	private string levelName;
	private string folderName;
	private string newPath; // Path that gets created when we create a new level.
	private float buttonWidth = 100;
	private float buttonHeight = 25;

	[MenuItem("Adrian's Tools/Create New Level")]
	private static void Init()
	{
		CreateNewLevelDataWizard window = (CreateNewLevelDataWizard)EditorWindow.GetWindow(typeof(CreateNewLevelDataWizard));
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

	private void CreateLevel()
	{
		Debug.Log("Level Created");
	}

	private void OnGUI()
	{
		var window = (CreateNewLevelDataWizard)EditorWindow.GetWindow(typeof(CreateNewLevelDataWizard));
		GUI.backgroundColor = Color.grey;

		Rect windowSize = window.position;
		float windowHeight = windowSize.size.y;
		float windowWidth = windowSize.size.x;

		GUILayout.Label("Wave Settings", EditorStyles.boldLabel);
		amountOfWaves = EditorGUILayout.IntField("Amount of Waves", amountOfWaves);

		while (showWaveSettings.Count < amountOfWaves)
		{
			showWaveSettings.Add(true);
		}

		for (int i = 0; i < amountOfWaves; i++)
		{
			showWaveSettings[i] = EditorGUILayout.Foldout(showWaveSettings[i], "Wave Settings: " + i, true);

			if (showWaveSettings[i])
			{
				using (new FixedWidthLabel("Enemy Count"))
				{
					numberOfEnemiesToSpawn = EditorGUILayout.IntField(numberOfEnemiesToSpawn);
				}

				using (new FixedWidthLabel("Spawn Delay"))
				{
					timeBetweenSpawns = EditorGUILayout.FloatField(timeBetweenSpawns);
				}

				// Enemy Types List
				int newEnemyTypeCount = Mathf.Max(0, EditorGUILayout.IntField("Amount of Enemy Types", enemyTypes.Count));
				while (newEnemyTypeCount < enemyTypes.Count)
				{
					enemyTypes.RemoveAt(enemyTypes.Count - 1);
				}

				while (newEnemyTypeCount > enemyTypes.Count)
				{
					enemyTypes.Add(null);
				}

				for (int j = 0; j < enemyTypes.Count; j++)
				{
					enemyTypes[j] = (GameObject)EditorGUILayout.ObjectField(enemyTypes[j], typeof(GameObject), false);
				}

				GUILayout.Space(10);
			}
		}

		// Button for creating new wave data object


		// Exit Button
		if (GUI.Button(new Rect((windowWidth / 2) - buttonWidth / 2, 500, buttonWidth, buttonHeight), "Exit"))
		{
			window.Close();
		}

		// Create Level Button
		if (GUI.Button(new Rect((windowWidth / 2) - buttonWidth / 2, (500 - buttonHeight) - 10, buttonWidth, buttonHeight), "Create Level"))
		{
			if (levelName == null)
			{
				EditorUtility.DisplayDialog("Error!", "No level name given. Please input a level name.", "Ok");
				GUI.enabled = false;
			}
			else
			{
				CreateFolder();
				CreateLevel();
				GUI.enabled = true;
				window.Close();
			}
		}
	}
}

public class FixedWidthLabel : IDisposable
{
	private readonly ZeroIndent indentReset;

	public FixedWidthLabel(GUIContent label)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(label, GUILayout.Width(GUI.skin.label.CalcSize(label).x + 9 * EditorGUI.indentLevel));

		indentReset = new ZeroIndent();
	}

	public FixedWidthLabel(string label) : this(new GUIContent(label))
	{

	}

	public void Dispose()
	{
		indentReset.Dispose();
		EditorGUILayout.EndHorizontal();
	}
}

class ZeroIndent : IDisposable
{
	private readonly int originalIndent;

	public ZeroIndent()
	{
		originalIndent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
	}

	public void Dispose()
	{
		EditorGUI.indentLevel = originalIndent;
	}
}