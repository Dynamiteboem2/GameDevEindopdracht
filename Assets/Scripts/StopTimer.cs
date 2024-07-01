using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public TimerController timerController; // Referentie naar het TimerController script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Zorg ervoor dat de speler een "Player" tag heeft
        {
            timerController.StopTimer(); // Roep de StopTimer methode aan op de TimerController variabele
        }
    }
}
