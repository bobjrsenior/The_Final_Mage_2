using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    /// <summary>
    /// The value that holds our enemy health.
    /// </summary>
    public float health;

    /// <summary>
    /// The maximum health that we could attain.
    /// </summary>
    public float maxHealth;

    /// <summary>
    /// A boolean that corresponds to whether the enemy is dead or not.
    /// </summary>
    public bool isDead;

    DifficultyManager difficulty;
    private EnemyAI enemyAI;
    // Use this for initialization
    void Start()
    {

        //Always want to start with the enemy alive
        difficulty = FindObjectOfType<DifficultyManager>();
        isDead = false;
        enemyAI = transform.GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        //Only if we have finished setting the enemy stats do we want an enemy to take damage.
        if (difficulty.statsSet == true)
        {
            //If our health ever hits 0
            if (health == 0)
            {
                //We die
                isDead = true;
                //Stop the enemy from moving anymore so that it will remain stationary during its death animation.
                enemyAI.lockMovement = true;
            }
            else
            {
                //If our health is not 0, we are alive.
                isDead = false;
            }
        }


        if (isDead == true)
        {
            //Destroy this object on death. NOTE: This is only temporary until we give enemies death animations.
            scoreEvent();
            Destroy(transform.gameObject);
        }
    }

    /// <summary>
    /// damages enemy by amount.
    /// </summary>
    /// <param name="amount"> The amount to damage the enemy by. </param>
    public void damage(float amount)
    {
        health = health - amount;
        if (health < 0)
        {
            //Ensures that our health is never a negative value.
            health = 0;
        }
    }

    /// <summary>
    /// Heals enemy by amount. If the new health exceeds the max health, it will simply bring it up to max health instead.
    /// </summary>
    /// <param name="amount"> The amount to heal by. </param>
    public void heal(float amount)
    {
        if ((health + amount) > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health = health + amount;
        }
    }

    private void scoreEvent()
    {
        Scoring score = FindObjectOfType<Scoring>();
        Enemy self = transform.GetComponent<Enemy>();
        DifficultyManager difficulty = FindObjectOfType<DifficultyManager>();

        if (self.meleeType == true)
        {
            //Score bonus increases with additional floors reached. Should be 1 for the first floor, 2 for floors 2 and 3, and 3 for floor 4.
            int bonusModifier = (score.meleeScore) * (Mathf.CeilToInt(((float)difficulty.floor + 1) / 2));
            score.score = score.score + bonusModifier;
        }
        else if (self.rangedType == true)
        {
            int bonusModifier = (score.rangedScore) * (Mathf.CeilToInt(((float)difficulty.floor + 1) / 2));
            score.score = score.score + ((score.rangedScore) * (Mathf.CeilToInt(((float)difficulty.floor + 1) / 2)));
        }
    }
}