using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    private static GameStatus gameStatus;
    public Transform monstersParent;
    public static GameManager instance;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int lastGameScore = 0;

    [SerializeField] private Timer timer;
    [SerializeField] private Shop shop;
    private void Awake() {
        instance = this;
    }
    public enum GameStatus {
        shoping,
        moveMonsters,
        calculatingScore,
        prepereNextRound,
        wait
    }
    void Start()
    {
        StartGame();
    }

    void Update()
    {
        switch (gameStatus) {
            case GameStatus.moveMonsters:
                StartCoroutine(ReleseMonsters());
                SetGameStatus(GameStatus.wait);
                break;
            case GameStatus.calculatingScore:
                StartCoroutine(CalculateScore());
                SetGameStatus(GameStatus.wait);
                break;
            case GameStatus.prepereNextRound:
                ClearMonsters();
                shop.SpawnIcons();
                timer.ResetTimer();
                SetGameStatus(GameStatus.shoping);
                break;
            case GameStatus.shoping:
                break;
            case GameStatus.wait:
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
        SetGameStatus(GameStatus.calculatingScore);
        yield return null;
    }
    public void SetGameStatus(GameStatus status) {
        gameStatus = status;
        statusText.text = status.ToString();
    }
    public GameStatus GetGameStatus() {
        return gameStatus;
    }
    private void ClearMonsters() {
        for (int monsterIndex = 0; monsterIndex< monstersParent.childCount; monsterIndex++) {
            Destroy(monstersParent.GetChild(monsterIndex).gameObject);
        }
    }
    private IEnumerator CalculateScore() {
        lastGameScore = 0;
        List<Field> fields = GridManager.instance.fields;
        Vector2 fieldSize = GridManager.instance.GetGridSize();/// x:column, y: row
        int scoreInRow = 0;
        int totalScore = 0;
        int checkingRowNumber = 1;
        for (int field = 0; field < fields.Count; field++) {
            Field monsterField = fields[(fields.Count - 1 * checkingRowNumber) - (int)fieldSize.y * field];
            if (monsterField.scored) {
                scoreInRow++;
            }
            if (field == (int)(fieldSize.x - 1)) {
                if(scoreInRow == fieldSize.x) {
                    Debug.LogError("Double points");
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
        scoreText.text = "Last score: " + lastGameScore;
        int deleyBeetwenRounds = 3;

        yield return new WaitForSeconds(deleyBeetwenRounds);
        SetGameStatus(GameStatus.prepereNextRound);
    }
    private void StartGame() {
        SetGameStatus(GameStatus.shoping);
        shop.SpawnIcons();
    }
}
