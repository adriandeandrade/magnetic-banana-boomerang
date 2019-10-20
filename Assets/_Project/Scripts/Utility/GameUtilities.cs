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

	public static Vector2 GetRandomPointOnCircle(float radius)
	{
		float angle = Random.Range(0f, Mathf.PI * 2);
		float x = Mathf.Sin(angle) * radius;
		float y = Mathf.Cos(angle) * radius;

		return new Vector2(x, y);
	}

	public static Timer CreateNewTimer()
	{
		GameObject timerParent;

		if (!GameObject.Find("Timers"))
		{
			timerParent = new GameObject("Timers");
		}
		else
		{
			timerParent = GameObject.Find("Timers");
		}

		GameObject timerPrefab = Resources.Load<GameObject>("Prefabs/prefab_Timer");
		GameObject newTimer = MonoBehaviour.Instantiate(timerPrefab);
		newTimer.transform.SetParent(timerParent.transform);

		return newTimer.GetComponent<Timer>();
	}
}
