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
	private float damageAmount;

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

	public void ShootProjectile(Vector3 endPoint, float _speed, float _damageAmount, float accuracy)
	{
		StartCoroutine(MoveToPointOverSpeed(endPoint, _speed, accuracy));
		damageAmount = _damageAmount;
	}

	IEnumerator MoveToPointOverSpeed(Vector3 endPoint, float _speed, float accuracy)
	{
		Vector2 dir = (endPoint - transform.position).normalized;

		dir.x += Random.Range(-accuracy, accuracy);
		dir.y += Random.Range(-accuracy, accuracy);

		rBody.velocity = dir * _speed * Time.fixedDeltaTime;
		//rBody.position = Vector2.MoveTowards(rBody.position, endPoint, _speed * Time.fixedDeltaTime);
		yield return new WaitForFixedUpdate();

		yield break;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		BaseCharacter _characterHit = other.GetComponent<BaseCharacter>();

		if (_characterHit != null && !_characterHit.GetComponent<BaseEnemy>())
		{
			Vector2 direction = _characterHit.transform.position - transform.position;
			_characterHit.GetComponent<BaseCharacter>().TakeDamage(damageAmount, direction);
			Destroy(gameObject);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, projectileRadius);
	}
}
