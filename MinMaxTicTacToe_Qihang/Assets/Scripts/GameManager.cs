using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public enum States
{
    CanMove,
    CantMove
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public BoxCollider2D collider;
    public GameObject token1, token2;
    public int Size = 3;
    public int[,] Matrix;
    [SerializeField] private States state = States.CanMove;
    public Camera camera;
    void Start()
    {
        Instance = this;
        Matrix = new int[Size, Size];
        Calculs.CalculateDistances(collider, Size);
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Matrix[i, j] = 0; // 0: desocupat, 1: fitxa jugador 1, -1: fitxa IA;
            }
        }
    }
    private void Update()
    {
        if (state == States.CanMove)
        {
            Vector3 m = Input.mousePosition;
            m.z = 10f;
            Vector3 mousepos = camera.ScreenToWorldPoint(m);
            if (Input.GetMouseButtonDown(0))
            {
                if (Calculs.CheckIfValidClick((Vector2)mousepos, Matrix))
                {
                    state = States.CantMove;
                    if(Calculs.EvaluateWin(Matrix)==2)
                        StartCoroutine(WaitingABit());
                }
            }
        }
    }

    private IEnumerator WaitingABit()
    {
        yield return new WaitForSeconds(1f);
        AITurn();
    }

    public void AITurn()
    {
        int x, y;
        PlayerAI minimaxAI = new PlayerAI();
        (x,y) = minimaxAI.GetAiMove(Matrix);

        DoMove(x, y, -1);
        state = States.CanMove;

        for(int i = 0; i < Matrix.GetLength(0); i++)
        {
            for (int j = 0; j < Matrix.GetLength(1); j++)
            {
                Debug.Log(Matrix[i, j]);
            }
            Debug.Log("\n");
        }

    }

    public void DoMove(int x, int y, int team)
    {
        Matrix[x, y] = team;
        if (team == 1)
            Instantiate(token1, Calculs.CalculatePoint(x, y), Quaternion.identity);
        else
            Instantiate(token2, Calculs.CalculatePoint(x, y), Quaternion.identity);
        int result = Calculs.EvaluateWin(Matrix);
        switch (result)
        {
            case 0:
                Debug.Log("Draw");
                break;
            case 1:
                Debug.Log("You Win");
                break;
            case -1:
                Debug.Log("You Lose");
                break;
            case 2:
                if(state == States.CantMove)
                    state = States.CanMove;
                break;
        }
    }
}
