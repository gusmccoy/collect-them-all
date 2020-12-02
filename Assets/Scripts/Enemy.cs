using UnityEngine;
using System.Collections;
using System;

//Enemy inherits from MovingObject, our base class for objects that can move, Player also inherits from this.
public class Enemy : MovingObject
{
    public int playerDamage;                             //The amount of food points to subtract from the player when attacking.
    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;
    public AudioClip chopSound1;
    public AudioClip chopSound2;
    public int hp = 3;

    private Animator animator;                            //Variable of type Animator to store a reference to the enemy's Animator component.
    private Transform target;                            //Transform to attempt to move toward each turn.
    private bool skipMove;                                //Boolean to determine whether or not enemy should skip a turn or move this turn.

    //Start overrides the virtual Start function of the base class.
    protected override void Start()
    {
        //Register this enemy with our instance of GameManager by adding it to a list of Enemy objects. 
        //This allows the GameManager to issue movement commands.
        GameManager.instance.AddEnemyToList(this);

        //Get and store a reference to the attached Animator component.
        animator = GetComponent<Animator>();

        //Find the Player GameObject using it's tag and store a reference to its transform component.
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //Call the start function of our base class MovingObject.
        base.Start();
    }


    //Override the AttemptMove function of MovingObject to include functionality needed for Enemy to skip turns.
    //See comments in MovingObject for more on how base AttemptMove function works.
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        // Allow enemy to only move every-other turn
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        //Call the AttemptMove function from MovingObject.
        base.AttemptMove<T>(xDir, yDir);

        //Now that Enemy has moved, set skipMove to true to skip next move.
        if(this.tag != "EnemyAdv")
        {
            skipMove = true;
        }
        
    }


    //MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
    public void MoveEnemy()
    {
        //Declare variables for X and Y axis move directions, these range from -1 to 1.
        //These values allow us to choose between the cardinal directions: up, down, left and right.
        int xDir = 0;
        int yDir = 0;

        // Is enemy and player in the same column?
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon && isTargetInSameRoom())
        {
            // Move up or down
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        // Is enemy and player in the same row?
        else if (isTargetInSameRoom())
        {
            // Mmove right or left
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        // Enemy is moving and expecting to potentially encounter a Player
        AttemptMove<Player>(xDir, yDir);
    }


    //OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
    //and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
    protected override void OnCantMove<T>(T component)
    {
        //Declare hitPlayer and set it to equal the encountered component.
        Player hitPlayer = component as Player;

        //Call the LoseFood function of hitPlayer passing it playerDamage, the amount of foodpoints to be subtracted.
        hitPlayer.LoseFood(playerDamage);

        //Set the attack trigger of animator to trigger Enemy attack animation.
        animator.SetTrigger("enemyAttack");

        //SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
    }

    // Function has issues and I'm not sure exactly what the issue is
    private bool isTargetInSameRoom()
    {
        //int targetColumn = (int)Math.Round(target.position.y);
        //int selfColumn = (int)Math.Round(transform.position.y);
        //int targetRow, selfRow;
        //for (targetRow = (int)Math.Round(target.position.x); BoardManager.gameBoard[targetRow, targetColumn] != 'X' &&
        //    BoardManager.gameBoard[targetRow, targetColumn] != 'O' && BoardManager.gameBoard[targetRow, targetColumn] != '0'; targetRow++);
        //for (selfRow = (int)Math.Round(transform.position.x); BoardManager.gameBoard[selfRow, selfColumn] != 'X' &&
        //     BoardManager.gameBoard[selfRow, selfColumn] != 'O' && BoardManager.gameBoard[selfRow, selfColumn] != '0'; selfRow++) ;

        //return targetRow == selfRow;
        return true;
    }
    public void DamageEnemy(int loss)
    {
        //Call the RandomizeSfx function of SoundManager to play one of two chop sounds.
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);

        //Subtract loss from hit point total.
        hp -= loss;

        //If hit points are less than or equal to zero:
        if (hp <= 0)
        {
            //Disable the gameObject.
            gameObject.SetActive(false);
        }
    }
}
