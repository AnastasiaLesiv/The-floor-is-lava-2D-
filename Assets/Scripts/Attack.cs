using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private Vector2 knockback = Vector2.zero;
    private Collider2D attackCollider;

    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            Vector2 deliveredKnockback =
                transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
            if(gotHit)
                Debug.Log(collision.name + " hit for " + attackDamage);
        }
        
    }

}