using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct PlayerColor
{
    public int ColorValue { get; private set; }
    public static PlayerColor White => new PlayerColor(1);
    public static PlayerColor Black => new PlayerColor(-1);
    public static PlayerColor Undefined => new PlayerColor(0);
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
            else if (IsBlack)
            {
                return White;
            }
            else
            {
                throw new Exception("Opposite of Undefined stone color");
            }
        }
    }

    private PlayerColor(int colorValue)
    {
        ColorValue = colorValue;
    }

    public override int GetHashCode()
    {
        return ColorValue.GetHashCode();
    }
}

