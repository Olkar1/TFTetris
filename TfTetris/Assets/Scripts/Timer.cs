using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private int initialTime;
    private float timePassed;
    private float timeLeft;
    [SerializeField] private Slider timerSlider;
    void Update()
    {
        UpdateTimerValue();
    }
    private void UpdateTimerValue() {
        if (GameManager.instance.GetGameStatus() != GameManager.GameStatus.Shoping) { return; }

        timePassed += Time.deltaTime;
        timeLeft = initialTime - timePassed;
        timerSlider.value = timePassed / initialTime;
        if (timeLeft <=0) {
            GameManager.instance.SetGameStatus(GameManager.GameStatus.MoveMonsters);
            ResetTimer();
        }
    }
    public void ResetTimer() {
        timerSlider.value = 0;
        timePassed = 0;
    }
}
