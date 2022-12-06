using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{

    private static int playerHealth;
    public static int gold;
    private void Start() {
        SetGold(GameManager.instance.playerInitialGold);
    }
    public static void SetGold(int value) {
        gold = value;
    }
    public static void DmgPlayer(int dmgDone) {
        playerHealth -= dmgDone;
        if (playerHealth <= 0) {
            KillPlayer();
        }
    }
    private static void KillPlayer() {
        Debug.LogError("You are dead");
    }
    public static int GetPlayerGold() {
        return gold;
    }
    public static void SubstractGold(int value) {
        gold -= value;
    }
}
