using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(TouchingDiractions), typeof(Damageble))]
public class PlayerController : MonoBehaviour
{
	Rigidbody2D rb;

	Animator animator;
	public float runSpeed = 8f;
	public float walkSpeed = 5f;
	public float jumpImpulse = 10f;
	public float airWalkSpeed = 5f;
	Vector2 moveInput;
	TouchingDiractions touchingDiractions;
	Damageble damageble;
	public bool CanMove => animator.GetBool(AnimationStrings.canMove);
	public float CurrentMoveSpeed
	{
		get
		{
			if (CanMove)
			{				
				if (IsMoving && !touchingDiractions.IsOnWall)
				{
					if (touchingDiractions.IsGrounded)
					{					
						if (IsRunning)
						{
							return runSpeed;
						}
						else
						{
							return walkSpeed;
						}
					}else
					{
						// Air move
						return airWalkSpeed;
					}
				}
				else
				{
					// Idle speed
					return 0;
				}
			}else
			{
				// movement blocked
				return 0;
			}
		}
	}

	public bool IsAlive => animator.GetBool(AnimationStrings.isAlive);
		
	

	[SerializeField]
	private bool _isMoving = false;
	public bool IsMoving
	{
		get
		{
			return _isMoving;
		}
		private set
		{
			_isMoving = value;
			animator.SetBool(AnimationStrings.isMoving, value);
		}
	}

	[SerializeField]
	private bool _isRunning = false;
	public bool IsRunning
	{
		get
		{
			return _isRunning;
		}
		set
		{
			_isRunning = value;
			animator.SetBool(AnimationStrings.isRunning, value);			
		}
	}

	public bool _isFacingRight = true;
	public bool IsFacingRight
	{
		get
		{
			return _isFacingRight;
		}
		private set
		{
			if (_isFacingRight != value)
			{
				transform.localScale *= new Vector2(-1, 1);
			}
				_isFacingRight = value;
		}
	}



	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		touchingDiractions = GetComponent<TouchingDiractions>();
		damageble = GetComponent<Damageble>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void FixedUpdate()
	{
		if (!damageble.LockVelocity)
		{			
			rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
		}
		animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
		
		if (IsAlive)
		{			

			IsMoving = moveInput != Vector2.zero;

			SetFacingDirection(moveInput);
		}else
		{
			IsMoving = false;
		}
	}

	private void SetFacingDirection(Vector2 moveInput)
	{
		if (moveInput.x > 0 && !IsFacingRight)
		{
			// face the right
			IsFacingRight = true;

		}
		else if (moveInput.x < 0 && IsFacingRight)
		{
			// face the left
			IsFacingRight = false;
		}
	}

	public void OnRun(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			IsRunning = true;
		}
		else if (context.canceled)
		{
			IsRunning = false;
		}
	}
	
	public void OnJump(InputAction.CallbackContext context)
	{
		// TODO check if alive
		if (context.started && touchingDiractions.IsGrounded && CanMove)
		{
			animator.SetTrigger(AnimationStrings.jumpTrigger);
			rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
		}
	}
	
	public void OnAttack(InputAction.CallbackContext context)
	{
		if(context.started)
		{
			animator.SetTrigger(AnimationStrings.attackTrigger);
		}
	}
	
	public void OnHit(int damage, Vector2 knockback)
	{
		rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
	}

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }
}
