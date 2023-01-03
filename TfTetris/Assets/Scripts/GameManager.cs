using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    private static GameStatus gameStatus;
    public int playerInitialGold;

    private Enemy currentEnemy;

    public delegate void OnEnemyDeath();
    public OnEnemyDeath enemyDeathEvent;
    /// <summary>
    /// TEMP
    /// </summary>
    [SerializeField] public TextMeshProUGUI playerHealth;
    [SerializeField] public TextMeshProUGUI enemyHealth;
    [SerializeField] private TextMeshProUGUI corutineActive;
    [SerializeField] private TextMeshProUGUI battleLog;
    [SerializeField] private TextMeshProUGUI goldText;
    /// <summary>
    /// 
    /// </summary>
    public Transform monstersParent;
    public Transform specialObjectParent;
    public Transform movementObjectsParent;

    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private Player player;
    [SerializeField] private Timer timer;
    [SerializeField] private Shop shop;
    [SerializeField] private GridManager grid;

    public static GameManager instance;

    private void Awake() {
        instance = this;
        enemyDeathEvent = SetRandomEnemy;
    }
    public enum GameStatus {
        Shoping,
        MoveMonsters,
        CalculatingScore,
        PrepereNextRound,
        Wait
    }
    void Start() {
        StartCoroutine(StartGame());
    }
    private IEnumerator StartGame() {
        ///SpawnGrid
        //grid.SpawnGrid();
        yield return StartCoroutine(grid.SpawnGrid());
        Debug.LogError("GaMeStarted");
        SetRandomEnemy();
        shop.SpawnIcons();
        SetGameStatus(GameStatus.PrepereNextRound);
    }
    public void SetGameStatus(GameStatus status) {
        gameStatus = status;
        if (status == GameStatus.Wait) {
            corutineActive.text = "Corutine active";
        }
        else {
            corutineActive.text = "";
        }
    }
    private void SetBoardScenerio(BoardScenerio scenerio) {
        if (!scenerio) { return; }
        foreach (var special in scenerio.scenerioObjects) {
            for (int i = 0; i < special.positions.Count; i++) {
                SpecialObject specialObject = Instantiate(special.objectToSpawn);
                specialObject.transform.SetParent(specialObjectParent);
                GridManager.instance.GetFieldByIndex((int)special.positions[i].x, (int)special.positions[i].y).SetSpecialObject(specialObject);
            }
        }
    }
    void Update() {
        UpdateGameStatus();
    }
    private void UpdateGameStatus() {
        if (!currentEnemy) { return; }
        switch (gameStatus) {
            case GameStatus.MoveMonsters:
                SetGameStatus(GameStatus.Wait);
                StartCoroutine(ReleseMonsters());
                Pointer.hold = false;
                break;
            case GameStatus.CalculatingScore:
                SetGameStatus(GameStatus.Wait);
                StartCoroutine(CalculateDamageAndLaunchSpecialObject());
                break;
            case GameStatus.PrepereNextRound: /// SET TO CORUTINE
                SetGameStatus(GameStatus.Wait);
                StartCoroutine(LaunchNextRound());
                break;
            case GameStatus.Shoping:
                break;
            case GameStatus.Wait:
                break;
        }
    }
    private IEnumerator ReleseMonsters() {
        List<Field> fields = grid.GetSortedFields();

        foreach (var field in fields) {
            if (field.GetMonster()) {
                Monster monster = field.GetMonster();
                yield return StartCoroutine(monster.MoveMonster());
                field.SetMonster(null);
            }
        }
        SetGameStatus(GameStatus.CalculatingScore);
        yield break;
    }
    private IEnumerator CalculateDamageAndLaunchSpecialObject() {
        int rowSize = (int)grid.GetGridSize().x;
        List<Field> fields = grid.GetSortedFields();
        int totalDmg = 0;

        int scoreInRow = 0;
        int currentFieldIndex = -1;
        foreach (var field in fields) {
            currentFieldIndex++;
            LaunchSpecialObject(field);
            if (field.scored) {
                scoreInRow++;
            }
            if (currentFieldIndex == rowSize) {
                currentFieldIndex = -1;
                if (ShouldDoubleScore(ref scoreInRow)) {
                    scoreInRow = scoreInRow * 2;
                }
                totalDmg += scoreInRow;
                scoreInRow = 0;
            }
        }
        currentEnemy.DamageEnemy(totalDmg);
        int deleyBeetwenRounds = 3;

        yield return new WaitForSeconds(deleyBeetwenRounds);
        SetGameStatus(GameStatus.PrepereNextRound);
    }
    private IEnumerator LaunchNextRound() {
        battleLog.text = "";
        yield return ClearBoard();

        GridManager.instance.CleardFields();

        shop.SetNewMonsters(true);
        timer.ResetTimer();
        Player.SetGold(playerInitialGold);

        SetBoardScenerio(GetRandomScenerio());
        SetGameStatus(GameStatus.Shoping);
    }
    private IEnumerator ClearBoard() {
        yield return ClearMonsters();
        yield return ClearSpecialObjects();
        yield return ClearMovementObjects();
    }
    private IEnumerator ClearMonsters() {
        for (int monsterIndex = 0; monsterIndex < monstersParent.childCount; monsterIndex++) {
            Destroy(monstersParent.GetChild(monsterIndex).gameObject);
        }
        yield break;
    }
    private IEnumerator ClearSpecialObjects() {
        for (int specialObjIndex = 0; specialObjIndex < specialObjectParent.childCount; specialObjIndex++) {
            Destroy(specialObjectParent.GetChild(specialObjIndex).gameObject);
        }
        yield break;
    }
    private IEnumerator ClearMovementObjects() {
        for (int specialObjIndex = 0; specialObjIndex < movementObjectsParent.childCount; specialObjIndex++) {
            Destroy(movementObjectsParent.GetChild(specialObjIndex).gameObject);
        }
        yield break;
    }
    private void LaunchSpecialObject(Field field) {
        if (field.GetSpecialObject()) {
            SpecialObject specialObject = field.GetSpecialObject();
            specialObject.specialEffect();
        }
    }
    private bool ShouldDoubleScore(ref int scoreInRow) {
        int rowSize = (int)grid.GetGridSize().x;
        if (scoreInRow == rowSize) {
            return true;
        }
        return false;
    }

    public void SubstractGold(int value) {
        Player.SubstractGold(value);
        goldText.text = "Gold: " + Player.GetPlayerGold();
    }
    private void SetRandomEnemy() {
        int enemyIndex = Random.Range(0, enemies.Count);
        SetCurrentEnemy(enemies[enemyIndex]);
    }
    private void SetCurrentEnemy(Enemy enemyToSet) {
        if (currentEnemy) {
            Destroy(currentEnemy.gameObject);
        }
        currentEnemy = Instantiate(enemyToSet);
    }
    public void SetNewEnemy() {
        SetRandomEnemy();
        Player.playerHealth = 15;
        currentEnemy.ResetHealth();
        playerHealth.text = Player.playerHealth.ToString();
        enemyHealth.text = "Enemy health: " + currentEnemy.GetEnemyHealth().ToString();
    }
    private BoardScenerio GetRandomScenerio() {
        int numberOfScenerios = currentEnemy.GetEnemyScenerios().Count;
        int randomScenerioIndex = Random.Range(0, numberOfScenerios);
        return currentEnemy.GetEnemyScenerios()[randomScenerioIndex];
    }
    public GameStatus GetGameStatus() {
        return gameStatus;
    }
    public Enemy GetCurrentEnemy() {
        return currentEnemy;
    }
}
