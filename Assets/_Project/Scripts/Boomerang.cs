using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enjoii.Curves;

public class Boomerang : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] private float flightDuration;

	private float flightProgress;
	private bool flightInProgress = false;

	private BezierCurve bezier;
	private PlayerBoomerang player;

	// Components


	private void Awake()
	{
        player = FindObjectOfType<PlayerBoomerang>();
	}

	public void StartPath(BezierCurve _bezierCurve)
	{
		bezier = _bezierCurve;
	}

	private void Update()
	{
		flightProgress += Time.deltaTime / flightDuration;

		if (flightProgress > 1f)
		{
			flightProgress = 1f;
            player.DoUpdatePoints = true;
			Destroy(gameObject);
			return;
		}

		transform.localPosition = bezier.GetPoint(flightProgress);
	}
}
