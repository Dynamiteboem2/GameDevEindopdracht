using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamerespawn : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.transform.position = respawnPoint.transform.position;
            Physics.SyncTransforms();
        }
    }
}
