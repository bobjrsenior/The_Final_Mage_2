﻿using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
    /// <summary>
    /// The value that holds our player health.
    /// </summary>
    public float health;

    /// <summary>
    /// The maximum health that we could attain.
    /// </summary>
    public float maxHealth;

    /// <summary>
    /// A boolean that corresponds to whether the player is dead or not.
    /// </summary>
    public bool isDead;

    /// <summary>
    /// True if we can be damaged, false if we cannot be damaged.
    /// </summary>
    public bool canDamage;

    /// <summary>
    /// How long do we delay before our player can be damaged again after being hit?
    /// </summary>
    public float delayTime;

    private Animator anim;
    private PlayerMovement playerMovement;
    // Use this for initialization
    void Start () {

        //Always want to start with the player alive
        isDead = false;
        //Always want to start where we can be damaged.
        canDamage = true;
        anim = GetComponent<Animator>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
	    //FOR TESTING PURPOSES ONLY, WILL DAMAGE YOU BY 1 IF YOU PRESS G
        if(Input.GetKeyDown(KeyCode.G))
        {
            damage(1);
        }

        //FOR TESTING PURPOSES ONLY, WILL HEAL YOU BY 1 IF YOU PRESS H
        if (Input.GetKeyDown(KeyCode.H))
        {
            heal(1);
        }

        //If our health ever hits 0
        if (health == 0)
        {
            //We die
            anim.SetBool("isDead", true);
            isDead = true;
            playerMovement.canMove = false;
        }
        else
        {
            //If our health is not 0, we are alive.
            anim.SetBool("isDead", false);
            isDead = false;
            playerMovement.canMove = true;
        }
	}

    /// <summary>
    /// damages player by amount.
    /// </summary>
    /// <param name="amount"> The amount to damage the player by. </param>
    public void damage(float amount)
    {
        if (canDamage == true)
        {
            //Starts our delay timer to prevent us from being damaged until the delay is complete.
            StartCoroutine(afterDamageDelay());

            health = health - amount;
            if (health < 0)
            {
                //Ensures that our health is never a negative value.
                health = 0;
            }
        }
    }

    /// <summary>
    /// Heals player by amount. If the new health exceeds the max health, it will simply bring it up to max health instead.
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

    /// <summary>
    /// Delays for delayTime seconds before allowing the player to be damaged again.
    /// </summary>
    /// <returns></returns>
    private IEnumerator afterDamageDelay()
    {
        canDamage = false;
        //Will wait for delayTime seconds.
        yield return new WaitForSeconds(delayTime);
        canDamage = true;
    }
}
