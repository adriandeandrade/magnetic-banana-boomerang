using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] protected InteractableData interactableData;

	private bool isHovering;
	private Color originalColor;

	// Components
	protected SpriteRenderer spriteRenderer;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		originalColor = spriteRenderer.color;
	}

	public void OnHover()
	{
		if (!isHovering)
		{
			isHovering = true;
			spriteRenderer.color = interactableData.hoverColor;
		}
	}

	private void OnMouseEnter()
	{
		OnHover();
	}

	private void OnMouseExit()
	{
		isHovering = false;
		spriteRenderer.color = originalColor;
	}

	public abstract void Interact();
}
