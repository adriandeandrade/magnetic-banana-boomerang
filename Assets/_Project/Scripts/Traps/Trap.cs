using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public abstract class Trap : MonoBehaviour
{
	// Inspector Fields
	[Tooltip("This is for debugging purposes. The color for when the trap is active.")]
	[SerializeField] protected Color activatedColor;
	[Tooltip("How long the trap will be active for.")]
	[SerializeField] protected float activeTime;
	[Tooltip("How far the trap will detect enemies.")]
	[SerializeField] protected float range = 2f;
	[Tooltip("Select the layers that should be detected by the trap.")]
	[SerializeField] protected LayerMask detectionLayer;

	// Private Variables
	protected Color originalColor;
	protected float currentActiveTime;
	protected bool active = false;

	// Components
	protected SpriteRenderer spriteRenderer;

	protected void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected virtual void Start()
	{
		if (spriteRenderer != null)
		{
			originalColor = spriteRenderer.color;
		}
	}

	protected virtual void Update()
	{
		if (active)
		{
			if (currentActiveTime <= 0)
			{
				Deactivate();
				return;
			}

            UpdateCallback();

			currentActiveTime -= Time.deltaTime;
		}
	}

	public abstract void Activate();
	public abstract void Deactivate();

    protected virtual void UpdateCallback()
    {

    }


    protected void InitializeTimer()
	{
		currentActiveTime = activeTime;
		active = true;
	}

	/// <summary> 
	/// This method will return a list of enemies detected within a certain radius.
	/// </summary>
	protected List<GameObject> DetectObjectsWithinRadius(Transform centerPoint, float detectionRadius)
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll(centerPoint.position, detectionRadius, detectionLayer);
		List<GameObject> otherObjects = new List<GameObject>();

		if (cols.Length > 0)
		{
			foreach (Collider2D col in cols)
			{
				if (col.gameObject != null)
				{
					otherObjects.Add(col.gameObject);
				}
			}
		}

		return otherObjects; // Return empty list.
	}
}
