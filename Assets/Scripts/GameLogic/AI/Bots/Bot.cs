using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bot
{
    protected OpeningBook _openingBook;

    public OpeningBook OpeningBook => _openingBook;
    public event Action<Move> MoveSelected;


    protected Bot()
    {
        _openingBook = new OpeningBook();
    }


    protected abstract Policy ApplyStrategy(GameState gameState);


    public Move SelectMove(GameState gameState)
    {
        (Move move, double score) = ApplyStrategy(gameState).BestItem;
        MoveSelected?.Invoke(move);

        Debug.LogError($"Bot score: {score}");

        return move;
    }
}
