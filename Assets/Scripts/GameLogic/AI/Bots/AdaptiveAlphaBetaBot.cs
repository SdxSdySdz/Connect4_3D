using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveAlphaBetaBot : AlphaBetaBot
{
    public AdaptiveAlphaBetaBot() : base(1) { }


    protected override Policy ApplyStrategy(GameState gameState)
    {
        float threshold = 0.75f;

        if (gameState.Board.StoneCount <= threshold * gameState.Board.Size)
        {
            MaxDepth = 5;
        }
        else if (gameState.Board.StoneCount <= 0.25 * gameState.Board.Size)
        {
            MaxDepth = 6;
        }
        else
        {
            MaxDepth = 9;
        }
        return base.ApplyStrategy(gameState);
    }
}
