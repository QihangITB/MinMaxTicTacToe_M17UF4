using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
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
    public Canvas resultScreen;
    public GameObject WinTitle;
    public GameObject LoseTitle;
    public GameObject DrawTitle;

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
        resultScreen.gameObject.SetActive(false);
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
        yield return new WaitForSeconds(0.1f);
        AITurn();
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
                StartCoroutine(WaitToShowResult(DrawTitle));
                Debug.Log("Draw");
                break;
            case 1:
                StartCoroutine(WaitToShowResult(WinTitle));
                Debug.Log("You Win");
                break;
            case -1:
                StartCoroutine(WaitToShowResult(LoseTitle));
                Debug.Log("You Lose");
                break;
            case 2:
                if(state == States.CantMove)
                    state = States.CanMove;
                break;
        }
    }

    // MI IMPLEMENTACION:
    private void AITurn()
    {
        int x, y;
        PlayerAI minimaxAI = new PlayerAI();
        (x, y) = minimaxAI.GetAiMove(Matrix);

        DoMove(x, y, -1);
        state = States.CanMove;

        ShowMatrixOnConsole(Matrix);
    }

    private void ShowMatrixOnConsole(int[,] matrix)
    {
        string board = "Tablero:\n";
        for (int j = 0; j < matrix.GetLength(1); j++) // Recorremos columnas primero
        {
            string row = "";
            for (int i = 0; i < matrix.GetLength(0); i++) // Luego recorremos filas
            {
                row += matrix[i, j] + " ";
            }
            board += row + "\n"; // Agregamos la fila transpuesta y un salto de línea
        }
        Debug.Log(board);
    }

    private IEnumerator WaitToShowResult(GameObject resultTitle) 
    {
        resultTitle.SetActive(true);
        yield return new WaitForSeconds(1f);

        if(resultTitle.activeSelf)
            resultScreen.gameObject.SetActive(true);
    }

    public void ReplayGame()
    {
        WinTitle.SetActive(false);
        LoseTitle.SetActive(false);
        DrawTitle.SetActive(false);
        resultScreen.gameObject.SetActive(false);

        RemoveTokenObject();

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Matrix[i, j] = 0; // 0: desocupat, 1: fitxa jugador 1, -1: fitxa IA;
            }
        }

        state = States.CanMove;
    }

    private void RemoveTokenObject()
    {
        List<GameObject> filteredObjects = FindObjectsOfType<GameObject>()
            .Where(obj => obj.name.ToLower().Contains("clone"))
            .ToList();

        foreach (GameObject obj in filteredObjects)
        {
            Destroy(obj);
        }
    }
}
