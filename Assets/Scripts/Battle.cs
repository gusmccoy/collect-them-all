using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{
    public float allyHealth = 3.0f;
    public float enemyHealth = 3.0f;
    public float allyDamage = 1.0f;
    public float enemyDamage = 0.5f;
    public bool yourTurn = true;
    public bool editHealthText = true;
    public Text allyText;
    public Text enemyText;
    public Text fightOverText;
    public AudioSource BattleTheme;
    public AudioSource NewCreatureCapture;

    void Start()
    {
        allyText.text = "HP: " + allyHealth;
        enemyText.text = "HP: " + enemyHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (editHealthText)
        {
            allyText.text = "HP: " + allyHealth;
            enemyText.text = "HP: " + enemyHealth;

            if (yourTurn)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    editHealthText = false;

                    StartCoroutine(DamageEnemy());
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene("SampleScene");
                }
            }
            else
            {
                editHealthText = false;

                StartCoroutine(DamageAlly());
            }



            if (enemyHealth == 0)
            {
                BattleTheme.Stop();
                StartCoroutine(FightOver());
            }
            else if (allyHealth == 0)
            {
                BattleTheme.Stop();
                StartCoroutine(FightOver());
            }
        }
    }

    IEnumerator DamageAlly()
    {
        allyHealth -= enemyDamage;

        if (allyHealth != 0)
        {
            allyText.text = allyText.text + "  -" + enemyDamage;
        }
        else
        {
            allyText.text = "Fainted";
            allyText.color = Color.red;
        }

        Debug.Log("Ally Health: " + allyHealth);
        yourTurn = true;

        if (allyHealth != 0)
        {
            yield return new WaitForSeconds(0.5f);
            editHealthText = true;
        }
        else
        {
            yield return new WaitForSeconds(0);
        }
    }

    IEnumerator DamageEnemy()
    {
        enemyHealth -= allyDamage;

        if (enemyHealth != 0)
        {
            enemyText.text = enemyText.text + "  -" + allyDamage;
        }
        else
        {
            enemyText.text = "Fainted";
            enemyText.color = Color.red;
        }

        Debug.Log("Enemy Health: " + enemyHealth);
        yourTurn = false;

        if (enemyHealth != 0)
        {
            yield return new WaitForSeconds(0.5f);
            editHealthText = true;
        }
        else
        {
            yield return new WaitForSeconds(0);
        }
    }

    IEnumerator FightOver()
    {
        if (allyHealth == 0)
        {
            fightOverText.color = Color.red;
        }
        else
        {
            fightOverText.color = Color.green;
            if (!SaveState.capturedCreatures[SaveState.enemyID])
            {
                NewCreatureCapture.Play();
                SaveState.capturedCreatures[SaveState.enemyID] = true;
                yield return new WaitForSeconds(3);
                SceneManager.LoadScene("CreatureLogScreen");
            }
            else
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }
}