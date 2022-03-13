using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class History : MonoBehaviour
{
    [SerializeField] private HistoryRow _rowPrefab;

    private int _startIndex;
    private int _endIndex;
    private int _semiMoveCount;
    private List<HistoryRow> _rows;

    private int RowCount => _endIndex - _startIndex + 1;
    private int CurrentRowIndex => _semiMoveCount / 2;


    public void Init(int startIndex, int endIndex)
    {
        _startIndex = startIndex;
        _endIndex = endIndex;
        _rows = new List<HistoryRow>();
        for (int i = 0; i < RowCount; i++)
        {
            HistoryRow row = Instantiate(_rowPrefab, transform);
            row.Init(_startIndex + i);
            _rows.Add(row);
        }
    }


    public void Clear()
    {
        _semiMoveCount = 0;
        foreach (var row in _rows)
        {
            row.Clear();
        }
    }


    public void ReturnMove()
    {
        _semiMoveCount--;
        _rows[CurrentRowIndex].ReturnMove();
        
    }


    public void Add(Move move)
    {
        _rows[CurrentRowIndex].Add(move);
        _semiMoveCount++;
    }
}
