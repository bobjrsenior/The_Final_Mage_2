using UnityEngine;
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

    public Animator anim;
    public PlayerMovement playerMovement;
    // Use this for initialization
    void Start () {

        //Always want to start with the player alive
        isDead = false;
        anim = GetComponent<Animator>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
	    //FOR TESTING PURPOSES ONLY, WILL DAMAGE YOU BY 1 IF YOU PRESS G
      //  if(Input.GetKeyDown(KeyCode.G))
      //  {
      //      damage(1);
      //  }

        //FOR TESTING PURPOSES ONLY, WILL HEAL YOU BY 1 IF YOU PRESS H
      //  if (Input.GetKeyDown(KeyCode.H))
      //  {
      //      heal(1);
      //  }

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
        health = health - amount;
        if (health < 0)
        {
            //Ensures that our health is never a negative value.
            health = 0;
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
}
