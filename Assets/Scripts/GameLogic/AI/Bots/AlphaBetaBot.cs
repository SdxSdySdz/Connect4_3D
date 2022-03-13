using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlphaBetaBot : Bot
{
    protected int MaxDepth;


    public AlphaBetaBot(int maxDepth) : base()
    {
        if (maxDepth < 1) throw new Exception("Incorrect max depth");

        MaxDepth = maxDepth;
    }


    protected override Policy ApplyStrategy(GameState gameState)
    {
        Policy policy = new Policy();

        GameState nextState;
        double opponentBestOutcome;
        double ourBestOutcome;

        double bestScore = Evaluator.MinScore;
        double bestBlack = Evaluator.MinScore;
        double bestWhite = Evaluator.MinScore;

        if (_openingBook.TryGetBestMove(gameState.Board, out Move bestMove))
        {
            return new Policy(bestMove);
        }

        Move possibleMove;
        var possibleMoves = gameState.GetPossibleMoves();
        for (int i = 0; i < possibleMoves.Count; i++)
        {
            possibleMove = possibleMoves[i];
            nextState = gameState.ApplyMove(possibleMove);

            opponentBestOutcome = OpponentBestScore(nextState, MaxDepth - 1, bestWhite, bestBlack);
            ourBestOutcome = -opponentBestOutcome;


            if (ourBestOutcome > bestScore)
            {
                bestScore = ourBestOutcome;

                if (gameState.PlayerColorToMove == PlayerColor.Black)
                {
                    bestBlack = bestScore;
                }
                else if (gameState.PlayerColorToMove == PlayerColor.White)
                {
                    bestWhite = bestScore;
                }
            }

            policy.Add(possibleMove, ourBestOutcome);
        }

        return policy;
    }


    private double OpponentBestScore(GameState game, int depth, double bestWhite, double bestBlack)
    {
        double score;
        PlayerColor opponentColor = game.PlayerColorToMove;

        /*** CHECK OPENING BOOK ***/
/*        if (_openingBook.TryGetScore(game.Board, opponentColor, out score))
        {
            return score;
        }*/

        /*** CHECK WINNER ***/
        if (game.TryGetWinner(out PlayerColor winnerColor, out var winningIndices))
        {
            GameState previousState = game.ReturnMove();
            _openingBook.Add(previousState.Board, game.LastMove);
            score = Evaluator.MinScore - depth;

            return score;
        }

        /*** CHECK END OF SEARCHING ***/
        if (depth == 0) 
        {
            score = Evaluator.WeightOrientedScore(game.Board);
            return game.PlayerColorToMove.IsWhite ? score : -score;
        }
        


        GameState nextState;
        double bestOurResult = Evaluator.MinScore-100;
        double opponentBestResult;
        double ourResult;
        double outcomeForBlack;
        double outcomeForWhite;

        var possibleMoves = game.GetPossibleMoves();
        foreach (var candidate in possibleMoves)
        {
            nextState = game.ApplyMove(candidate);

            opponentBestResult = OpponentBestScore(nextState, depth - 1, bestWhite, bestBlack);
            ourResult = -opponentBestResult;
/*            if (ourResult >= Evaluator.MaxScore)
            {
                _openingBook.Add(game.Board, ourResult);
            }*/

            if (ourResult > bestOurResult)
            {
                bestOurResult = ourResult;
            }

            if (game.PlayerColorToMove.IsWhite)
            {
                if (bestOurResult > bestWhite) bestWhite = bestOurResult;
                outcomeForBlack = -bestOurResult;

                if (outcomeForBlack < bestBlack) return bestOurResult;

            }
            else if (game.PlayerColorToMove.IsBlack)
            {
                if (bestOurResult > bestBlack) bestBlack = bestOurResult;
                outcomeForWhite = -bestOurResult;

                if (outcomeForWhite < bestWhite) return bestOurResult;
            }
        }

        // _openingBook.Add(game.Board, score);
        return bestOurResult;
    }
}
