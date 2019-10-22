using UnityEngine;
using PathCreation;
using MagneticBananaBoomerang.Characters;
using System.Collections;
using System.Collections.Generic;
using PolyNav;

public class Boomerang : MonoBehaviour
{
	// Inspector Fields
	[Header("Boomerang Configuration")]
	[Tooltip("The speed at which the boomerang travels.")]
	[SerializeField] private float speed = 10f;
	[SerializeField] private float damageFallOffAmount = 0.20f;
	[SerializeField] private Stat damageStat;

	// Private Variables
	private GameObject detectedObjectInstance; // Gets set when a new boomerang is spawned and initialized with data from BoomerangManager.
	private bool returning = false;
	private float nextDamage;

	// Components
	private BoomerangManager boomerangManager;
	private Player player;
	private CircleCollider2D col;
	private Rigidbody2D rBody;
    private StatManager statManager;

	// Events
	public System.Action OnBoomerangPickedUpAction;

	private void Awake()
	{
		col = GetComponent<CircleCollider2D>();
		rBody = GetComponent<Rigidbody2D>();
		player = Toolbox.instance.GetGameManager().PlayerRef;
        statManager = FindObjectOfType<StatManager>();
	}

    public void InitializeBoomerang(BoomerangManager _boomerangManager, Vector2 targetDestinationPoint, GameObject _detectedObjectInstance = null)
	{
		boomerangManager = _boomerangManager;
		detectedObjectInstance = _detectedObjectInstance;
        damageStat = statManager.GetStatWithName("gorilla_base_damage");

        nextDamage = damageStat.currentValue;
        Debug.Log(nextDamage);

		if (detectedObjectInstance != null)
		{
			StartCoroutine(MoveToPointOverSpeed(detectedObjectInstance.transform.position, speed));
		}
		else
		{
			StartCoroutine(MoveToPointAndDetectCollisions(targetDestinationPoint, speed, false));
		}

		Invoke("EnableCollider", 0.2f);
	}

	private void StartReturnToPlayerWithCollisions()
	{
		StartCoroutine(MoveToPointAndDetectCollisions(player.transform.position, speed, true));
	}

	IEnumerator MoveToPointAndDetectCollisions(Vector3 endPoint, float _speed, bool returnToPlayer)
	{
		Vector2 dir = Vector2.zero;
		Vector3 targetEndpoint = Vector2.zero;
		HashSet<GameObject> objectsHit = new HashSet<GameObject>(); // Keeps track of objects we hit so we dont deal damage multiple times.

		if (returnToPlayer)
		{
			dir = (transform.position - player.transform.position).normalized;
			targetEndpoint = player.transform.position;
		}
		else
		{
			dir = (transform.position - endPoint).normalized;
			targetEndpoint = endPoint;
		}

		while (rBody.position != (Vector2)targetEndpoint)
		{
			if (returnToPlayer) targetEndpoint = player.transform.position;

			rBody.position = Vector2.MoveTowards(rBody.position, targetEndpoint, _speed * Time.fixedDeltaTime);

			Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(rBody.position, col.radius); // Check for the objects hit.

			if (detectedObjects.Length > 0 && detectedObjects != null)
			{
				foreach (Collider2D detectedObject in detectedObjects)
				{
					if (detectedObject.gameObject == player.transform.gameObject) continue;

					BaseEnemy _enemy = detectedObject.GetComponent<BaseEnemy>();

					if (_enemy != null)
					{
						if (!objectsHit.Contains(_enemy.gameObject)) // If the hashset doesnt contain the object already add it to the hashset and deal damage to it.
						{
							Vector2 direction = transform.position - detectedObject.transform.position;
							_enemy.TakeDamage(nextDamage, Vector2.zero, player);

							// TODO: Have a look at this again.
							if(nextDamage - damageFallOffAmount * damageStat.currentValue > 0)
							{
								nextDamage -= damageFallOffAmount * damageStat.currentValue;
                            }

							objectsHit.Add(detectedObject.gameObject);
						}
					}
				}
			}

			yield return new WaitForFixedUpdate();
		}

		Invoke("StartReturnToPlayerWithCollisions", 0.01f);
		yield break;
	}

	IEnumerator MoveToPointOverSpeed(Vector3 endPoint, float _speed)
	{
		Vector2 dir = (transform.position - endPoint).normalized;

		while (rBody.position != (Vector2)endPoint)
		{
			rBody.position = Vector2.MoveTowards(rBody.position, endPoint, _speed * Time.fixedDeltaTime);
			yield return new WaitForFixedUpdate();
		}

		if (detectedObjectInstance != null)
		{
			Interactable _interactable = detectedObjectInstance.GetComponent<Interactable>(); // If we are interacting with a interactable object.
			if (_interactable != null)
			{
				_interactable.Interact();
			}

			LittleTurtle littleTurtle = detectedObjectInstance.GetComponent<LittleTurtle>();
			if (littleTurtle != null)
			{
				littleTurtle.ApplyKnockback();
				detectedObjectInstance = null;
			}
		}

		yield return StartCoroutine(MoveToPlayerOverSpeed()); // Return to player.
	}

	IEnumerator MoveToPlayerOverSpeed()
	{
		Vector2 dir = (transform.position - player.transform.position).normalized;

		while (rBody.position != (Vector2)player.transform.position)
		{
			rBody.position = Vector2.MoveTowards(rBody.position, player.transform.position, speed * Time.fixedDeltaTime);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	private void EnableCollider()
	{
		col.enabled = true;
	}

	public void Pickup()
	{
		if (OnBoomerangPickedUpAction != null)
		{
			OnBoomerangPickedUpAction.Invoke();
		}

		Destroy(gameObject);
	}
}
