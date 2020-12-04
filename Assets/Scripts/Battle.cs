using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{
    public float enemyHealth = 3.0f;
    public float enemyDamage = 0.5f;
    public bool yourTurn = true;
    public bool editHealthText = true;
    public Text allyText;
    public Text enemyText;
    public Text fightOverText;
    public AudioSource BattleTheme;
    public AudioSource NewCreatureCapture;
    public Button fightBtn;
    public Button runBtn;

    private float allyHealth = 3.0f;
    private float allyDamage = 1.0f;
    private int monstersCollected;

    void Start()
    {
        fightBtn.onClick.AddListener(() => AttackEnemy());
        runBtn.onClick.AddListener(() => RunFromBattle());

        for (int x = 0; x < 10; x++)
        {
            if (SaveState.capturedCreatures[x])
            {
                monstersCollected++;
            }
        }

        float bonusDamage = monstersCollected * 0.5f;
        float bonusHealth = monstersCollected;

        allyDamage += bonusDamage;
        allyHealth += bonusHealth;

        allyText.text = "HP: " + allyHealth;
        enemyText.text = "HP: " + enemyHealth;
    }

    void Update()
    {
        if (editHealthText)
        {
            allyText.text = "HP: " + allyHealth;
            enemyText.text = "HP: " + enemyHealth;

            if (!yourTurn)
            {
                editHealthText = false;
                StartCoroutine(DamageAlly());
            }

            if (enemyHealth <= 0)
            {
                BattleTheme.Stop();
                StartCoroutine(FightOver());
            }
            else if (allyHealth <= 0)
            {
                BattleTheme.Stop();
                StartCoroutine(FightOver());
            }
        }
    }

    IEnumerator DamageAlly()
    {
        allyHealth -= enemyDamage;

        if (allyHealth > 0)
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

        if (allyHealth > 0)
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

        if (enemyHealth > 0)
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

        if (enemyHealth > 0)
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
        if (allyHealth <= 0)
        {
            fightOverText.color = Color.red;
            fightOverText.text = "You lost the fight, reatreat!";
        }
        else
        {
            fightOverText.color = Color.green;
            fightOverText.text = "You defeated the enemy!";

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

    public void AttackEnemy()
    {
        if (yourTurn)
        {
            editHealthText = false;
            StartCoroutine(DamageEnemy());
        }
        if (enemyHealth <= 0)
        {
            BattleTheme.Stop();
            StartCoroutine(FightOver());
        }
        else if (allyHealth <= 0)
        {
            BattleTheme.Stop();
            StartCoroutine(FightOver());
        }
    }

    public void RunFromBattle()
    {
        if (yourTurn)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}