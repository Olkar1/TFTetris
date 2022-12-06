using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] public int enemyHealth;
    public int initHealth;
    [SerializeField] private List<BoardScenerio> enemyScenerios;
    private void Start() {
        initHealth = enemyHealth;
        GameManager.instance.enemyHealth.text = "Enemy health: " + enemyHealth.ToString();
    }
    public List<BoardScenerio> GetEnemyScenerios() {
        return enemyScenerios;
    }
    public int GetEnemyHealth() {
        return enemyHealth;
    }
    public void DamageEnemy(int value) {
        enemyHealth -= value;
        if (enemyHealth<=0) {
            Kill();
        }
        GameManager.instance.enemyHealth.text = "Enemy health: " + enemyHealth.ToString();
    }
    public void HealEnemy(int value) {
        enemyHealth += value;
        GameManager.instance.enemyHealth.text = "Enemy health: " + enemyHealth.ToString();
    }
    private void Kill() {
        Debug.LogError("Enemy Defeted");
        GameManager.instance.enemyDeathEvent();
    }
}
