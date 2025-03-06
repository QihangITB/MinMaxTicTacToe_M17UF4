using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAI
{
    private const int UserPlayer = 1;
    private const int AIPlayer = -1;
    private const int Empty = 0;

    private int AIWin = 1;
    private int AILose = -1;
    private int Draw = 0;

    public (int, int) GetAiMove(int[,] matrix)
    {
        int alpha = int.MinValue;
        int beta = int.MaxValue;
        int x = 0;
        int y = 0;

        int bestValue = int.MinValue;

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == Empty)
                {
                    matrix[i, j] = AIPlayer;

                    int value = Minimax(matrix, true, alpha, beta);
                    if (value > bestValue)
                    {
                        bestValue = value;
                        x = i;
                        y = j;
                    }
                    matrix[i, j] = Empty;

                    alpha = Mathf.Max(alpha, bestValue);
                    if (beta <= alpha)
                        break; // Poda beta
                }
            }
        }
        return (x,y);
    }

    public int Minimax(int[,] matrix, bool isPlayerMinimizer, int alpha, int beta)
    {
        int result = Calculs.EvaluateWin(matrix);
        if (result == UserPlayer) return AILose; // El jugador ha ganado, valor negativo para la IA
        else if (result == AIPlayer) return AIWin; // La IA ha ganado, valor positivo para la IA
        else if (result == Empty) return Draw; // Empate

        if (isPlayerMinimizer) // Es el turno del jugador (quien minimiza)
        {
            int bestValue = int.MaxValue;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        matrix[i, j] = 1; // El jugador pone su ficha
                        bestValue = Mathf.Min(bestValue, Minimax(matrix, false, alpha, beta));
                        matrix[i, j] = 0; // Deshacemos el movimiento

                        beta = Mathf.Min(beta, bestValue);
                        if (beta <= alpha)
                            break; // Poda beta
                    }
                }
            }
            return bestValue;
        }
        else // Es el turno de la IA (quien maximiza)
        {
            int bestValue = int.MinValue;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        matrix[i, j] = -1; // La IA pone su ficha
                        bestValue = Mathf.Max(bestValue, Minimax(matrix, true, alpha, beta));
                        matrix[i, j] = 0; // Deshacemos el movimiento

                        alpha = Mathf.Max(alpha, bestValue);
                        if (beta <= alpha)
                            break; // Poda alpha
                    }
                }
            }
            return bestValue;
        }
    }
}