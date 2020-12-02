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
    public GameObject floorTile;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;
    public GameObject populateGrass;
    public GameObject player;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    //public void ReadMap()
    //{
    //    string filename = "Assets/Resources/map.txt";

    //    var reader = new StreamReader(filename);
    //    string line;
    //    int row = 0;
    //    while((line = reader.ReadLine()) != null)
    //    {
    //        for(int letter = 0; letter < line.Length; letter++)
    //        {
    //            gameBoard[row, letter] = line[letter];
    //        }
    //        row++;
    //    }
    //}


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
        // Clear our list gridPositions.
        gridPositions.Clear();

        // Loop through x axis (columns).
        for (int x = 1; x < columns - 1; x++)
        {
            // Within each column, loop through y axis (rows).
            for (int y = 1; y < rows - 1; y++)
            {
                // At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        //for (int x = 0; x < columns; x++)
        //{
        //    for (int y = 0; y < rows; y++)
        //    {
        //        GameObject toInstantiate = floorTile;
        //        GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
        //        instance.transform.SetParent(boardHolder);

        //    }
        //}
    }

    public void SetupScene()
    {
        BoardSetup();
        InitialiseList();
    }
}
