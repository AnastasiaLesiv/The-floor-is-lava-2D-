using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class TouchingDirection : MonoBehaviour
{
    private CapsuleCollider2D touchingCol;
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float ceilingDistance = 0.05f;
    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    private Animator animator;
    [SerializeField]
    private bool isGrounded = true;

    private bool isOnCeiling = false;
    Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left ;

    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
        set
        {
            isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    } 
    
   
    public bool IsOnCeiling
    {
        get
        {
            return isOnCeiling;
        }
        set
        {
            isOnCeiling = value;
            animator.SetBool("isOnCeiling", value);
        }
    }

    void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        
    }
    
    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
}
