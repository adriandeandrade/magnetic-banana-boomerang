using System.Collections.Generic;
using UnityEngine;

internal class GameUtilities
{
	public static GameObject FindClosestGameObject(List<GameObject> gameObjects, Vector2 currentPosition)
	{
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector2 position = currentPosition;

		foreach (GameObject go in gameObjects)
		{
			Vector2 difference = (Vector2)go.transform.position - position;
			float currentDistance = difference.sqrMagnitude;

			if (currentDistance < distance)
			{
				closest = go;
				distance = currentDistance;
			}
		}
		return closest;
	}
}
