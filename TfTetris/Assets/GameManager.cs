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
    }
    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (gameStatus == GameStatus.moveMonsters) {
            StartCoroutine(ReleseMonsters());
            gameStatus = GameStatus.calculatingScore;

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
    }
    private void StartGame() {
        gameStatus = GameStatus.shoping;
        shop.SpawnIcons();
    }
}
