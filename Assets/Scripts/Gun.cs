using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    public UnityEvent OnGunShoot;
    public float FireCooldown;
    public bool Automatic;
    private float CurrentCooldown;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        CurrentCooldown = FireCooldown;
        audio = GetComponent <AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Automatische modus
        if (Automatic)
        {
            if (Input.GetMouseButton(0) && CurrentCooldown <= 0f)
            {
                Shoot();
            }
        }
        // Handmatige modus
        else 
        {
            if (Input.GetMouseButtonDown(0) && CurrentCooldown <= 0f)
            {
                Shoot();
            }
        }

        // Cooldown bijwerken
        CurrentCooldown -= Time.deltaTime;
    }

    // Schiet logica
    private void Shoot()
    {
        OnGunShoot?.Invoke();
        CurrentCooldown = FireCooldown;
        audio.PlayOneShot(audio.clip);
    }
}
