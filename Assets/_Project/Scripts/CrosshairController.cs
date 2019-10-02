using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagneticBananaBoomerang.Characters;

public class CrosshairController : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] private Image crosshair;
	[SerializeField] private Sprite enemyHoverCrosshair;
	[SerializeField] private Sprite regularCrosshair;
	[SerializeField] private Sprite trapHoverCrosshair;
    [SerializeField] private LayerMask enemyLayer;

	private void Update()
	{
		// TODO: Change this so it the crosshair sprite is saved on the interactable data object instead of here. When we hover over an object, we check what object it is
		// and we change the crosshair sprite based on that.

		transform.position = Input.mousePosition;

		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, enemyLayer);

		if (hit)
		{
			SetCrosshair(enemyHoverCrosshair);
		}
		else
		{
			Disable();
		}
	}

	private void SetCrosshair(Sprite spr)
	{
        crosshair.enabled = true;
		crosshair.sprite = spr;
	}

    private void Disable()
    {
        crosshair.enabled = false;
    }

}
