using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HistoryRow : MonoBehaviour
{
    private Text _text;
    private Move _whiteMove;
    private Move _blackMove;
    private int _rowNumber;

    private bool IsEmpty => _whiteMove == Move.EmptyMove && _blackMove == Move.EmptyMove;


    private void Awake()
    {
        _text = GetComponent<Text>();
        Clear();
    }


    public void Init(int rowNumber)
    {
        _rowNumber = rowNumber;
    }


    public void Add(Move move)
    {
        if (_whiteMove == Move.EmptyMove)
        {
            _whiteMove = new Move(move.Row, move.Column);
        }
        else if (_blackMove == Move.EmptyMove)
        {
            _blackMove = new Move(move.Row, move.Column);
        }

        UpdateText();
    }


    public void ReturnMove()
    {
        if (_blackMove != Move.EmptyMove)
        {
            _blackMove = Move.EmptyMove;
        }
        else if (_whiteMove != Move.EmptyMove)
        {
            _whiteMove = Move.EmptyMove;
        }

        UpdateText();
    }

    public void Clear()
    {
        _whiteMove = Move.EmptyMove;
        _blackMove = Move.EmptyMove;

        UpdateText();
    }

    private void UpdateText()
    {
        if (IsEmpty) 
        {
            _text.text = "";
            return;
        }

        string whiteMove = MoveConverter.MoveToString(_whiteMove);
        string blackMove = MoveConverter.MoveToString(_blackMove);

        _text.text = $"{_rowNumber}. {whiteMove} - {blackMove}"; 
    }
}
