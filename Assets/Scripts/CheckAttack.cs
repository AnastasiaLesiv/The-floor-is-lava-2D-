using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAttack : MonoBehaviour
{
    public Transform player;
    private float distanceToPlayer = 0.2f;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= distanceToPlayer)
        {
            transform.parent.GetComponent<Mushroom>().HasTarget = true;
        }
        else
        {
            transform.parent.GetComponent<Mushroom>().HasTarget = false;
        }
        if(transform.parent.GetComponent<Mushroom>().AttackCooldown > 0)
            transform.parent.GetComponent<Mushroom>().AttackCooldown -= Time.deltaTime;
    }
}
