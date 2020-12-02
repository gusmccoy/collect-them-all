using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;

public class BoardManager : MonoBehaviour
{

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public static char[,] gameBoard = new char[36, 36];
    private int columns = 36;
    private int rows = 36;
    public static GameObject[] enemySprites;
    public static GameObject[] allySprites;
    public GameObject populateGrass;
    public GameObject player;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {

        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    // Clears our list gridPositions and prepares it to generate a new board.
    void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
    }

    public void SetupScene()
    {
        BoardSetup();
        InitialiseList();
    }
}
