using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Policy
{
    private Dictionary<Move, double> _policy;

    public Move BestMove
    {
        get
        {
            double maxScore = double.MinValue;
            Move candidate = Move.EmptyMove;
            foreach (var item in _policy)
            {
                if (item.Value > maxScore)
                {
                    maxScore = item.Value;
                    candidate = item.Key;
                }
            }

            return candidate;
        }
    }
    public (Move, double) BestItem
    {
        get
        {
            double maxScore = double.MinValue;
            Move candidate = Move.EmptyMove;
            foreach (var item in _policy)
            {
                if (item.Value > maxScore)
                {
                    maxScore = item.Value;
                    candidate = item.Key;
                }
            }

            return (candidate, maxScore);
        }
    }


    public Policy()
    {
        _policy = new Dictionary<Move, double>();
    }

    public Policy(Move bestMove) : this()
    {
        Add(bestMove, Evaluator.MaxScore);
    }

    public void Add(Move move, double score)
    {
        _policy[move] = score;
    }


    public double GetScore(Move move)
    {
        if (_policy.TryGetValue(move, out double score))
        {
            return score;
        }
        else
        {
            return Evaluator.MinScore;
        }
    }
}
