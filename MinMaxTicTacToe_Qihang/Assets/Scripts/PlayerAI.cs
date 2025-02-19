using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

public static class PlayerAI
{
    private const int MinDepth = 1;
    private const int UserToken = 1, AIToken = -1, Empty = 0;

    public static void MinMaxWithPruning(int[,] matrix)
    {
        int? posX = null;
        int? posY = null;

        int? value = null;
        int alpha = int.MinValue;
        int beta = int.MaxValue;

        TurnMove moves = new TurnMove(matrix, matrix, AIToken);

    }
}



