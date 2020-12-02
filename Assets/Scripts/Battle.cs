using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{
    public float allyHealth = 3.0f;
    public float enemyHealth = 3.0f;
    public bool yourTurn = true;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(yourTurn)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                enemyHealth--;
                Debug.Log("Enemy Health: " + enemyHealth);
                yourTurn = false;
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
        else
        {
            allyHealth -= 0.5f;
            Debug.Log("Ally Health: " + allyHealth);
            yourTurn = true;
        }

        if(enemyHealth == 0)
        {
            Debug.Log("You defeated the enemy!");
            SceneManager.LoadScene("SampleScene");
        }
        if(allyHealth == 0)
        {
            Debug.Log("You lost to the enemy, retreat!");
        }
       
    }
}
