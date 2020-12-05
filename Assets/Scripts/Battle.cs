using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{
    public bool yourTurn = true;
    public bool editHealthText = true;
    public Text allyText;
    public Text enemyText;
    public Text fightOverText;
    public AudioSource BattleTheme;
    public AudioSource NewCreatureCapture;
    public AudioSource DefeatedMusic;
    public Button fightBtn;
    public Button runBtn;
    public Slider allySlider;
    public Slider enemySlider;

    private float allyHealth = 3.0f;
    private float allyMaxHealth;
    private float allyDamage = 1.0f;
    private float enemyHealth;
    private float enemyMaxHealth;
    private float enemyDamage;
    private int monstersCollected;
    private bool gotToLog = false;
    private float allyHealthProgressNumber;
    private float enemyHealthProgressNumber;

    void Start()
    {
        fightBtn.onClick.AddListener(() => AttackEnemy());
        runBtn.onClick.AddListener(() => RunFromBattle());

        allyHealthProgressNumber = 1.0f;
        enemyHealthProgressNumber = 1.0f;
        allySlider.value = allyHealthProgressNumber;
        enemySlider.value = enemyHealthProgressNumber;

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
        allyMaxHealth = allyHealth;

        if (SaveState.enemyID == 0)
        {
            enemyHealth = 8.0f;
            enemyDamage = 4.0f;
        }
        else if (SaveState.enemyID == 1)
        {
            enemyHealth = 10.0f;
            enemyDamage = 5.0f;
        }
        else if (SaveState.enemyID == 2)
        {
            enemyHealth = 2.0f;
            enemyDamage = 1.0f;
        }
        else if (SaveState.enemyID == 3)
        {
            enemyHealth = 6.0f;
            enemyDamage = 2.0f;
        }
        else if (SaveState.enemyID == 4)
        {
            enemyHealth = 4.0f;
            enemyDamage = 3.0f;
        }
        else if (SaveState.enemyID == 5)
        {
            enemyHealth = 3.0f;
            enemyDamage = 1.5f;
        }
        else if (SaveState.enemyID == 6)
        {
            enemyHealth = 3.0f;
            enemyDamage = 2.0f;
        }
        else if (SaveState.enemyID == 7)
        {
            enemyHealth = 12.0f;
            enemyDamage = 5.5f;
        }
        else if (SaveState.enemyID == 8)
        {
            enemyHealth = 5.0f;
            enemyDamage = 1.0f;
        }
        else
        {
            enemyHealth = 1.0f;
            enemyDamage = 0.5f;
        }

        enemyMaxHealth = enemyHealth;

        allyText.text = "HP: " + allyHealth;
        enemyText.text = "HP: " + enemyHealth;
    }

    void Update()
    {
        if (editHealthText)
        {
            allyText.text = "HP: " + allyHealth;
            enemyText.text = "HP: " + enemyHealth;

            if (!yourTurn && enemyHealth > 0)
            {
                editHealthText = false;
                StartCoroutine(DamageAlly());
            }

            if (enemyHealth <= 0)
            {
                BattleTheme.Stop();
                fightBtn.onClick.RemoveAllListeners();
                runBtn.onClick.RemoveAllListeners();
                StartCoroutine(FightOver());
            }
            else if(allyHealth <= 0)
            {
                BattleTheme.Stop();
                fightBtn.onClick.RemoveAllListeners();
                runBtn.onClick.RemoveAllListeners();
                StartCoroutine(FightOver());
            }
        }
    }

    IEnumerator DamageAlly()
    {
        allyHealth -= enemyDamage;
        allyHealthProgressNumber = allyHealth / allyMaxHealth;

        if (allyHealthProgressNumber >= 0)
        {
            allySlider.value = allyHealthProgressNumber;
        }
        else
        {
            allySlider.value = 0f;
        }

        if (allyHealth > 0)
        {
            allyText.text = allyText.text + "  -" + enemyDamage;
        }
        else
        {
            allyText.text = "Fainted";
            allyText.color = Color.red;
        }

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
        enemyHealthProgressNumber = enemyHealth / enemyMaxHealth;

        if (enemyHealthProgressNumber >= 0)
        {
            enemySlider.value = enemyHealthProgressNumber;
        }
        else
        {
            enemySlider.value = 0f;
        }

        if (enemyHealth > 0)
        {
            enemyText.text = enemyText.text + "  -" + allyDamage;
        }
        else
        {
            enemyText.text = "Fainted";
            enemyText.color = Color.red;
        }

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
        if (enemyHealth <= 0)
        {
            fightOverText.color = Color.green;
            fightOverText.text = "You defeated the enemy!";

            if (!SaveState.capturedCreatures[SaveState.enemyID])
            {
                NewCreatureCapture.Play();
                SaveState.capturedCreatures[SaveState.enemyID] = true;
                gotToLog = true;
                yield return new WaitForSeconds(3);
            }
            else
            {
                gotToLog = false;
                SceneManager.LoadScene("MainScene");
            }
            if (gotToLog)
            {
                SceneManager.LoadScene("CreatureLogScreen");
            }
        }
        else
        {
            fightOverText.color = Color.red;
            DefeatedMusic.Play();
            gotToLog = false;
            SaveState.allyID = 1;
            SaveState.playerCoordinateX = 0;
            SaveState.playerCoordinateY = 0;
            SaveState.inTown = true;
            SaveState.capturedCreatures = new bool[10];
            yield return new WaitForSeconds(3);
            fightOverText.text = "You lost the fight, retreat!";
            SceneManager.LoadScene("TitleScreen");
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
            SceneManager.LoadScene("MainScene");
        }
    }
}