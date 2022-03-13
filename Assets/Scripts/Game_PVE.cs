using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using IJunior.TypedScenes;

public class Game_PVE : Game, ISceneLoadHandler<(PlayerColor userColor, Bot opponent)>
{
    [SerializeField] private User _user;
    private PlayerColor _userColor;
    private Bot _opponent;
    
    private void OnEnable()
    {
        _user.MoveSelected += OnMoveSelectedByUser;
    }

    private void OnDisable()
    {
        _user.MoveSelected -= OnMoveSelectedByUser;
    }
    
    private void Start()
    {
        if (_userColor.IsBlack)
        {
            MakeBotMove();
        }
    }
    
    public void OnSceneLoaded((PlayerColor userColor, Bot opponent) argument)
    {
        _opponent = argument.opponent;
        _userColor = argument.userColor;
    }
    
    private void OnMoveSelectedByUser(Move move)
    {
        if (_isOver == false && _gameState.IsValidMove(move))
        {
            ApplyMove(move);
            if (_gameState.TryGetWinner(out PlayerColor winnerColor, out List<(int Row, int Column, int Peak)> winningIndices))
            {
                OnWin();
                return;
            }

            MakeBotMove();
        }
    }

    private void OnWin()
    {
        Debug.LogError("You WIN!!!");
        _isOver = true;
    }

    private void OnLose()
    {
        Debug.LogError("You LOSE!!!");
        _isOver = true;
    }

    private void MakeBotMove()
    {
        Move opponentMove = _opponent.SelectMove(_gameState);
        ApplyMove(opponentMove);
        if (_gameState.TryGetWinner(out var winnerColor, out var winningIndices))
        {
            OnLose();
        }
    }
}
