using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum TrapType
{
	SPIKE, KNOCKBACK
}

public class CreateNewTrap : EditorWindow
{
	private static string assetFilePath = "Assets/_Project/Traps/Data";
	private TrapType trapType;

	private GameObject knockbackTrapPrefab;
	private GameObject spikeTrapPrefab;
	private GameObject trapActivatorPrefab;

	[MenuItem("Adrian's Tools/Create New Trap")]
	private static void Init()
	{
		CreateNewTrap window = (CreateNewTrap)EditorWindow.GetWindow(typeof(CreateNewTrap));
		window.minSize = new Vector2(300, 200);
		window.Show();
	}

	private void OnGUI()
	{
		var window = (CreateNewTrap)EditorWindow.GetWindow(typeof(CreateNewTrap));

		GUILayout.Space(5);
		GUILayout.Label("Create new trap wizard.", EditorStyles.boldLabel);
		GUILayout.Space(5);

		trapType = (TrapType)EditorGUILayout.EnumPopup("Trap Type:", trapType);

		if (GUILayout.Button("Create new trap"))
		{
			OnCreateButtonPressed(trapType);
		}

		if (GUILayout.Button("Cancel"))
		{
			window.Close();
		}
	}

	private void OnCreateButtonPressed(TrapType type)
	{
		switch (type)
		{
			case TrapType.KNOCKBACK:
				Object newKnockbackTrap = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Prefabs/Traps/KnockbackTrap"));
				newKnockbackTrap.name = "New Knockback Trap";
				break;
			case TrapType.SPIKE:
				Object newSpikeTrap = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Prefabs/Traps/SpikeTrap"));
				newSpikeTrap.name = "New Knockback Trap";
				break;
		}

		Object newActivator = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("Prefabs/Traps/Activator"));
		newActivator.name = "New Trap Activator";
	}
}
