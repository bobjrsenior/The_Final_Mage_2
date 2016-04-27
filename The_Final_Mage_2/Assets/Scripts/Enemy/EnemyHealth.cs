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

    public int expToGive;

    /// <summary>
    /// A boolean that corresponds to whether the enemy is dead or not.
    /// </summary>
    public bool isDead;

    /// <summary>
    /// For spawning mana vials on death.
    /// </summary>
    public GameObject manaVial;

    public Timer flashTimer;

    Enemy self;

    private EnemyAI enemyAI;
    // Use this for initialization
    void Start()
    {
        //Always want to start with the enemy alive
        isDead = false;
        try
        {
            flashTimer = gameObject.AddComponent<Timer>();
            flashTimer.initialize(.1f, false);
            self = transform.GetComponent<Enemy>();
            enemyAI = transform.GetComponent<EnemyAI>();
        }
        catch
        {
        }


        
    }

    // Update is called once per frame
    void Update()
    {
        //Only if we have finished setting the enemy stats do we want an enemy to take damage.
        if (DifficultyManager.dManager.statsSet == true)
        {
            //If our health ever hits 0
            if (health == 0)
            {
                //We die
                isDead = true;
                if (enemyAI != null)
                {
                    //Stop the enemy from moving anymore so that it will remain stationary during its death animation.
                    enemyAI.lockMovement = true;
                }
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
            Experience.playerExperience.addEXP(expToGive);
            if (LevelGen.gen != null)
            {
                LevelGen.gen.enemyDied(transform.position);
            }

            //If we have enhanced drop rate...
            if (Skills.pSkills.skill1 == true)
            {
                if (Random.Range(0, 100) >= 70)
                {
                    Instantiate(manaVial, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                }
            }
            else
            {
                if (Random.Range(0, 100) >= 85)
                {
                    Instantiate(manaVial, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                }
            }   
            Destroy(transform.gameObject);
        }
    }

    private IEnumerator damageFlash()
    {
        transform.GetComponent<SpriteRenderer>().color = Color.red;
        flashTimer.started = true;
        {
            while (flashTimer.complete == false)
            {
                flashTimer.countdownUpdate();
                yield return null;
            }
            flashTimer.complete = false;
            transform.GetComponent<SpriteRenderer>().color = Color.white;
            yield return null;
        }
    }

    /// <summary>
    /// damages enemy by amount.
    /// </summary>
    /// <param name="amount"> The amount to damage the enemy by. </param>
    public void damage(float amount)
    {
        StartCoroutine(damageFlash());
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
        int bonusModifier;
        if(self == null)
        {

        }
        else if (self.meleeType == true)
        {
            //Score bonus increases with additional floors reached. Should be 1 for floors 1-2, 2 f or 3-4, and 3 for 5-6.
            if (DifficultyManager.dManager.floor > 2 && DifficultyManager.dManager.floor <= 4)
            {
                bonusModifier = 2;
            }
            else if (DifficultyManager.dManager.floor > 4)
            {
                bonusModifier = 3;
            }
            else
            {
                bonusModifier = 1;
            }
            Scoring.scoreKeeper.score = Scoring.scoreKeeper.score + (Scoring.scoreKeeper.meleeScore * bonusModifier) * (DifficultyTracker.difficultyTrack.getDifficulty());
        }
        else if (self.rangedType == true)
        {
            //Score bonus increases with additional floors reached. Should be 1 for floors 1-2, 2 f or 3-4, and 3 for 5-6.
            if (DifficultyManager.dManager.floor > 2 && DifficultyManager.dManager.floor <= 4)
            {
                bonusModifier = 2;
            }
            else if (DifficultyManager.dManager.floor > 4)
            {
                bonusModifier = 3;
            }
            else
            {
                bonusModifier = 1;
            }
            //Should double the score on hard and take into account the floor modifier.
            Scoring.scoreKeeper.score = Scoring.scoreKeeper.score + (Scoring.scoreKeeper.meleeScore * bonusModifier) * (DifficultyTracker.difficultyTrack.getDifficulty());
        }
    }
}