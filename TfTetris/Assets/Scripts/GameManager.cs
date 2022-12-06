using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    private static GameStatus gameStatus;

    private int lastGameScore = 0;
    public int playerInitialGold;

    public List<BoardScenerio> boardScenerios;
    /// <summary>
    /// TEMP
    /// </summary>
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI dmgText;
    [SerializeField] private TextMeshProUGUI corutineActive;
    [SerializeField] private TextMeshProUGUI battleLog;
    [SerializeField] private TextMeshProUGUI goldText;
    /// <summary>
    /// 
    /// </summary>
    public Transform monstersParent;
    public Transform specialObjectParent;

    [SerializeField] private Player player;
    [SerializeField] private Timer timer;
    [SerializeField] private Shop shop;
    [SerializeField] private GridManager grid;

    public static GameManager instance;

    private void Awake() {
        instance = this;
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

    void Update() {
        UpdateGameStatus();
    }
    private void UpdateGameStatus() {
        switch (gameStatus) {
            case GameStatus.MoveMonsters:
                StartCoroutine(ReleseMonsters());
                SetGameStatus(GameStatus.Wait);
                break;
            case GameStatus.CalculatingScore:
                StartCoroutine(CalculateScoreAndLaunchSpecialObject());
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
    public void SetGameStatus(GameStatus status) {
        gameStatus = status;
        if (status == GameStatus.Wait) {
            corutineActive.text = "Corutine active";
        }
        else {
            corutineActive.text = "";
            statusText.text = status.ToString();
        }
    }
    public GameStatus GetGameStatus() {
        return gameStatus;
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
    private IEnumerator CalculateScoreAndLaunchSpecialObject() {
        int rowSize = (int)grid.GetGridSize().x;
        List<Field> fields = grid.GetSortedFields();
        int totalScore = 0;

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
                totalScore += scoreInRow;
                scoreInRow = 0;
            }
        }
        dmgText.text = "Dmg done: " + totalScore;
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
    private void StartGame() {

        SetGameStatus(GameStatus.Shoping);
        SetBoardScenerio(GetRandomScenerio());
        shop.SpawnIcons();
        shop.SetNewMonsters(true);
    }
    private void SetBoardScenerio(BoardScenerio scenerio) {
        foreach (var special in scenerio.scenerioObjects) {
            for (int i = 0; i < special.positions.Count; i++) {
                SpecialObject specialObject = Instantiate(special.objectToSpawn);
                specialObject.transform.SetParent(specialObjectParent);
                GridManager.instance.GetFieldByIndex((int)special.positions[i].x, (int)special.positions[i].y).SetSpecialObject(specialObject);
            }
        }
    }
    public void SubstractGold(int value) {
        Player.SubstractGold(value);
        goldText.text = "Gold: " + Player.GetPlayerGold();
    }
    private BoardScenerio GetRandomScenerio() {
        int numberOfScenerios = boardScenerios.Count;
        int randomScenerioIndex = Random.Range(0, numberOfScenerios);
        return boardScenerios[randomScenerioIndex];
    }
}
