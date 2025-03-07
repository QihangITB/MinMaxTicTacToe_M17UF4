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
        int bestValue = int.MinValue;
        int bestX = -1;
        int bestY = -1;
        int alpha = int.MinValue;
        int beta = int.MaxValue;

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
                        bestX = i;
                        bestY = j;
                    }
                    matrix[i, j] = Empty;

                    alpha = Mathf.Max(alpha, bestValue);
                    if (beta <= alpha)
                        break; // Poda beta
                }
            }
        }
        return (bestX, bestY);
    }

    private int Minimax(int[,] matrix, bool isPlayerTurn, int alpha, int beta)
    {
        switch (Calculs.EvaluateWin(matrix))
        {
            case UserPlayer:
                return AILose; // Pierde el AI (Gana el jugador)
            case AIPlayer:
                return AIWin; // Gana el AI (Pierde el jugador)
            case Empty:
                return Draw; // Empate

        }

        // Assignamos los valores infinitos dependiendo del turno:
        // +infinito para el jugador
        // -infinito para la AI
        int bestValue = isPlayerTurn ? int.MaxValue : int.MinValue;

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == Empty)
                {
                    matrix[i, j] = isPlayerTurn ? UserPlayer : AIPlayer;
                    int value = Minimax(matrix, !isPlayerTurn, alpha, beta);
                    matrix[i, j] = Empty;

                    if (isPlayerTurn)
                    {
                        // Si es turno del jugador, utilizaremos Minimo porque queremos el menor valor posible
                        bestValue = Mathf.Min(bestValue, value);
                        beta = Mathf.Min(beta, bestValue);
                        if (beta <= alpha)
                            break; // Podamos
                    }
                    else
                    {
                        // Si es turno del AI, utilizaremos Maximo porque queremos el mayor valor posible
                        bestValue = Mathf.Max(bestValue, value);
                        alpha = Mathf.Max(alpha, bestValue);
                        if (beta <= alpha)
                            break; // Podamos
                    }
                }
            }
        }
        return bestValue;
    }
}