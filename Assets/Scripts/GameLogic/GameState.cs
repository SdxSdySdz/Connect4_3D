using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState
{
    private Board _board;
    private PlayerColor _playerToMove;
    private GameState _previousState;
    private Move _lastMove;
    private readonly int _winningStoneCount = 4;

    public static GameState NewGame => new GameState(new Board(), PlayerColor.White, null, Move.EmptyMove);
    // public bool IsOver => CheckWinByLastMoveDebug();
    public Board Board => _board.Copy();
    public PlayerColor PlayerToMove => _playerToMove;
    public Move LastMove => new Move(_lastMove);


    private GameState(Board board, PlayerColor playerToMove, GameState previousState, Move lastMove)
    {
        _board = board;
        _playerToMove = playerToMove;
        _previousState = previousState;
        _lastMove = lastMove;
    }


    public GameState ApplyMove(Move move)
    {
        Board nextBoard = _board.Copy();
        nextBoard.PlaceStone(_playerToMove, move);
        return new GameState(nextBoard, _playerToMove.Opposite, this, move);
    }


    public GameState ReturnMove()
    {
        if (_previousState != null)
        {
            return _previousState;
        }
        else
        {
            return this;
        }
    }


    public bool IsValidMove(Move move)
    {
        return _board.IsValidIndex(move.Row, move.Column) && _board.IsAvailablePeak(move.Row, move.Column);
    }


    public bool IsOver(out List<(int Row, int Column, int Peak)> winningIndices)
    {
        return CheckWinByLastMoveDebug(out winningIndices);
    }


    private bool CheckWinByLastMoveDebug(out List<(int Row, int Column, int Peak)> winningIndices)
    {
        if (_lastMove == Move.EmptyMove)
        {
            winningIndices = null;
            return false;
        }

        winningIndices = new List<(int Row, int Column, int Peak)>();
        int row = _lastMove.Row;
        int column = _lastMove.Column;
        int peak = _board.GetHighestPeak(row, column);

        int rowHorizontalSum = Mathf.Abs(_board.GetHorizontalSumByRowPlane(column, peak));
        if (rowHorizontalSum == _winningStoneCount)
        {
            Debug.Log("rowHorizontalWin");
            for (int i = 0; i < _winningStoneCount; i++)
            {
                winningIndices.Add((i, column, peak));
            }
            
            return true;
        }

        int columnHorizontalSum = Mathf.Abs(_board.GetHorizontalSumByColumnPlane(row, peak));
        if (columnHorizontalSum == _winningStoneCount)
        {
            Debug.Log("columnHorizontalWin");
            for (int i = 0; i < _winningStoneCount; i++)
            {
                winningIndices.Add((row, i, peak));
            }

            return true;
        }

        int verticalSum = Mathf.Abs(_board.GetVerticalSum(row, column));
        if (verticalSum == _winningStoneCount)
        {
            Debug.Log("VerticalWin");
            for (int i = 0; i < _winningStoneCount; i++)
            {
                winningIndices.Add((row, column, i));
            }

            return true;
        }

        List<int[]> diagonals = _board.GetAllDiagonals();
        foreach (var diagonal in diagonals)
        {
            if (Mathf.Abs(diagonal.Sum()) == _winningStoneCount)
            {
                Debug.Log("DiagonalWin");

                return true;
            }
        }

        winningIndices = null;
        return false;
    }
}
