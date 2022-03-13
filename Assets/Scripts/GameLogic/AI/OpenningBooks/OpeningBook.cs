using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OpeningBook
{
    private Dictionary<ulong, Move> _book;


    public OpeningBook()
    {
        _book = new Dictionary<ulong, Move>();
    }


    public bool TryGetBestMove(Board board, out Move bestMove) => _book.TryGetValue(board.ZobristHash, out bestMove);
    /*    public bool TryGetBestMove(Board board, PlayerColor playerColor, out Move bestMove)
        {
            if (_book.TryGetValue(board.ZobristHash, out var value))
            {
                return value.TryGetValue(playerColor, out bestMove);
            }
            else
            {
                bestMove = Move.EmptyMove;
                return false;
            }
        }*/


    public void Add(Board board, Move bestMove)
    {
        Add(board.ZobristHash, bestMove);
    }


    private void Add(ulong zobristHash, Move bestMove)
    {
        _book[zobristHash] = bestMove;
    }
    /*    private void Add(ulong zobristHash, PlayerColor playerColor, Move bestMove)
        {
            if (_book.TryGetValue(zobristHash, out var value))
            {
                _book[zobristHash][playerColor] = bestMove;
            }
            else
            {
                _book[zobristHash] = new Dictionary<PlayerColor, Move>();
            }

        }*/
}

