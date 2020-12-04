using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public static GameManager instance = null;

    private BoardManager boardScript;
    private bool doingSetup = true;



    //Awake is always called before any Start functions
    void Awake()
    {
        // Singleton logic
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();

        //Call the SetupScene function of the BoardManager script, pass it current level number.
        boardScript.SetupScene();
    }

    //This is called each time a scene is loaded.
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        InitGame();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    //Initializes the game for each level.
    void InitGame()
    {
        //While doingSetup is true the player can't move, prevent player from moving while title card is up.
        doingSetup = true;
        doingSetup = false;

    }


}