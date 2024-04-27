using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject prolectilePrefab;

    public void FireProjectile()
    {
        GameObject projectile = Instantiate(prolectilePrefab, launchPoint.position, prolectilePrefab.transform.rotation);
        Vector3 origScale = projectile.transform.localScale;
        projectile.transform.localScale = new Vector3(
            origScale.x * transform.localScale.x > 0 ? 0.3f : -0.3f, 
            origScale.y,
            origScale.z
        );
    }
}