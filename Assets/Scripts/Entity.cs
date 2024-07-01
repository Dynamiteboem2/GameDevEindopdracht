using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float StartingHealth;
    private float health;
    private RespawnManager respawnManager;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            Debug.Log(health);

            if (health <= 0f)
            {
                Debug.Log("Entity health is 0, destroying the entity.");
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        health = StartingHealth; // Stel de startwaarde van health in
        respawnManager = FindObjectOfType<RespawnManager>();
        if (respawnManager == null)
        {
            Debug.LogError("RespawnManager not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
