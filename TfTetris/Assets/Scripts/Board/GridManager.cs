using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int columnNumber;
    [SerializeField] private int rowNumber;

    public List<Field> fields;
    private List<Field> sortedFields = new List<Field>();
    private Field currentActiveField;
    public float fieldsOffset;

    public static GridManager instance;

    [SerializeField] private Field fieldPrefab;
    private void Awake() {
        instance = this;
    }
    void Start() {
        SpawnGrid();
        sortedFields = ReturnSortedFields();
    }
    private List<Field> ReturnSortedFields() {
        //List<Field> fields = GridManager.instance.fields;
        Vector2 fieldSize = GetGridSize();/// x:column, y: row

        List<Field> sortedFields = new List<Field>();
        int checkingRowNumber = 1;
        for (int field = 0; field < fields.Count; field++) {

            Field currentField = fields[(fields.Count - 1 * checkingRowNumber) - (int)fieldSize.y * field];
            sortedFields.Add(currentField);
            if (field == (int)(fieldSize.x - 1)) {
                checkingRowNumber++;
                field = -1;
            }
            if (checkingRowNumber > (int)fieldSize.y) {
                break;
            }
        }
        return sortedFields;
    }
    private void SpawnGrid()
    {
        fields.Clear();
        for (int rowIndex = 0; rowIndex < columnNumber; rowIndex++) {
            for (int columnIndex = 0; columnIndex < rowNumber; columnIndex++) {
                if (rowIndex % 2 == 0 && columnIndex % 2 == 0 || rowIndex % 2 == 1 && columnIndex % 2 == 1) {
                    SpawnPosition(rowIndex, columnIndex, true);
                }
                else {
                    SpawnPosition(rowIndex, columnIndex, false);
                }
            }
        }
    }
    private void Update() {
        GetFieldByPointerPosition(Pointer.pointerPosition);
    }
    private Field GetFieldByPointerPosition(Vector3 pointerPosition) {
        foreach (Field field in fields) {
            bool fieldOnMouse = pointerPosition.x > field.leftDownCorner.x + fieldsOffset && pointerPosition.x < field.rightDownCorner.x - fieldsOffset &&
                                pointerPosition.z > field.leftDownCorner.z + +fieldsOffset && pointerPosition.z < field.leftUpperCorner.z - fieldsOffset;
            if (fieldOnMouse) {
                field.ActiveOutline(true);
                return currentActiveField = field;
            }
            currentActiveField = null;
            field.ActiveOutline(false);
        }
        return null;
    }
    private void SpawnPosition(int row, int column, bool white) {
        float fieldWidth = fieldPrefab.GetModelSize();

        float fieldXPosition = row * fieldWidth + fieldsOffset + transform.position.x;
        float fieldYPosition = column * fieldWidth + fieldsOffset + transform.position.z;
        Vector3 spawnPosition = new Vector3(fieldXPosition, transform.position.y, fieldYPosition);

        var field = Instantiate(fieldPrefab, spawnPosition, Quaternion.identity);

        if (white) {
            field.CreateField(column,row,true);
        }
        else {
            field.CreateField(column, row, false);
        }

        field.transform.name = "(" + row.ToString() + "," + column.ToString() + ")";
        field.transform.SetParent(this.transform);
        fields.Add(field);
    }

    private void DisableFieldByIndex(int x, int y) {
        Field field = GetFieldByIndex(x, y);
        field.gameObject.SetActive(false);
    }
    public  Field GetFieldByIndex(int column, int row) {
        for (int i = 0; i < fields.Count; i++) {
            Vector2 currentFieldCoordinate = fields[i].coordinates;
            if (currentFieldCoordinate.x == column && currentFieldCoordinate.y == row) {
                return fields[i];
            }
        }
        return null;
    }
    public Vector2 GetIndexByField(Field field) {
        return field.coordinates;
    }

    public Field GetUpFrontField(Field searchField) {

        float searchRow = searchField.coordinates.x;
        float searchColumn = searchField.coordinates.y;
        return GetFieldByIndex((int)searchRow, (int)(searchColumn + 1));
    }
    public Field GetNextEmptyField(Field searchField) {
        Field upfrontField = GetUpFrontField(searchField);
        while (true) {
            if (upfrontField.IsEmpty()) {
                return upfrontField;
            }
            else {
                upfrontField = GetUpFrontField(upfrontField);
            }
            if(upfrontField == null) {
                break;
            }
        }
        return null;

    }
    public Vector2 GetGridSize() {
        return new Vector2(columnNumber, rowNumber);
    }
    public void CleardFields() {
        foreach(Field field in fields) {
            field.ClearField();
        }
    }
    public Field GetCurrentActiveField() { 
        return currentActiveField;
    }
    public List<Field> GetSortedFields() {
        return sortedFields;
    }
}
