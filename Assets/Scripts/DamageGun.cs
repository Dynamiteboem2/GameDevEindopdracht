using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGun : MonoBehaviour
{
    public float Damage;
    public float BulletRange;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    public void Shoot()
    {
        Ray gunRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(gunRay, out RaycastHit hitInfo, BulletRange))
        {
            Entity enemy = hitInfo.collider.gameObject.GetComponent<Entity>();
            if (enemy != null)
            {
                enemy.Health -= Damage;
            }
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Check voor de linkermuisknop (indrukken)
        {
            Shoot();
        }

        if (Input.GetKey(KeyCode.Mouse0)) // Check voor het vasthouden van de linkermuisknop
        {
            Debug.Log("Holding left mouse button");
            // Voeg hier eventuele functionaliteit toe die moet worden uitgevoerd tijdens het ingedrukt houden van de linkermuisknop
        }
    }
}
