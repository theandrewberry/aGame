using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;    // Reference to the bullet prefab
    public Transform bulletSpawn;      // Reference to the bullet spawn point
    public float bulletSpeed = 20f;    // Speed of the bullet

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Fire1 is usually mapped to the left mouse button
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the bullet at the bulletSpawn position and rotation
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bulletSpawn.forward * bulletSpeed;
    }
}