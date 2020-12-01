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
    public GameObject exit;
    public GameObject roomExit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;
    public GameObject populateGrass;
    public GameObject key;
    public GameObject door;
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

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);

                if(x == 3 && y == 3)
                {
                    toInstantiate = populateGrass;
                }
                //if (gameBoard[y, x] == 'X')
                //{
                //    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                //} else if(gameBoard[y, x] == 'I')
                //{
                //    toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
                //} else if (gameBoard[y, x] == 'O')
                //{
                //    toInstantiate = roomExit;
                //} else if (gameBoard[y, x] == 'E')
                //{
                //    toInstantiate = exit;
                //}
                //else if (gameBoard[y, x] == 'F')
                //{
                //    toInstantiate = foodTiles[Random.Range(0, foodTiles.Length)];
                //}
                //else if (gameBoard[y, x] == '1')
                //{
                //    toInstantiate = enemyTiles[0];
                //}
                //else if (gameBoard[y, x] == '2')
                //{
                //    toInstantiate = enemyTiles[1];
                //}
                //else if (gameBoard[y, x] == '3')
                //{
                //    toInstantiate = enemyTiles[2];
                //}
                //else if (gameBoard[y, x] == 'K')
                //{
                //    toInstantiate = key;
                //}
                //else if (gameBoard[y, x] == 'L')
                //{
                //    toInstantiate = door;
                //}


                instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);

            }
        }
    }

    public void SetupScene()
    {
        BoardSetup();
        InitialiseList();
    }
}
