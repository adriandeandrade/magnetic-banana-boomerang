using UnityEngine;
using PathCreation;
using MagneticBananaBoomerang.Characters;

public class Boomerang : MonoBehaviour
{
	// Inspector Fields
	[Header("Boomerang Configuration")]
	[Tooltip("The speed at which the boomerang travels at at the middle point of the path.")]
	[SerializeField] private float apexSpeed = 5;
	[Tooltip("The speed at which the boomerang travels at on the path.")]
	[SerializeField] private float speed = 10f;
	[SerializeField] private float knockbackDetectionRadius = 2f;

	// Private Variables
	private bool startMove = false;
	private float distanceTravelled;
	private float velocity;

	// Components
	private BoomerangManager boomerangManager;
	private PathCreator pathCreator;
	private EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Stop;
	private Collider2D col;
	private Rigidbody2D rBody;

	// Events
	public delegate void OnReachEndOfPathAction(); // Gets raised when we reach the end of the path.
	public static event OnReachEndOfPathAction OnReachEndOfPath;

	public delegate void OnPickedUpAction();
	public static event OnPickedUpAction OnPickedUp;

	private void Awake()
	{
		col = GetComponent<Collider2D>();
		rBody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		KnockbackEnemiesInPath();
	}

	private void FixedUpdate()
	{
		if (pathCreator != null)
		{
			if (startMove)
			{
				BoomerangMovement();
			}
		}
	}

	private void BoomerangMovement()
	{
		if (distanceTravelled < pathCreator.path.length) // If we arent at the end of the path yet.
		{
			if (distanceTravelled > pathCreator.path.length * 0.4f && distanceTravelled < pathCreator.path.length * 0.6f)
			{
				velocity = apexSpeed;
			}
			else
			{
				velocity = speed;
			}


			distanceTravelled += velocity * Time.deltaTime;
			rBody.MovePosition(pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction));
		}

		else // If we reached the end of the path.
		{
			if (OnReachEndOfPath != null)
			{
				OnReachEndOfPath();
			}

			startMove = false;
			print("Path complete");
		}
	}

	private void KnockbackEnemiesInPath()
	{
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, knockbackDetectionRadius);

		if (cols.Length > 0)
		{
			foreach (Collider2D col in cols)
			{
				BaseEnemy enemy = col.GetComponent<BaseEnemy>();

				if (enemy != null)
				{
					Vector2 dir = col.transform.position - transform.position;
					enemy.knockback.ApplyKnockback(dir, Color.red);
				}
			}
		}
	}

	public void InitializeBoomerang(BoomerangManager _boomerangManager, PathCreator creator)
	{
		pathCreator = creator;
		boomerangManager = _boomerangManager;
		startMove = true;
		Invoke("EnableCollider", 0.2f);
	}

	private void EnableCollider()
	{
		col.enabled = true;
	}

	public void Pickup()
	{
		if (OnPickedUp != null)
		{
			OnPickedUp();
		}

		//boomerangController.ResetController();
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		IDamageable damageable = other.GetComponent<IDamageable>();

		if (damageable != null)
		{
			Vector2 dir = other.transform.position - transform.position;
			damageable.TakeDamage(2f, dir);
		}
	}
}
