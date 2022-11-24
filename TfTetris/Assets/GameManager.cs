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
        moveMonsters,
        calculatingScore,
        showScore,
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
                gameStatus = GameStatus.wait;
                break;
            case GameStatus.calculatingScore:
                CalculateScore();
                gameStatus = GameStatus.showScore;
                break;
            case GameStatus.showScore:
                break;
            case GameStatus.wait:
                break;
        }
        if (gameStatus == GameStatus.moveMonsters) {


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
        gameStatus = GameStatus.calculatingScore;
    }
    private void CalculateScore() {
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
        Debug.LogError(totalScore);
    }
    private void StartGame() {
        gameStatus = GameStatus.shoping;
        shop.SpawnIcons();
    }
}
