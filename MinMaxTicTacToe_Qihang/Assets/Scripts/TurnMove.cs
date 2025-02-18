using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class TurnMove
{
    private const int Empty = 0;

    private int[,] previousMatrix;
    private int[,] currentMatrix;
    private List<TurnMove> followingMatrixs = new List<TurnMove>();

    public int[,] PreviousMatrix { get => previousMatrix; set => previousMatrix = value; }
    public int[,] CurrentMatrix { get => currentMatrix; set => currentMatrix = value; }
    public List<TurnMove> FollowingMatrixs { get => followingMatrixs; set => followingMatrixs = value; }

    public TurnMove(int[,] previous, int[,] current, int token)
    {
        previousMatrix = previous;
        currentMatrix = current;
        followingMatrixs = CreateFollowingMovements(currentMatrix, -token);
    }

    private List<TurnMove> CreateFollowingMovements(int[,] matrix, int token)
    {
        List<TurnMove> result = new List<TurnMove>();
        int[,] process = matrix;
        int? row, column;

        do
        {
            // Obtenemos la siguiente posición
            (row, column) = NextTurnPosition(process);

            if (row != null && column != null)
            {
                // Creamos una nueva matriz con el movimiento simulado y lo guardamos
                int[,] newMatrix = matrix;
                newMatrix[(int)row, (int)column] = token;
                result.Add(new TurnMove(matrix, newMatrix, token));

                // Guardamos también el recorrido para no repetir ("bloquear" la casilla)
                process[(int)row, (int)column] = token;
            }
        } while (row != null && column != null);
        
        return result;
    }

    public static (int?,int?) NextTurnPosition(int[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] == Empty)
                {

                    return (i,j);
                }
            }
        }
        return (null, null);
    }
}

