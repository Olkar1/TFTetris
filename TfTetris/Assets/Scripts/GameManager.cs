using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    private static GameStatus gameStatus;
    public Transform monstersParent;
    public Transform specialObjectParent;
    public static GameManager instance;
    private int lastGameScore = 0;

    [SerializeField] private Timer timer;
    [SerializeField] private Shop shop;

    /// <summary>
    /// TEMP
    /// </summary>
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI dmgText;
    [SerializeField] private TextMeshProUGUI corutineActive;
    [SerializeField] private TextMeshProUGUI battleLog;
    [SerializeField] private TextMeshProUGUI goldText;
    public List<BoardScenerio> boardScenerios;
    public int gold = 10;
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
    void Start()
    {
        StartGame();
    }

    void Update()
    {
        switch (gameStatus) {
            case GameStatus.MoveMonsters:
                StartCoroutine(ReleseMonsters());
                SetGameStatus(GameStatus.Wait);
                break;
            case GameStatus.CalculatingScore:
                StartCoroutine(CalculateScoreAndLounchSpecialObject());
                SetGameStatus(GameStatus.Wait);
                break;
            case GameStatus.PrepereNextRound:
                battleLog.text = "";
                ClearMonsters();
                ClearSpecialObjects();
                GridManager.instance.CleardFields();

                shop.SetNewMonsters(true);
                timer.ResetTimer();
                SetGold(15);

                SetGameStatus(GameStatus.Shoping);
                SetBoardScenerio(GetRandomScenerio());
                break;
            case GameStatus.Shoping:
                break;
            case GameStatus.Wait:
                break;
        }
    }
    /// Iteration start from upper right corner
    /// direction of iteration : Left,down
    private IEnumerator ReleseMonsters() {
        List<Field> fields = GridManager.instance.fields;
        Vector2 fieldSize = GridManager.instance.GetGridSize();/// x:column, y: row

        int checkingRowNumber = 1;
        for (int field = 0; field < fields.Count; field++) {
            Field monsterField = fields[(fields.Count - 1 * checkingRowNumber) - (int)fieldSize.y * field];
            if (monsterField.GetMonster()) {
                yield return StartCoroutine(monsterField.GetMonster().MoveMonster());
                monsterField.SetMonster(null);
            }
            if (field == (int)(fieldSize.x - 1)) {
                checkingRowNumber++;
                field = -1;
            }
            if (checkingRowNumber > (int)fieldSize.y) {
                break;
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
        for (int monsterIndex = 0; monsterIndex< monstersParent.childCount; monsterIndex++) {
            Destroy(monstersParent.GetChild(monsterIndex).gameObject);
        }
    }
    private void ClearSpecialObjects() {
        for (int specialObjIndex = 0; specialObjIndex < specialObjectParent.childCount; specialObjIndex++) {
            Destroy(specialObjectParent.GetChild(specialObjIndex).gameObject);
        }
    }
    private IEnumerator CalculateScoreAndLounchSpecialObject() {
        lastGameScore = 0;
        List<Field> fields = GridManager.instance.fields;
        Vector2 fieldSize = GridManager.instance.GetGridSize();/// x:column, y: row
        int scoreInRow = 0;
        int totalScore = 0;
        int checkingRowNumber = 1;
        for (int field = 0; field < fields.Count; field++) {
            Field checkingField = fields[(fields.Count - 1 * checkingRowNumber) - (int)fieldSize.y * field];
            if (checkingField.GetSpecialObject()) {
                battleLog.text += checkingField.GetSpecialObject().effect + "\n";
            }
            if (checkingField.scored) {
                scoreInRow++;
            }
            if (field == (int)(fieldSize.x - 1)) {
                if(scoreInRow == fieldSize.x) {
                    scoreInRow = scoreInRow * 2;
                }
                checkingRowNumber++;
                field = -1;
                totalScore += scoreInRow;
                scoreInRow = 0;
            }
            if (checkingRowNumber > (int)fieldSize.y) {
                break;
            }
        }
        lastGameScore = totalScore;
        dmgText.text = "Dmg done: " + lastGameScore;
        int deleyBeetwenRounds = 3;

        yield return new WaitForSeconds(deleyBeetwenRounds);
        SetGameStatus(GameStatus.PrepereNextRound);
    }
    private void StartGame() {

        SetGameStatus(GameStatus.Shoping);
        SetBoardScenerio(GetRandomScenerio());
        shop.SpawnIcons();
        shop.SetNewMonsters(true);
        SetGold(15);
    }
    private void SetBoardScenerio(BoardScenerio scenerio) {
        foreach (var special in scenerio.scenerioObjects) {
            for (int i = 0; i< special.positions.Count;i++) {
                SpecialObject specialObject = Instantiate(special.objectToSpawn);
                specialObject.transform.SetParent(specialObjectParent);
                GridManager.instance.GetFieldByIndex((int)special.positions[i].x, (int)special.positions[i].y).SetSpecialObject(specialObject);
            }
        }
    }
    public void SubstractGold(int value) {
        gold -= value;
        goldText.text = "Gold: " + gold;
    }
    public void SetGold(int value) {
        gold = value;
        goldText.text = "Gold: " + gold;
    }
    private BoardScenerio GetRandomScenerio() {
        int numberOfScenerios = boardScenerios.Count;
        int randomScenerioIndex = Random.Range(0, numberOfScenerios);
        return boardScenerios[randomScenerioIndex];
    }
}
