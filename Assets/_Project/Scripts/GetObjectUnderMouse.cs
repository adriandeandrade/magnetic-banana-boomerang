using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class GetObjectUnderMouse : MonoBehaviour
{
	BoomerangManager boomerangManager;

	private void Start()
	{
		boomerangManager = Toolbox.instance.GetBoomerangManager();
	}

	private void Update()
	{
		GetObject();
	}

	public static GameObject GetObject()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

		if (hit)
		{
			if(hit.collider.GetComponent<BaseEnemy>()) return null;
			return hit.collider.gameObject;
		}

		return null;
	}
}
