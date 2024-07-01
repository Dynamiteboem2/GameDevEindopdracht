using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    private RespawnManager respawnManager;

    void Start()
    {
        respawnManager = FindObjectOfType<RespawnManager>();
        if (respawnManager == null)
        {
            Debug.LogError("RespawnManager not found in the scene!");
        }
        else
        {
            Debug.Log("RespawnManager successfully found.");
        }
    }
}


