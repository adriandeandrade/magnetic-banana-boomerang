using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagneticBananaBoomerang.Characters;

public class BaseProjectile : MonoBehaviour
{
	// Inspector Fields
	[SerializeField] private float projectileLifeTime;
	[SerializeField] private float projectileRadius;

	// Private Variables
	private float currentProjectileLifetime;

	// Components
	private Rigidbody2D rBody;

	private void Awake()
	{
		rBody = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		currentProjectileLifetime = projectileLifeTime;
	}

	private void Update()
	{
		if (currentProjectileLifetime <= 0)
		{
			Destroy(gameObject);
		}

		currentProjectileLifetime -= Time.deltaTime;
	}

	public void ShootProjectile(Vector3 endPoint, float _speed)
	{
		StartCoroutine(MoveToPointOverSpeed(endPoint, _speed));
	}

	IEnumerator MoveToPointOverSpeed(Vector3 endPoint, float _speed)
	{
		Vector2 dir = (transform.position - endPoint).normalized;

		while (rBody.position != (Vector2)endPoint)
		{
			rBody.position = Vector2.MoveTowards(rBody.position, endPoint, _speed * Time.fixedDeltaTime);
			yield return new WaitForFixedUpdate();
		}

		rBody.AddForce(-dir * 2f, ForceMode2D.Impulse);

		yield break;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		BaseCharacter _characterHit = other.GetComponent<BaseCharacter>();

		if (_characterHit != null && !_characterHit.GetComponent<BaseEnemy>())
		{
			Vector2 direction = _characterHit.transform.position - transform.position;
			_characterHit.GetComponent<BaseCharacter>().TakeDamage(0f, direction);
			Destroy(gameObject);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, projectileRadius);
	}
}
