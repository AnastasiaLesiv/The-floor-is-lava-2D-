using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float runSpeed    = 5f;
    public float jumpImpulse = 10f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private TouchingDirection touchingDirection;
    private Damageable damageable;
    private bool isMoving = false;
    private bool isFacingRight = true;
    
    
    public bool IsMoving {
        get
        {
            return isMoving;
        }
        private set
        {
            isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    public bool IsFacingRight
    {
        get
        {
            return isFacingRight;
        }
        set
        {
            if (isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            isFacingRight = value;
        }
    }
    public float CurrentSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving)
                {
                    if (touchingDirection.IsGrounded)
                        return runSpeed;
                    else
                        return jumpImpulse;
                }
                else
                    return 0;
            }
            else
            {
                return 0;
            }
            
        }
    }
    public bool CanMove
    {
        get => animator.GetBool(AnimationStrings.canMove);
    }
    public bool IsAlive
    {
        get => animator.GetBool(AnimationStrings.isAlive);
    }

    
   
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
        damageable = GetComponent<Damageable>();
    } 
    void FixedUpdate()
    {
        if (!damageable.LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentSpeed * 0.5f, rb.velocity.y);
            animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        }
        
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, CurrentSpeed);
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
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
        if(context.started)
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
    }
    void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if(moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Dress")
        {
            animator.SetBool(AnimationStrings.isFinish, true);
            RestartGameWithDelay(2);
        }
        if (collision.gameObject.tag == "Lava")
        {
            Debug.Log("Restart");
            RestartScene();
        }
    }
    
    public void RestartGameWithDelay(float delay)
    {
        // Викликаємо метод RestartGame через вказаний час delay
        Invoke("RestartGame", delay);
    }

    private void RestartScene()
    {
        
        // Отримуємо індекс поточної сцени
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        Debug.Log("Reload scene: " + currentSceneIndex);
        // Завантажуємо поточну сцену знову, щоб перезапустити гру
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}

