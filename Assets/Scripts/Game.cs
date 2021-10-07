using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private BoardViewer _boardViewer;

    private GameState _gameState;
    private Dictionary<char, int> _notationMap;

    private void Start()
    {
        _notationMap = new Dictionary<char, int>
        {
            {'A', 0 },
            {'B', 1 },
            {'C', 2 },
            {'D', 3 },
        };

        _gameState = GameState.NewGame;
    }


    public void OnPeakClicked(Move move)
    {
        List<(int Row, int Column, int Peak)> winningIndices;
        if (_gameState.IsValidMove(move))
        {
            int row = move.Row;
            int column = move.Column;
            int peak = _gameState.Board.GetHighestPeak(row, column) + 1;

            _boardViewer.SpawnStone(_gameState.PlayerToMove, move.Row, move.Column, peak);
            _gameState = _gameState.ApplyMove(move);

            if (_gameState.IsOver(out winningIndices))
            {
                _boardViewer.HighlightWinningStones(winningIndices);
            }
        }
        else
        {
            OnIncorrectMove();
        }
    }


    public void OnPeakEnter(Move move)
    {
        _boardViewer.HighlightPeak(move);
    }


    public void OnPeakExit(Move move)
    {
        _boardViewer.ResetHighlighting();
    }


    public void OnMoveButtonClicked(string moveNotation)
    {
        Move move = MoveFromNotation(moveNotation);
        if (_gameState.IsValidMove(move))
        {
            _boardViewer.SpawnStone(_gameState.PlayerToMove, move.Row, move.Column, 0);
            _gameState = _gameState.ApplyMove(move);  
        }
        else
        {
            OnIncorrectMove();
        }
    }


    public void OnReturnMoveButtonClicked()
    {
        _boardViewer.DeleteLastStone();
        _gameState = _gameState.ReturnMove();
    }


    public void OnResetButtonClicked()
    {
        _boardViewer.DeleteAllStones();
        _gameState = GameState.NewGame;
    }


    private void OnIncorrectMove()
    {

    }


    private Move MoveFromNotation(string moveNotation)
    {
        char letter = moveNotation[0];
        string strNumber = $"{moveNotation[1]}";

        Debug.Log($"Letter {letter}");
        Debug.Log($"StrNumber {strNumber}");

        if (Int32.TryParse(strNumber, out int row))
        {
            row--;
            int column = _notationMap[letter];

            Debug.Log($"row {row}");
            Debug.Log($"column {column}");

            return new Move(row, column);
        }

        throw new Exception();
        
    }
}
