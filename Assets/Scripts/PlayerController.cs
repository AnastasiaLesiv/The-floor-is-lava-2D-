using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]
public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpImpulse = 10f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private TouchingDirection touchingDirection;
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
            animator.SetBool("isMoving", value);
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
            if (IsMoving && !touchingDirection.IsOnWall)
            {
                if (touchingDirection.IsGrounded)
                    return runSpeed;
                else
                    return jumpImpulse;
            }
            else
                return 0;
        }
    }
   
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
    } 
    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentSpeed * 0.5f, rb.velocity.y);
        animator.SetFloat("yVelocity", rb.velocity.y);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.IsGrounded)
        {
            animator.SetTrigger("jump");
            rb.velocity = new Vector2(rb.velocity.x, CurrentSpeed);
        }
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
            animator.SetBool("isFinish", true);
            RestartGameWithDelay(2);
        }
        if (collision.gameObject.tag == "Lava")
        {
            RestartGame();
        }
    }
    
    public void RestartGameWithDelay(float delay)
    {
        // Викликаємо метод RestartGame через вказаний час delay
        Invoke("RestartGame", delay);
    }

    private void RestartGame()
    {
        // Отримуємо індекс поточної сцени
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Завантажуємо поточну сцену знову, щоб перезапустити гру
        SceneManager.LoadScene(currentSceneIndex);
    }
}

