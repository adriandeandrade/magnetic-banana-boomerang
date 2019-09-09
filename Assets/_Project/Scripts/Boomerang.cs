using UnityEngine;
using PathCreation;

public class Boomerang : MonoBehaviour
{
	// Inspector Fields
	[Header("Boomerang Configuration")]
	[Tooltip("The speed at which the boomerang travels at at the middle point of the path.")]
	[SerializeField] private float apexSpeed = 5;
	[Tooltip("The speed at which the boomerang travels at on the path.")]
	[SerializeField] private float speed = 10f;

	// Private Variables
	private bool startMove = false;
	private float distanceTravelled;
	private float velocity;

	// Components
	private BoomerangController boomerangController;
	private PathCreator pathCreator;
	private EndOfPathInstruction endOfPathInstruction;
	private Collider2D col;

	// Events
	public delegate void OnReachEndOfPathAction(); // Gets raised when we reach the end of the path.
	public static event OnReachEndOfPathAction OnReachEndOfPath;

	public delegate void OnPickedUpAction();
	public static event OnPickedUpAction OnPickedUp;

	private void Awake()
	{
		col = GetComponent<Collider2D>();
	}

	private void Start()
	{
		endOfPathInstruction = EndOfPathInstruction.Stop;
	}

	private void Update()
	{
		if (pathCreator != null)
		{
			if (startMove)
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
					transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
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
		}
	}

	public void StartPath(BoomerangController _boomerangController, PathCreator creator)
	{
		pathCreator = creator;
		boomerangController = _boomerangController;
		startMove = true;
		Invoke("EnableCollider", 0.5f);
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
}
