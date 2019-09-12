using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			return hit.collider.gameObject;

			/* Interactable interactable = hit.collider.GetComponent<Interactable>();
			if (interactable != null)
			{
				interactable.OnHover();

				if (Input.GetKeyDown(KeyCode.F))
				{
					boomerangManager.Interact(FindObjectOfType<Player>().transform.position, hit.collider.transform.position);
				}
			} */
		}

		return null;
	}
}
