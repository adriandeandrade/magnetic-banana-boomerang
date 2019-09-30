using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class DirectionalArrowToTarget : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] private GameObject arrowUI;
	[SerializeField] private Transform target;
	[SerializeField] private Transform startingPoint;

	// Private Variables
	private bool showArrow = false;
	private float aimAngle;

	// Components
	private Camera cam;

	private void Awake()
	{
		cam = Camera.main;
	}

	private void Update()
	{
		if (!CheckIfWithinBounds())
		{
			DrawArrowPointingAtTarget(transform.position);
		}
        else
        {
            arrowUI.SetActive(false);
        }

	}

	private bool CheckIfWithinBounds()
	{
		Vector3 screenPoint = cam.WorldToViewportPoint(target.position);

		bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
		return onScreen;
	}

	private void DrawArrowPointingAtTarget(Vector2 startPoint)
	{
		arrowUI.SetActive(true);

		Vector2 arrowPos = target.position - transform.position;

		aimAngle = Mathf.Atan2(arrowPos.y, arrowPos.x) * Mathf.Rad2Deg;

		arrowPos = Quaternion.AngleAxis(aimAngle, Vector3.forward) * (Vector2.right * 1.2f);
		arrowUI.transform.position = startPoint + arrowPos;
		arrowUI.transform.rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
	}
}
