using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Move
{
    public int Row { get; private set; }
    public int Column { get; private set; }


    public Move(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public Move(Move lastMove) : this(lastMove.Row, lastMove.Column)
    {
    }

    public static Move EmptyMove => new Move(-1, -1);
    public static bool operator ==(Move a, Move b) => a.Row == b.Row && a.Column == b.Column;
    public static bool operator !=(Move a, Move b) => a.Row != b.Row || a.Column != b.Column;


    public override bool Equals(object obj)
    {
        if (obj is Move)
        {
            Move other = (Move)obj;

            return this == other;
        }
        else
        {
            return false;
        }
    }


    public override int GetHashCode()
    {
        return (Row, Column).GetHashCode();
    }


    public override string ToString()
    {
        return $"[Move]({Row}, {Column})";
    }
}
