using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public LayerMask blockingLayer;

    public float moveTime = 0.1f;
    private BoxCollider2D boxCollider;

    public GameObject Camera;
    public float restartLevelDelay = 1f;
    // public int enemyDamage = 1;
    public Text oakMessageText;
    public GameObject messageImage;
    public AudioSource CityMusic;
    public AudioSource WildernessMusic;

    public float speed = 3.0f;

    [HideInInspector] public static int score;

    private Animator animator;
    private KeyCode direction = KeyCode.None;


    public void Start()
    {
        animator = GetComponent<Animator>();
        messageImage.SetActive(false);
        boxCollider = GetComponent<BoxCollider2D>();

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
            CityMusic.Play();
        else
            WildernessMusic.Play();
    }


    private void Update()
    {
        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

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
            horizontal = vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove(horizontal, vertical);
        }

    }

    private void AttemptMove(int xDir, int yDir)
    {
        RaycastHit2D hit;
        Move(xDir, yDir, out hit);

        if (hit.transform == null)
        {
            var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            transform.position += move * speed * Time.deltaTime;
            Camera.transform.position = transform.position + new Vector3(3.5f, 1f, -10f);
        }
    }

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

    private bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(start, end, blockingLayer);

        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            return true;
        }

        return false;
    }
}