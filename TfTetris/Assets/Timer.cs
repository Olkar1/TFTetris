using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private int initialTime;
    [SerializeField] private float timePassed;
    [SerializeField] private float timeLeft;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Shop shop;
    void Start()
    {
        
    }
    void Update()
    {
        UpdateTimerValue();


    }
    private void UpdateTimerValue() {
        if (GameManager.gameStatus != GameManager.GameStatus.shoping) { return; }

        timePassed += Time.deltaTime;
        timeLeft = initialTime - timePassed;
        timerSlider.value = timePassed / initialTime;
        if (timeLeft <=0) {
            GameManager.gameStatus = GameManager.GameStatus.fighting;
            ResetTimer();
        }
    }
    private void ResetTimer() {
        timerSlider.value = 0;
        timePassed = 0;
    }
}
