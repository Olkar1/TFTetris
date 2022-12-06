using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    private static GameStatus gameStatus;
    public int playerInitialGold;

    public Enemy currentEnemy;

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
        StartGame();
    }
    private void StartGame() {
        SetCurrentEnemy(GetRandomEnemy());
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
                StartCoroutine(ReleseMonsters());
                SetGameStatus(GameStatus.Wait);
                break;
            case GameStatus.CalculatingScore:
                StartCoroutine(CalculateDamageAndLaunchSpecialObject());
                SetGameStatus(GameStatus.Wait);
                break;
            case GameStatus.PrepereNextRound:
                battleLog.text = "";
                ClearMonsters();
                ClearSpecialObjects();
                GridManager.instance.CleardFields();

                shop.SetNewMonsters(true);
                timer.ResetTimer();
                Player.SetGold(playerInitialGold);

                SetGameStatus(GameStatus.Shoping);
                SetBoardScenerio(GetRandomScenerio());
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
        yield return new WaitForSeconds(1f);//DELEY WHEN NO MONSTERS
        SetGameStatus(GameStatus.CalculatingScore);
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
    private void ClearMonsters() {
        for (int monsterIndex = 0; monsterIndex < monstersParent.childCount; monsterIndex++) {
            Destroy(monstersParent.GetChild(monsterIndex).gameObject);
        }
    }
    private void ClearSpecialObjects() {
        for (int specialObjIndex = 0; specialObjIndex < specialObjectParent.childCount; specialObjIndex++) {
            Destroy(specialObjectParent.GetChild(specialObjIndex).gameObject);
        }
    }
    public void SubstractGold(int value) {
        Player.SubstractGold(value);
        goldText.text = "Gold: " + Player.GetPlayerGold();
    }
    private Enemy GetRandomEnemy() {
        int enemyIndex = Random.Range(0, enemies.Count);
        return enemies[enemyIndex];
    }
    private void SetCurrentEnemy(Enemy enemyToSet) {
        currentEnemy = enemyToSet;
    }
    public void SetRandomEnemy() {
        Player.playerHealth = 15;
        currentEnemy = GetRandomEnemy();
        currentEnemy.enemyHealth = currentEnemy.initHealth;
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
}
