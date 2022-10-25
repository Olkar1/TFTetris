using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int rowNumber;
    [SerializeField] private int columnNumber;
    [SerializeField] private Field fieldPrefab;
    public List<Field> fields;


    [SerializeField]private bool createOnStart;

    public Vector2 temp;
    private void Awake() {
        ClearFields();
    }
    void Start()
    {
        if (createOnStart) {
            SpawnGrid();
        }

    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.D)) {
            DisableFieldByIndex((int)temp.x, (int)temp.y);
        }
    }
    private void SpawnGrid() {
        for (int rowIndex = 0; rowIndex < rowNumber; rowIndex++) {
            for (int columnIndex = 0; columnIndex < columnNumber; columnIndex++) {
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
        int cubeOffset = 20;
        Vector3 spawnPosition = new Vector3(row * cubeOffset, 0, column * cubeOffset);
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
    private Field GetFieldByIndex(int x, int y) {
        for (int i = 0; i < fields.Count; i++) {
            Vector2 currentFieldCoordinate = fields[i].coordinates;
            if (currentFieldCoordinate.x == x && currentFieldCoordinate.y == y) {
                return fields[i];
            }
        }
        return null;
    }
}
