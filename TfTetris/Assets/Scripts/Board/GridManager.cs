using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int columnNumber;
    [SerializeField] private int rowNumber;

    [SerializeField] private Field fieldPrefab;
    public List<Field> fields;
    [HideInInspector]
    public Field currentActiveField;
    public float fieldsOffset ;

    public static GridManager instance;
    private void Awake() {
        instance = this;
        ClearFields();
    }
    void Start()
    {
        SpawnGrid();
    }
    private void Update() {
        GetFieldByPointerPosition(Pointer.pointerPosition);
    }
    private void SpawnGrid() {
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
    private void ClearFields() {
        fields.Clear();
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
    private Field GetFieldByPointerPosition(Vector3 pointerPosition) {
        foreach (Field field in fields) {
            if (pointerPosition.x > field.leftDownCorner.x + fieldsOffset && pointerPosition.x < field.rightDownCorner.x - fieldsOffset &&
                pointerPosition.z > field.leftDownCorner.z + +fieldsOffset && pointerPosition.z < field.leftUpperCorner.z - fieldsOffset) {
                field.ActiveOutline(true);
                return currentActiveField = field;
            }
            currentActiveField = null;
            field.ActiveOutline(false);
        }
        return null;
    }
    public Field GetUpFrontField(Field searchField) {

        float searchColumn = searchField.coordinates.x;
        float searchRow = searchField.coordinates.y;
        return GetFieldByIndex((int)searchColumn, (int)(searchRow + 1));
    }
    public Vector2 GetGridSize() {
        return new Vector2(columnNumber, rowNumber);
    }
    public void CleardFields() {
        foreach(Field field in fields) {
            field.scored = false;
            field.SetMonster(null);
        }
    }
}
