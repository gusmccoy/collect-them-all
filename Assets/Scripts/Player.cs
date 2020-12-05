using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public LayerMask blockingLayer;
    private BoxCollider2D boxCollider;
    public GameObject Camera;
    public Text oakMessageText;
    public GameObject messageImage;
    public AudioSource CityMusic;
    public AudioSource WildernessMusic;
    public AudioSource BeatGameMusic;

    public float speed = 5.0f;
    public int minimumMonsters = 5;
    public int maxMonsters = 10;
    public float enterBattleDelay = 1f;
    public float endGameDelay = 4.0f;
    public float deactivateOakMessageDelay = 3.0f;
    public float moveTime = 0.1f;
    public float cityBorderCoordinateY = 21.0f;
    public int battleChance = 4;
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
            Camera.transform.position = transform.position + new Vector3(3.5f, 1f, -10f);
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
        if (other.tag == "DangerArea")
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            var rand = new System.Random(t.Seconds);
            int chance = rand.Next(battleChance);
            if(chance == 1)
            {
                Invoke("enterBattleScene", enterBattleDelay);
            }
        }
        if (other.tag == "CityGate")
        {
            if(transform.position.y <= cityBorderCoordinateY)
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

            for(int i = 0; i < maxMonsters; i++)
            {
                if (SaveState.capturedCreatures[i])
                    monstersCollected++;
            }

            if(monstersCollected >= minimumMonsters)
            {
                oakMessageText.text = "You collected " + monstersCollected + " monsters. Thank you so much for your contribution! You win!";
                messageImage.SetActive(true);
                CityMusic.Stop();
                BeatGameMusic.Play();
                Invoke("endGame", endGameDelay);
            }
            else
            {
                oakMessageText.text = "Goodness! You seemed to have jumped the gun! You still need " + (minimumMonsters - monstersCollected) + " more monsters before you've completed your task!";
                messageImage.SetActive(true);
                Invoke("deactiveText", deactivateOakMessageDelay);
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
        SaveState.capturedCreatures = new bool[maxMonsters];
        SceneManager.LoadScene("TitleScreen");
    }

    private void enterBattleScene()
    {
        TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
        var rand = new System.Random(t.Seconds);
        SaveState.allyID = 1;
        SaveState.enemyID = rand.Next(maxMonsters);
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

        hit = Physics2D.Linecast(start, end, blockingLayer);

        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            return true;
        }

        return false;
    }
}