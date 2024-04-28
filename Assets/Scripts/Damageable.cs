using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
   [SerializeField] private int maxHealth = 100;
   [SerializeField] private int health = 100;
   [SerializeField] private bool isAlive = true;
   [SerializeField] private bool isInvincible = false;
   [SerializeField] private float invincibilityTime = 0.25f;
   public UnityEvent<int, Vector2> damageableHit;
   public UnityEvent<int, int> healthChanged;
   private float timeSinceHit = 0;
   private Animator animator;
   
   public int MaxHealth
   {
      get => maxHealth;
      set
      {
         maxHealth = value;
      }
   }
   public int Health
   {
      get => health;
      set
      {
         health = value;
         healthChanged?.Invoke(health, maxHealth);
         if (health <= 0)
            IsAlive = false;
      }
   }
   public bool IsAlive
   {
      get => isAlive;
      set
      {
         isAlive = value;
         animator.SetBool(AnimationStrings.isAlive, value);
        Debug.Log("IsAlive set " + value);
      }
      
   }

   public bool LockVelocity
   {
      get => animator.GetBool(AnimationStrings.lockVelocity);
      set => animator.SetBool(AnimationStrings.lockVelocity, value);
   }


   private void Awake()
   {
      animator = GetComponent<Animator>();
   }

   private void Update()
   {
      if (isInvincible)
      {
         if (timeSinceHit > invincibilityTime)
         {
            isInvincible = false;
            timeSinceHit = 0;
         }
         timeSinceHit += Time.deltaTime;
      }
   }

   public bool Hit(int damage, Vector2 knockback)
   {
      if (IsAlive && !isInvincible)
      {
         Health -= damage;
         isInvincible = true;
         animator.SetTrigger(AnimationStrings.hitTrigger);
         LockVelocity = true;
         damageableHit?.Invoke(damage, knockback);  
         CharacterEvents.characterDamaged.Invoke(gameObject, damage);
         return true;
      }
      return false;
   }

   public bool Heal(int healthRestore)
   {
      if (IsAlive && Health < MaxHealth)
      {
         int maxHeal = Mathf.Max(MaxHealth - Health, 0);
         int actualHeal = Mathf.Min(maxHeal, healthRestore);
         Health += actualHeal;
         CharacterEvents.characterHealed(gameObject, actualHeal);
         return true;
      }
      return false;
   }
}
