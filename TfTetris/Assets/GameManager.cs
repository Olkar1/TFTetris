using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]private Timer timer;
    [SerializeField] private Shop shop;
    public static GameStatus gameStatus;
    private void Awake() {
    }
    public enum GameStatus {
        shoping,
        fighting,
        calculatingScore,
    }
    void Start()
    {
        StartGame();
    }

    void Update()
    {
        
    }
    private void StartGame() {
        gameStatus = GameStatus.shoping;
        shop.SpawnIcons();
    }
}
