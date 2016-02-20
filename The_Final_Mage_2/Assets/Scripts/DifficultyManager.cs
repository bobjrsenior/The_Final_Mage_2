using UnityEngine;
using System.Collections;

public class DifficultyManager : MonoBehaviour {

    /// <summary>
    /// Which floor have we reached?
    /// </summary>
    public int floor;

    /// <summary>
    /// True when we go to a new floor, false otherwise.
    /// </summary>
    public bool newFloor;

    /// <summary>
    /// Our base standard melee health.
    /// </summary>
    public float enemyStandardMeleeHP;

    /// <summary>
    /// Our base standard ranged health.
    /// </summary>
    public float enemyStandardRangedHP;

    /// <summary>
    /// Our base standard melee damage
    /// </summary>
    public float enemyStandardMeleeDamage;

    /// <summary>
    /// Our base standard range damage
    /// </summary>
    public float enemyStandardRangeDamage;

    /// <summary>
    /// Have we set our stats for the enemies on this floor?
    /// </summary>
    public bool statsSet;

    /// <summary>
    /// The players standard health
    /// </summary>
    public float playerStandardHealth;

    /// <summary>
    /// The players standard max health
    /// </summary>
    public float playerStandardMaxHealth;

    /// <summary>
    /// The players standard melee damage.
    /// </summary>
    public float playerStandardMeleeDam;

    /// <summary>
    /// The players standard ranged damage.
    /// </summary>
    public float playerStandardRangedDam;

	// Use this for initialization
	void Start () {

        //Start on floor 1.
        floor = 1;
        newFloor = true;
        setPlayerStats();
	}
	
	// Update is called once per frame
	void Update () {

        if (newFloor == true)
        {
            setEnemyStats();
        }
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
                enemies[x].rangedType = false;
                enemies[x].meleeType = true;
            }
            else if (type == 1)
            {
                enemies[x].meleeType = false;
                enemies[x].rangedType = true;
            }

            //If the current enemy is a melee type
            if (enemies[x].meleeType == true)
            {
                //No additional floor modifiers on health and attack power for the time being.
                enemyHP.maxHealth = enemyStandardMeleeHP;
                enemyHP.health = enemyStandardMeleeHP;

                enemyAI.meleeDamage = enemyStandardMeleeDamage;
            }
            else
            {
                //No additional floor modifiers on health and attack power for the time being.
                enemyHP.maxHealth = enemyStandardRangedHP;
                enemyHP.health = enemyStandardRangedHP;

                enemyAI.rangeDamage = enemyStandardRangeDamage;
            }

        }
        statsSet = true;
    }

    private void setPlayerStats()
    {
        PlayerHealth playerHP = FindObjectOfType<PlayerHealth>();
        PlayerAttack playerAttack = FindObjectOfType<PlayerAttack>();

        playerHP.maxHealth = playerStandardMaxHealth;
        playerHP.health = playerStandardHealth;
        playerAttack.meleeDamage = playerStandardMeleeDam;
        playerAttack.rangeDamage = playerStandardRangedDam;
    }
}
