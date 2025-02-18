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

    public static int[,] GetNextPosition()
    {
        int[,] position = new int[MinDepth, MinDepth];




        return position;
    }

    public static void MinMaxWithPruning(int[,] matrix)
    {
        int? posX = null;
        int? posY = null;

        float? value = null;
        float alpha = float.MinValue;
        float beta = float.MaxValue;

        List<int[,]> matrixList = new List<int[,]>();


    }

    public static int[,] CreateMinMaxTree(int[,] matrix, int user, int ai)
    {
        Dictionary<int[,], List<int[,]>> matrixValues = new Dictionary<int[,], List<int[,]>>();
        List<int[,]> matrixList = new List<int[,]>();

        while (!IsMatrixFull(matrix))
        {
            matrixList.Add(Turn(matrix, AIToken));
        }

        return matrix;
    }

    // Coje el primer espacio que encuentre y coloca el token en caso de jugador o IA
    public static int[,] Turn(int[,] matrix, int token)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0 ; j < matrix.GetLength(1); j++)
            {
                if (matrix[i,j] == Empty)
                {
                    matrix[i, j] = token;
                    return matrix;
                }
            }
        }
        return matrix;
    }

    public static bool IsMatrixFull(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == Empty)
                {
                    return false;
                }
            }
        }
        return true;
    }   
}



