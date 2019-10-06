using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChestLootEditor : EditorWindow
{
	Item itemToAddToChest = null;
	Item itemToRemoveFromChest = null;

	[MenuItem("Adrian's Tools/Edit Chest Spawnables")]
	private static void Init()
	{
		ChestLootEditor window = (ChestLootEditor)EditorWindow.GetWindow(typeof(ChestLootEditor));
		window.Show();
	}

	private void OnGUI()
	{
		var window = (ChestLootEditor)EditorWindow.GetWindow(typeof(ChestLootEditor));

		GameObject chestPrefab = AssetDatabase.LoadAssetAtPath("Assets/_Project/Objects/Chest (Crate)/Resources/Chest.prefab", typeof(GameObject)) as GameObject;
		Chest chestComponent = chestPrefab.GetComponent<Chest>();

		EditorGUILayout.LabelField("Item To Add");

		itemToAddToChest = EditorGUILayout.ObjectField(itemToAddToChest, typeof(Item), false) as Item;

		EditorGUILayout.LabelField("Item To Remove");
		itemToRemoveFromChest = EditorGUILayout.ObjectField(itemToRemoveFromChest, typeof(Item), false) as Item;

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		// Show current items in chest
		EditorGUILayout.LabelField("Current Items");

		foreach (Item item in chestComponent.possibleItemSpawns)
		{
			GUILayout.BeginHorizontal("BOX");
			Item _item = EditorGUILayout.ObjectField(item, typeof(Item), false) as Item;
			EditorGUILayout.Space();

			GUILayout.EndHorizontal();
		}

        EditorGUILayout.Space();
		EditorGUILayout.Space();

		if (GUILayout.Button(("Add Item")))
		{
			if (chestComponent.possibleItemSpawns.Contains(itemToAddToChest))
			{
				Debug.Log("Item already can spawn from this chest type.");
			}
			else
			{
				chestComponent.possibleItemSpawns.Add(itemToAddToChest);
			}
		}

		if (GUILayout.Button("Remove Item"))
		{
			if (!chestComponent.possibleItemSpawns.Contains(itemToRemoveFromChest))
			{
				Debug.Log("Item has already been removed or doesnt exist in this chest type.");
			}
			else
			{
				chestComponent.possibleItemSpawns.Remove(itemToRemoveFromChest);
			}
		}

		if (GUILayout.Button("Finish"))
		{
			AssetDatabase.SaveAssets();
			window.Close();
		}
	}
}
