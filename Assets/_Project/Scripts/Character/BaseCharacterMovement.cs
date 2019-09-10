using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    FACING DIRECTIONS:
    DOWN =      0
    UPRIGHT =   1
    DOWNRIGHT = 2
    LEFT =      3
    RIGHT =     4
    UPLEFT =    5
    DOWNLEFT =  6
    UP =        7
 */

namespace MagneticBananaBoomerang.Characters
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(Animator))]
	public abstract class BaseCharacterMovement : MonoBehaviour
	{
		// Inspector Fields
		[Header("Movement Setup")]
		[SerializeField] protected float moveSpeed = 5f; // Character base movement speed.

		protected float facingDirection; // The current direction the character is facing. This is used to pass the information on to the animator.
		protected Vector2 velocity; // The current velocity of our character.

		#region Facing Direction Constants
		/* This is the facing direction context. They are just constants that define each direction in 8 directional movement. */
		private Vector2 down = new Vector2(0, -1);
		private Vector2 upright = new Vector2(0, 1);
		private Vector2 downright = new Vector2(-1, -1);
		private Vector2 left = new Vector2(1, 0);
		private Vector2 right = new Vector2(-1, 0);
		private Vector2 upleft = new Vector2(1, 1);
		private Vector2 downleft = new Vector2(1, -1);
		private Vector2 up = new Vector2(0, 1);
		#endregion

		// Properties
		public float FacingDirection { get => facingDirection; set => facingDirection = value; }
		public float MoveSpeed { set => moveSpeed = value; }
		public Vector2 Velocity { get => velocity; }
		public Rigidbody2D RBody2D { get => RBody2D; }

		// Components
		protected Animator animator;
		protected Rigidbody2D rBody;

		public virtual void Awake()
		{
			// Get references to our components if references are null.
			if (rBody == null) rBody = GetComponent<Rigidbody2D>();
			if (animator == null) animator = GetComponent<Animator>();
		}

		public virtual void Update()
		{
			GetFacingDirection();
		}

        public virtual void FixedUpdate()
        {

        }

        ///<summary>Moves the rigidbody given a direction;</summary>
        public virtual void Move(Vector2 direction)
        {
            velocity = direction;
            rBody.velocity = velocity * moveSpeed;
			UpdateAnimator();
        }

        public virtual void UpdateAnimator()
        {
            animator.SetFloat("Horizontal", velocity.x);
            animator.SetFloat("Vertical", velocity.y);
			animator.SetFloat("Speed", velocity.sqrMagnitude);
			animator.SetFloat("FacingDirection", facingDirection);
        }

        public virtual void GetFacingDirection()
        {
            if (velocity == Vector2.zero) return; // If we are standing still return so we idle in the last direction moved.

			if (velocity == new Vector2(0, -1)) facingDirection = 0f;
			if (velocity == new Vector2(-1, 1)) facingDirection = 1f;
			if (velocity == new Vector2(-1, -1)) facingDirection = 2f;
			if (velocity == new Vector2(1, 0)) facingDirection = 3f;
			if (velocity == new Vector2(-1, 0)) facingDirection = 4f;
			if (velocity == Vector2.one) facingDirection = 5f;
			if (velocity == new Vector2(1, -1)) facingDirection = 6f;
			if (velocity == new Vector2(0, 1)) facingDirection = 7f;
        }
	}
}


