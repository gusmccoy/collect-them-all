using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
{
    public GameObject Camera;
    public float restartLevelDelay = 1f;
    public int enemyDamage = 1;
    public Text oakMessageText;
    public GameObject messageImage;
    public AudioSource CityMusic;
    public AudioSource WildernessMusic;

    [HideInInspector] public static int score;

    private Animator animator;
    private KeyCode direction = KeyCode.None;


    //Start overrides the Start function of MovingObject
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        messageImage.SetActive(false);

        if (SaveState.playerCoordinateX == 0.0f && SaveState.playerCoordinateY == 0.0f)
        {
            for (int i = 0; i < 10; i++)
            {
                SaveState.capturedCreatures[i] = false;
            }
        }
        else
        {
            transform.position = new Vector2(SaveState.playerCoordinateX, SaveState.playerCoordinateY);
        }

        if(SaveState.inTown)
        {
            CityMusic.Play();
        }
        else
        {
            WildernessMusic.Play();
        }

        //Call the Start function of the MovingObject base class.
        base.Start();
    }


    private void Update()
    {
        Camera.transform.position = transform.position + new Vector3(3.5f, 1f, -10f);

        if (!GameManager.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int) Input.GetAxisRaw("Vertical");

        if (direction != KeyCode.LeftArrow && Input.GetKey(KeyCode.LeftArrow))
        {
            direction = KeyCode.LeftArrow;
            animator.SetTrigger("playerWalkLeft");
            vertical = 0;
        }
        else if (direction != KeyCode.RightArrow && Input.GetKey(KeyCode.RightArrow))
        {
            direction = KeyCode.RightArrow;
            animator.SetTrigger("playerWalkRight");
            vertical = 0;
        }
        else if (direction != KeyCode.DownArrow && Input.GetKey(KeyCode.DownArrow))
        {
            direction = KeyCode.DownArrow;
            animator.SetTrigger("playerWalkForward");
            horizontal = 0;
        }
        else if (direction != KeyCode.UpArrow && Input.GetKey(KeyCode.UpArrow))
        {
            direction = KeyCode.UpArrow;
            animator.SetTrigger("playerWalkBackward");
            horizontal = 0;
        }
        else
        {
            direction = KeyCode.None;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Enemy>(horizontal, vertical);
        }
    }

    //AttemptMove overrides the AttemptMove function in the base class MovingObject
    //AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //Every time player moves, subtract from food points total.

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
        Enemy hitEnemy = component as Enemy;

        hitEnemy.DamageEnemy(enemyDamage);

        if (hitEnemy.hp == 0)
        {
            // Add function that catches the enemy
        }
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
        if (other.tag == "DangerArea")
        {
            var rand = new System.Random();
            int chance = rand.Next(4);
            // ONE IN FOUR CHANCE OF A BATTLE
            if(chance == 1)
            {
                Invoke("enterBattleScene", restartLevelDelay);
            }
        }
        if (other.tag == "CityGate")
        {
            if(transform.position.y <= 21.0f)
            {
                WildernessMusic.Stop();
                CityMusic.Play();
            }
            else
            {
                CityMusic.Stop();
                WildernessMusic.Play();
            }
        }
        if( other.tag == "Oak")
        {
            int monstersCollected = 0;

            for(int i = 0; i < 10; i++)
            {
                if (SaveState.capturedCreatures[i])
                    monstersCollected++;
            }

            if(monstersCollected >= 5)
            {
                oakMessageText.text = "You collected " + monstersCollected + " monsters. Thank you so much for your contribution! You win!";
                messageImage.SetActive(true);
                Invoke("endGame", 3.0f);
            }
            else
            {
                oakMessageText.text = "Goodness! You seemed to have jumped the gun! You still need " + (5 - monstersCollected) + " more monsters before you've completed your task!";
                messageImage.SetActive(true);
                Invoke("deactiveText", 3.0f);
            }
        }
    }

    private void deactiveText()
    {
        oakMessageText.text = "";
        messageImage.SetActive(false);
    }


    private void endGame()
    {
        SaveState.allyID = 1;
        SaveState.playerCoordinateX = 0;
        SaveState.playerCoordinateY = 0;
        SaveState.inTown = true;
        SaveState.capturedCreatures = new bool[10];
        SceneManager.LoadScene("TitleScreen");
    }

    private void enterBattleScene()
    {
        var rand = new System.Random();
        SaveState.allyID = 1;
        SaveState.enemyID = rand.Next(10);
        SaveState.playerCoordinateX = transform.position.x;
        SaveState.playerCoordinateY = transform.position.y;
        SaveState.inTown = false;
        SceneManager.LoadScene("BattleScene");
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

        //foodText.text = "-" + loss + " Food: " + food;

        //Check to see if game has ended.
        CheckIfGameOver();
    }

    //CheckIfGameOver checks if the player is out of food points and if so, ends the game.
    private void CheckIfGameOver()
    {
        // Game is over when no more food
        
    }
}