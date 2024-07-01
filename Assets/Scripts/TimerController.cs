using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    [SerializeField] private float timeCounter;
    [SerializeField] private float countdownTimer = 120f;
    [SerializeField] private int minutes;
    [SerializeField] private int seconds;

    [SerializeField] private TextMeshProUGUI timerText;

    private bool isRunning = true; // Boolean to check if the timer is running

    private void Update()
    {
        if (isRunning)
        {
            timeCounter += Time.deltaTime;
            minutes = Mathf.FloorToInt(timeCounter / 60f);
            seconds = Mathf.FloorToInt(timeCounter - minutes * 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
