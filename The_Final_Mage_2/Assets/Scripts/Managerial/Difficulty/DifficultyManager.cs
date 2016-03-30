﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour {

    /// <summary>
    /// Which floor have we reached?
    /// </summary>
    public int floor = 1;

    /// <summary>
    /// How fast do the enemies move?
    /// </summary>
    public float enemyStandardMeleeSpeed;

    public float enemyHardMeleeSpeed;

    /// <summary>
    /// How fast do ranged enemies move?
    /// </summary>
    public float enemyStandardRangedSpeed;

    public float enemyHardRangedSpeed;

    /// <summary>
    /// True when we go to a new floor, false otherwise.
    /// </summary>
    public bool newFloor;

    /// <summary>
    /// Our base standard melee health.
    /// </summary>
    public float enemyStandardMeleeHP;

    public float enemyHardMeleeHP;

    /// <summary>
    /// Our base standard ranged health.
    /// </summary>
    public float enemyStandardRangedHP;

    public float enemyHardRangedHP;

    /// <summary>
    /// Our base standard melee damage
    /// </summary>
    public float enemyStandardMeleeDamage;

    public float enemyHardMeleeDamage;

    /// <summary>
    /// Our base standard range damage
    /// </summary>
    public float enemyStandardRangeDamage;

    public float enemyHardRangeDamage;

    /// <summary>
    /// Have we set our stats for the enemies on this floor?
    /// </summary>
    public bool statsSet = false;

    public bool pStatsSet;

    /// <summary>
    /// The players standard health
    /// </summary>
    public float playerStandardHealth = 5.0f;

    /// <summary>
    /// The players standard max health
    /// </summary>
    public float playerStandardMaxHealth = 5.0f;

    /// <summary>
    /// The players standard melee damage.
    /// </summary>
    public float playerStandardMeleeDam = 0.5f;

    /// <summary>
    /// The players standard ranged damage.
    /// </summary>
    public float playerStandardRangedDam = 1.0f;

    /// <summary>
    /// Melee sprite
    /// </summary>
    public Sprite meleeType;

    /// <summary>
    /// Range sprite
    /// </summary>
    public Sprite rangeType;

    /// <summary>
    /// The speed of an enemy ranged attack.
    /// </summary>
    public float enemyRangeSpeed;

    /// <summary>
    /// How long is the delay between enemy ranged attacks?
    /// </summary>
    public float enemyShootWaitTime;

    /// <summary>
    /// The ranged projectile prefab enemies shoot.
    /// </summary>
    public GameObject enemyRangeProjectile;

    /// <summary>
    /// Whether or not the player has the keycard
    /// </summary>
    public bool gotKeyCard = false;

    /// <summary>
    /// The index the level is on
    /// </summary>
    private int buildIndex;

    public static DifficultyManager dManager;

    void OnGUI()
    {
        //If the player exists.
        if (PlayerHealth.pHealth != null)
        {
            GUI.color = Color.yellow;
            GUI.Box(new Rect(0, 80, 100, 25), "Floor: " + floor);
        }
    }

    void Start()
    {
        if(dManager != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            dManager = this;
            DontDestroyOnLoad(this.gameObject);

            //Start on floor 1
            floor = 1;
            buildIndex = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(setUpFloor());
        }
        
    }


    void OnLevelWasLoaded(int level)
    {
        
        if (dManager != null && dManager.Equals(this) && buildIndex == SceneManager.GetActiveScene().buildIndex)
        {
            StartCoroutine(setUpFloor());
        }

    }

    // Update is called once per frame
    void Update () {


	}

    /// <summary>
    /// Setup the current floor
    /// </summary>
    private IEnumerator setUpFloor()
    {
        //If we've gotten to the end...
        if (floor == 7)
        {
            Destroy(Experience.playerExperience.transform.root.gameObject);
            Destroy(Skills.pSkills.transform.root.gameObject);
            Destroy(PlayerHealth.pHealth.transform.root.gameObject);
            Destroy(TextBoxScript.textScript.transform.root.gameObject);
            SceneManager.LoadScene("Victory");
        }
        if (floor == 1)
        {
            //Ensure that our score always starts at the default score on floor 1.
            Scoring.scoreKeeper.score = Scoring.scoreKeeper.initialScore;
        }
        gotKeyCard = false;
        if (SceneManager.GetActiveScene().name.Equals("LevelGen"))
        {
            LevelGen.gen.generateLevel();
        }
        while (PlayerHealth.pHealth == null || PlayerAttack.pAttack == null) { yield return null; }
        setPlayerStats();
        setEnemyStats();
    }

    /// <summary>
    /// Called when a floor has been completed
    /// </summary>
    public void wonFloor()
    {
        if (gotKeyCard)
        {
            ++floor;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }

    /// <summary>
    /// Called when you have retrieved a keycard
    /// </summary>
    public void retrievedKeyCard()
    {
        gotKeyCard = true;
    }

    /// <summary>
    /// Sets the enemy stats to the appropriate values.
    /// </summary>
    private void setEnemyStats()
    {
        newFloor = false;
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        //For every enemy in the gamespace
        for (int x = 0; x < enemies.Length; x++)
        {
            EnemyHealth enemyHP = enemies[x].GetComponent<EnemyHealth>();
            EnemyAI enemyAI = enemies[x].GetComponent<EnemyAI>();
            
            //Randomly generates enemy type.
            int type = Random.Range(0, 2);
            if (type == 0)
            {
                //Destroy the collider for melee enemies
                Destroy(enemies[x].GetComponent<CircleCollider2D>());
                enemies[x].rangedType = false;
                enemies[x].meleeType = true;
                enemies[x].GetComponent<SpriteRenderer>().sprite = meleeType;
            }
            else if (type == 1)
            {
                //Destroy the collider for ranged enemies.
                Destroy(enemies[x].GetComponent<BoxCollider2D>());
                enemies[x].meleeType = false;
                enemies[x].rangedType = true;
                enemies[x].GetComponent<SpriteRenderer>().sprite = rangeType;
            }

            //If the current enemy is a melee type
            if (enemies[x].meleeType == true)
            {
                //Floor 1 - 2
                if (floor <= 2)
                {
                    enemyHP.maxHealth = enemyHPDifficulty(enemies[x]) - 2;
                    enemyHP.health = enemyHP.maxHealth;
                    enemyAI.enemySpeed = enemySpeedDifficulty(enemies[x]);
                    enemyAI.meleeDamage = enemyDamageDifficulty(enemies[x]);
                }
                //floor 3 - 4
                else if (floor > 2 && floor <= 4)
                {
                    enemyHP.maxHealth = enemyHPDifficulty(enemies[x]) - 1;
                    enemyHP.health = enemyHP.maxHealth;
                    enemyAI.enemySpeed = enemySpeedDifficulty(enemies[x]) + .1f;
                    enemyAI.meleeDamage = enemyDamageDifficulty(enemies[x]);
                }
                else
                {
                    enemyHP.maxHealth = enemyHPDifficulty(enemies[x]);
                    enemyHP.health = enemyHP.maxHealth;
                    enemyAI.enemySpeed = enemySpeedDifficulty(enemies[x]) + .15f;
                    enemyAI.meleeDamage = enemyDamageDifficulty(enemies[x]);
                }
            }
            else
            {
                //Floor 1 - 2
                if (floor <= 2)
                {
                    enemyHP.maxHealth = enemyHPDifficulty(enemies[x]) - 2;
                    enemyHP.health = enemyHP.maxHealth;
                    enemyAI.enemySpeed = enemySpeedDifficulty(enemies[x]);
                    enemyAI.rangeDamage = enemyDamageDifficulty(enemies[x]);
                    enemyAI.rangeSpeed = enemyRangeSpeed;
                    enemyAI.shootWaitTime = enemyShootWaitTime;
                    enemyAI.rangeProjectile = enemyRangeProjectile;
                }
                //Floor 3 - 4
                else if (floor > 2 && floor <= 4)
                {
                    enemyHP.maxHealth = enemyHPDifficulty(enemies[x]) - 1;
                    enemyHP.health = enemyHP.maxHealth;
                    enemyAI.enemySpeed = enemySpeedDifficulty(enemies[x]) + .1f;
                    enemyAI.rangeDamage = enemyDamageDifficulty(enemies[x]);
                    enemyAI.rangeSpeed = enemyRangeSpeed;
                    enemyAI.shootWaitTime = enemyShootWaitTime;
                    enemyAI.rangeProjectile = enemyRangeProjectile;
                }
                //Floor 5 - 6
                else
                {
                    enemyHP.maxHealth = enemyHPDifficulty(enemies[x]);
                    enemyHP.health = enemyHP.maxHealth;
                    enemyAI.enemySpeed = enemySpeedDifficulty(enemies[x]) + .15f;
                    enemyAI.rangeDamage = enemyDamageDifficulty(enemies[x]);
                    enemyAI.rangeSpeed = enemyRangeSpeed;
                    enemyAI.shootWaitTime = enemyShootWaitTime;
                    enemyAI.rangeProjectile = enemyRangeProjectile;
                }
            }

        }
        statsSet = true;
    }

    private void setPlayerStats()
    {
        if (floor == 1)
        {
            PlayerHealth.pHealth.maxHealth = playerStandardMaxHealth;
            PlayerHealth.pHealth.health = playerStandardHealth;
        }
        PlayerAttack.pAttack.meleeDamage = playerStandardMeleeDam;
        PlayerAttack.pAttack.rangeDamage = playerStandardRangedDam;
        pStatsSet = true;
    }

    /// <summary>
    /// Gets the HP value based on enemy type and difficulty
    /// </summary>
    /// <returns></returns>
    private float enemyHPDifficulty(Enemy enemy)
    {
        if (enemy.rangedType == true)
        {
            if (DifficultyTracker.difficultyTrack.getDifficulty() == 1)
            {
                return enemyStandardRangedHP;
            }
            else return enemyHardRangedHP;
        }
        else if (enemy.meleeType == true)
        {
            if (DifficultyTracker.difficultyTrack.getDifficulty() == 1)
            {
                return enemyStandardMeleeHP;
            }
            else return enemyHardMeleeHP;
        }
        else return 1; //Should never happen.
    }

    /// <summary>
    /// Gets the attack value based on enemy type and difficulty
    /// </summary>
    /// <returns></returns>
    private float enemyDamageDifficulty(Enemy enemy)
    {
        if (enemy.rangedType == true)
        {
            if (DifficultyTracker.difficultyTrack.getDifficulty() == 1)
            {
                return enemyStandardRangeDamage;
            }
            else return enemyHardRangeDamage;
        }
        else if (enemy.meleeType == true)
        {
            if (DifficultyTracker.difficultyTrack.getDifficulty() == 1)
            {
                return enemyStandardMeleeDamage;
            }
            else return enemyHardMeleeDamage;
        }
        else return 1; //Should never happen.
    }

    private float enemySpeedDifficulty(Enemy enemy)
    {
        if (enemy.rangedType == true)
        {
            if (DifficultyTracker.difficultyTrack.getDifficulty() == 1)
            {
                return enemyStandardRangedSpeed;
            }
            else return enemyHardRangedSpeed;
        }
        else if (enemy.meleeType == true)
        {
            if (DifficultyTracker.difficultyTrack.getDifficulty() == 1)
            {
                return enemyStandardMeleeSpeed;
            }
            else return enemyHardMeleeSpeed;
        }
        else return 1; //Should never happen.
    }
}