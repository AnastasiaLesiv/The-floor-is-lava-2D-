using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DefaultNamespace;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    //private Transform currentPoint;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    public float speed = 0.5f;
    [SerializeField] private bool hasTarget = false;
    private WalkableDirection walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
    private Damageable damageable;
    private TouchingDirection touchingDirection;
    
    public enum WalkableDirection {Right, Left};
    public WalkableDirection WalkDirection
    {
        get => walkDirection;
        set
        {
            if (walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1,
                    gameObject.transform.localScale.y);
                if (value == WalkableDirection.Right)
                    walkDirectionVector = Vector2.right;
                else if (value == WalkableDirection.Left)
                    walkDirectionVector = Vector2.left;
            }
            walkDirection = value;
        }
    }
    
    public bool HasTarget
    {
        get => hasTarget;
        set
        {
            hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get => animator.GetBool(AnimationStrings.canMove);
    }

    
    public float AttackCooldown
    {
        get => animator.GetFloat(AnimationStrings.attackCooldown);
        set => animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
    }
    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        touchingDirection = GetComponent<TouchingDirection>();
    }

    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCooldown > 0)
            AttackCooldown -= Time.deltaTime;
       
        if (!damageable.LockVelocity)
        {
            if (CanMove)
            {
                rb.velocity = new Vector2(speed * walkDirectionVector.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        
    }
    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
            WalkDirection = WalkableDirection.Left;
        else if(WalkDirection == WalkableDirection.Left)
            WalkDirection = WalkableDirection.Right;
        else
            Debug.LogError("Current walkable direction is not set to legal values of right or left");
    }
    public void OnHit(int damage, Vector2 knockback)
    {
         rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirection.IsGrounded)
        {
            FlipDirection();
        }
    }
}
