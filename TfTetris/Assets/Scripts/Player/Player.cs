using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{

    public static int playerHealth = 15;
    public static int gold;
    private void Start() {
        SetGold(GameManager.instance.playerInitialGold);
        GameManager.instance.playerHealth.text = "Player health: " + playerHealth.ToString();
    }
    public static void SetGold(int value) {
        gold = value;
    }
    public static void DmgPlayer(int dmgDone) {
        playerHealth -= dmgDone;
        if (playerHealth <= 0) {
            KillPlayer();
        }
        GameManager.instance.playerHealth.text = "Player health: " + playerHealth.ToString();
    }
    private static void KillPlayer() {
        GameManager.instance.SetNewEnemy();
    }
    public static int GetPlayerGold() {
        return gold;
    }
    public static void SubstractGold(int value) {
        gold -= value;
    }
}
