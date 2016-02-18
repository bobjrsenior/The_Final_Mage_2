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



    private EnemyAI enemyAI;
    // Use this for initialization
    void Start()
    {
        //The followings two declarations are generalized defaults for all enemies simply intact for testing purposes alone.
        maxHealth = 3;
        health = 3;

        //Always want to start with the enemy alive
        isDead = false;
        enemyAI = transform.GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
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
        if (self.meleeType == true)
        {
            score.score = score.score + score.meleeScore;
        }
        else if (self.rangedType == true)
        {
            score.score = score.score + score.meleeScore;
        }
    }
}