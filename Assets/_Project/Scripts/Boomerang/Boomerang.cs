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
	[SerializeField] private Stat boomerangStat;

	// Private Variables
	private GameObject detectedObjectInstance; // Gets set when a new boomerang is spawned and initialized with data from BoomerangManager.

	// Components
	private BoomerangManager boomerangManager;
	private Player player;
	private Collider2D col;
	private Rigidbody2D rBody;

	// Events
	public delegate void OnReachEndOfPathAction(); // Gets raised when we reach the end of the path.
	public static event OnReachEndOfPathAction OnReachEndOfPath;

	public delegate void OnPickedUpAction();
	public static event OnPickedUpAction OnPickedUp;

	public delegate void OnHitEnemyAction();
	public static event OnHitEnemyAction OnHitEnemy;

	private void Awake()
	{
		col = GetComponent<Collider2D>();
		rBody = GetComponent<Rigidbody2D>();
		player = Toolbox.instance.GetGameManager().PlayerRef;
	}

	public void InitializeBoomerang(BoomerangManager _boomerangManager, Vector2 targetDestinationPoint, GameObject _detectedObjectInstance = null)
	{
		boomerangManager = _boomerangManager;
		detectedObjectInstance = _detectedObjectInstance;

		if (detectedObjectInstance != null)
		{
			StartCoroutine(MoveToPointOverSpeed(detectedObjectInstance.transform.position, speed));
		}
		else
		{
			StartCoroutine(MoveToPointOverSpeed(targetDestinationPoint, speed));
		}

		Invoke("EnableCollider", 0.2f);
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

			BaseEnemy _enemy = detectedObjectInstance.GetComponent<BaseEnemy>();

			if (_enemy != null)
			{
				//_enemy.GetComponent<PolyNavAgent>().enabled = false;

				Vector2 direction = transform.position - detectedObjectInstance.transform.position;
				detectedObjectInstance.GetComponent<BaseEnemy>().TakeDamage(player.PlayerStats.GetStatValue(boomerangStat), direction, player);
				detectedObjectInstance = null;

				if(OnHitEnemy != null)
				{
					OnHitEnemy.Invoke();
				}

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
		if (OnPickedUp != null)
		{
			OnPickedUp();
		}

		Destroy(gameObject);
	}
}
