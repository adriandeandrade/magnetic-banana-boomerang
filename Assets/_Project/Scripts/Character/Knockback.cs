using System.Collections;
using System.Collections.Generic;
using PolyNav;
using UnityEngine;

public class Knockback : MonoBehaviour
{
	[Header("Knocback Setup")]
	[Tooltip("The amount of force used to knockback.")]
	[SerializeField] private float knockbackAmount;
	[Tooltip("How long the character will be knockbacked for.")]
	[SerializeField] private float knockbackTime;

	// Private Variables
	private Vector2 knockbackDirection;
	private float knockbackCounter;
	private bool knockbackTimerStart = false;
	private bool isKnockback;
	private bool doKnockback;

	// Componenents
	private Rigidbody2D rBody;
	private SpriteRenderer spriteRenderer;
	private Color originalColor;

	public float KnockbackCounter => knockbackCounter;
	public float KnockbackAmount { get { return knockbackAmount; } set { knockbackAmount = value; } }
	public float KnockbackTime { get { return knockbackTime; } set { knockbackTime = value; } }
	public bool IsKnockback { get => isKnockback; }

	private void Awake()
	{
		rBody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		originalColor = spriteRenderer.color;
	}


	private void Update()
	{
		if (knockbackCounter > 0 && knockbackTimerStart)
		{
			knockbackCounter -= Time.deltaTime;
			isKnockback = true;
		}
		else if (knockbackCounter <= 0 && knockbackTimerStart)
		{
			knockbackTimerStart = false;
			isKnockback = false;
			spriteRenderer.color = originalColor;
		}
	}


	private void FixedUpdate()
	{
		if (doKnockback)
		{
			rBody.AddForce(knockbackDirection.normalized * knockbackAmount, ForceMode2D.Impulse);
			doKnockback = false;
		}
	}

	public void ApplyKnockback(Vector2 direction, Color damageColor)
	{
		rBody.velocity = Vector2.zero;
		doKnockback = true;
		knockbackCounter = knockbackTime;
		knockbackTimerStart = true;
		knockbackDirection = direction;
		spriteRenderer.color = damageColor;
	}
}
