using UnityEngine;
using System.Collections;

public class finalBossHealth : MonoBehaviour
{

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

    public Timer flashTimer;

    Enemy self;

    // Use this for initialization
    void Start()
    {
        //Always want to start with the enemy alive
        isDead = false;
        flashTimer = gameObject.AddComponent<Timer>();
        flashTimer.initialize(.1f, false);
    }

    // Update is called once per frame
    void Update()
    {
        //If our health ever hits 0
        if (health == 0)
        {
            if (!isDead)
            {
                scoreEvent();
            }
            //We die
            isDead = true;
        }
        else
        {
            //If our health is not 0, we are alive.
            isDead = false;
        }

            //Let final boss AI script handle death.
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
        Scoring.scoreKeeper.score += 20000;
    }
}