using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PlayerColor
{
    public int ColorValue { get; private set; }
    public static PlayerColor White => new PlayerColor(1);
    public static PlayerColor Black => new PlayerColor(-1);
    public bool IsWhite => ColorValue == 1;
    public bool IsBlack => ColorValue == -1;

    public PlayerColor Opposite
    {
        get
        {
            if (IsWhite)
            {
                return Black;
            }
            else
            {
                return White;
            }
        }
    }


    private PlayerColor(int colorValue)
    {
        ColorValue = colorValue;
    }


    public static bool operator==(PlayerColor a, PlayerColor b)
    {
        return a.ColorValue == b.ColorValue;
    }


    public static bool operator !=(PlayerColor a, PlayerColor b)
    {
        return a.ColorValue != b.ColorValue;
    }


    public override int GetHashCode()
    {
        return ColorValue.GetHashCode();
    }
}

