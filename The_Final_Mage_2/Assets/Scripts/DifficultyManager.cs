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
    public float standardMeleeHP;

    /// <summary>
    /// Our base standard ranged health.
    /// </summary>
    public float standardRangedHP;

    /// <summary>
    /// Our base standard melee damage
    /// </summary>
    public float standardMeleeDamage;

    /// <summary>
    /// Our base standard range damage
    /// </summary>
    public float standardRangeDamage;

    /// <summary>
    /// Have we set our stats for the enemies on this floor?
    /// </summary>
    public bool statsSet;

	// Use this for initialization
	void Start () {

        //Start on floor 1.
        floor = 1;
        newFloor = true;
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

        //For every enemy on the field.
        for (int x = 0; x < enemies.Length; x++)
        {
            EnemyHealth enemyHP = enemies[x].GetComponent<EnemyHealth>();
            EnemyAI enemyAI = enemies[x].GetComponent<EnemyAI>();

            //If the current enemy is a melee type
            if (enemies[x].meleeType == true)
            {
                //No additional floor modifiers on health and attack power for the time being.
                enemyHP.maxHealth = standardMeleeHP;
                enemyHP.health = standardMeleeHP;

                enemyAI.meleeDamage = standardMeleeDamage;
            }
            else
            {
                //No additional floor modifiers on health and attack power for the time being.
                enemyHP.maxHealth = standardRangedHP;
                enemyHP.health = standardRangedHP;

                enemyAI.rangeDamage = standardRangeDamage;
            }

        }
        statsSet = true;
    }
}
