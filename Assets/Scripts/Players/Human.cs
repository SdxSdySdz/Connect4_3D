using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour, IPlayer
{
    public Move SelectMove(GameState gameState)
    {
        return Move.EmptyMove;
    }
}
