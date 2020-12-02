using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;        //Allows us to use SceneManager
using System;
using UnityEngine.UI;

//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
    public GameObject Camera;
    public float restartLevelDelay = 1f;
    public Text foodText;
    public Text scoreText;
    public AudioClip moveSound1;

    public CreatureLog log;

    [HideInInspector] public static int score;

    private Animator animator;
    private int food;


    //Start overrides the Start function of MovingObject
    protected override void Start()
    {
        //Get a component reference to the Player's animator component
        animator = GetComponent<Animator>();

        log = new CreatureLog();

        //Get the current food point total stored in GameManager.instance between levels.
        //food = GameManager.instance.playerFoodPoints;
        //foodText.text = "Food: " + food;

        //score = 0;
        //scoreText.text = "Score: " + score;

        //Call the Start function of the MovingObject base class.
        base.Start();
    }


    //This function is called when the behaviour becomes disabled or inactive.
    private void OnDisable()
    {
        //When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
        // GameManager.instance.playerFoodPoints = food;
    }


    private void Update()
    {
        Camera.transform.position = transform.position + new Vector3(3.5f, 1f, -10f);

        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;      //Used to store the horizontal move direction.
        int vertical = 0;        //Used to store the vertical move direction.

        //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
        horizontal = (int) Input.GetAxisRaw("Horizontal");

        //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
        vertical = (int) Input.GetAxisRaw("Vertical");

        //Check if moving horizontally, if so set vertical to zero.
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetTrigger("playerWalkLeft");
            vertical = 0;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            animator.SetTrigger("playerWalkRight");
            vertical = 0;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            animator.SetTrigger("playerWalkForward");
            horizontal = 0;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            animator.SetTrigger("playerWalkBackward");
            horizontal = 0;
        }

        //Check if we have a non-zero value for horizontal or vertical
        if (horizontal != 0 || vertical != 0)
        {
            //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
            //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    //AttemptMove overrides the AttemptMove function in the base class MovingObject
    //AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //Every time player moves, subtract from food points total.
        food--;

        //foodText.text = "Food: " + food;
        //scoreText.text = "Score: " + score;

        //Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
        base.AttemptMove<T>(xDir, yDir);

        //Hit allows us to reference the result of the Linecast done in Move.
        RaycastHit2D hit;

        //If Move returns true, meaning Player was able to move into an empty space.
        if (Move(xDir, yDir, out hit))
        {
        }

        //Since the player has moved and lost food points, check if the game has ended.
        CheckIfGameOver();

        //Set the playersTurn boolean of GameManager to false now that players turn is over.
        GameManager.instance.playersTurn = false;
    }


    //OnCantMove overrides the abstract function OnCantMove in MovingObject.
    //It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
    protected override void OnCantMove<T>(T component)
    {
        //Set hitWall to equal the component passed in as a parameter.
        Wall hitWall = component as Wall;

    }

    // OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit")
        {
            //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
            Invoke("endGame", restartLevelDelay);
        }
    }


    private void endGame()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void LoseFood(int loss)
    {
        //Set the trigger for the player animator to transition to the playerHit animation.
        animator.SetTrigger("playerHit");

        //Subtract lost food points from the players total.
        food -= loss;

        //foodText.text = "-" + loss + " Food: " + food;

        //Check to see if game has ended.
        CheckIfGameOver();
    }

    //CheckIfGameOver checks if the player is out of food points and if so, ends the game.
    private void CheckIfGameOver()
    {
        // Game is over when no more food
        if (food <= 0)
        {
            //SoundManager.instance.PlaySingle(gameOverSound);
            //SoundManager.instance.musicSource.Stop();
            //GameManager.instance.GameOver();
        }
    }
}