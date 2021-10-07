using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public Move SelectMove(GameState gameState);
}
