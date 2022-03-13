using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    // [SerializeField] protected BoardViewer _boardViewer;
    [SerializeField] private History _history;

    protected GameState _gameState;
    protected bool _isOver;

    public UnityEvent<GameState> MoveApplied;


    protected virtual void Awake()
    {
        _gameState = GameState.NewGame;
        _isOver = false;
        _history.Init(1, _gameState.Board.Size / 2);
    }


    public void OnReturnMoveButtonClicked()
    {
        _gameState = _gameState.ReturnMove();
        _isOver = false;
        _history.ReturnMove();
    }


    public void OnResetButtonClicked()
    {
        _gameState = GameState.NewGame;
        _isOver = false;
        _history.Clear();
    }


    protected void ApplyMove(Move move)
    {
        _gameState = _gameState.ApplyMove(move);
        MoveApplied?.Invoke(_gameState);

        _history.Add(move);
    }
}
