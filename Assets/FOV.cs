using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    public GameObject bulletPrefab;    // Reference to the bullet prefab
    public Transform bulletSpawn;      // Reference to the bullet spawn point
    public float bulletSpeed = 20f; 

    private float shootCooldown = 1f; // Time in seconds between shots
    private float lastShotTime;       // Time when the last shot was fired

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
         transform.Rotate(0, 100 * Time.deltaTime, 0);

        if(canSeePlayer)
        {
            Aim();
            Shoot();
        }
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    //used to shoot bullet at enemy
    void Shoot()
    {
        if (Time.time - lastShotTime >= shootCooldown)
        {
            // Instantiate the bullet at the bulletSpawn position and rotation
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = bulletSpawn.forward * bulletSpeed;

            // Update the time when the last shot was fired
            lastShotTime = Time.time;
        }
    }

    //used to aim at enemy
    void Aim()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = (playerRef.transform.position - transform.position).normalized;

        // Calculate the new rotation
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        // Apply the rotation to the enemy
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}