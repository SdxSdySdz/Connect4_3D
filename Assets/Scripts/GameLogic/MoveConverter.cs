using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveConverter
{
    private static char[] _indexToLetter;


    static MoveConverter()
    {
        _indexToLetter = new char[] { 'A', 'B', 'C', 'D'};
    }


    public static string MoveToString(Move move)
    {
        if (move == Move.EmptyMove)
        {
            return "...";
        }
        else
        {
            return $"{_indexToLetter[move.Column]}{move.Row + 1}";
        }
    }
}
