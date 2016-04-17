using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    //We create an array of health images in the player
    //So that when the scene loads we can reference the player health images
    //The UIManager will pull from this which image to show based on health
    public Sprite[] images; //Health
    public Sprite[] __exps; //Exp


    /// <summary>
    /// The value that holds our current player health.
    /// </summary>
    public float health = 5.0f;

    /// <summary>
    /// The maximum health that we could attain.
    /// </summary>
    public float maxHealth = 5.0f;

    /// <summary>
    /// The maximum mana that we could attain
    /// </summary>
    public float maxMana = 100.0f;

    /// <summary>
    /// The value that holds our current player mana.
    /// </summary>
    public float mana = 100.0f;
    /// <summary>
    /// A boolean that corresponds to whether the player is dead or not.
    /// </summary>
    public bool isDead = false;

    /// <summary>
    /// True if we can be damaged, false if we cannot be damaged.
    /// </summary>
    public bool canDamage = true;

    /// <summary>
    /// How long do we delay before our player can be damaged again after being hit?
    /// </summary>
    public float delayTime = 1.0f;

    private Timer delayTimer;

    public float healthRegenTime = 20.0f;

    private Timer healthRegenTimer;

    public float manaRegenTime = 5f;

    private Timer manaRegenTimer;

    private Timer gameOverTimer;

    public float manaRegenAmount = 5f;

    public float manaCost = 5f;

    public bool healthRegenCooldown = false;

    public bool manaRegenCooldown = false;

    private Animator anim;

    public SoundScript soundSource;

    public static PlayerHealth pHealth;

    private bool healthSkillApplied;

    private bool manaSkillApplied;


    void Awake () {

        if (pHealth != null)
        {
            Destroy(transform.root.gameObject);
        }
        else
        {
            pHealth = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }

    }

    void Start()
    {
        
        //Always want to start with the player alive
        isDead = false;
        //Always want to start where we can be damaged.
        canDamage = true;
        manaRegenTimer = gameObject.AddComponent<Timer>();
        manaRegenTimer.initialize(manaRegenTime, false);
        healthRegenTimer = gameObject.AddComponent<Timer>();
        healthRegenTimer.initialize(healthRegenTime, false);
        delayTimer = gameObject.AddComponent<Timer>();
        delayTimer.initialize(delayTime, false);
        gameOverTimer = gameObject.AddComponent<Timer>();
        gameOverTimer.initialize(2, false);
        anim = GetComponent<Animator>();
        soundSource = FindObjectOfType<SoundScript>();
    }
	
	// Update is called once per frame
	void Update () {

        //If we have purchased the health regen skill.
        if (Skills.pSkills.skill3 == true && healthSkillApplied == false)
        {
            healthRegenTimer.initialTime -= 5;
            if (healthRegenTimer.started == true)
            {
                healthRegenTimer.time = 0;
            }
            healthSkillApplied = true;
        }

        //If we have purchased the health regen skill.
        if (Skills.pSkills.skill4 == true && manaSkillApplied == false)
        {
            manaRegenTimer.initialTime -= 2;
            if (manaRegenTimer.started == true)
            {
                manaRegenTimer.time = 0;
            }
            manaSkillApplied = true;
        }

        //Checks to see if we are currently in a game over scenario.
        if (health == 0 && gameOverTimer.started == false)
        {
            StartCoroutine(gameOver());
        }
        //Health regen over time.
        if (healthRegenCooldown == false && PlayerHealth.pHealth.health != PlayerHealth.pHealth.maxHealth)
        {
            StartCoroutine(healthRegen());
        }
        if (manaRegenCooldown == false && PlayerHealth.pHealth.mana != PlayerHealth.pHealth.maxMana)
        {
            if (mana < maxMana)
            {
                StartCoroutine(manaRegen());
            }
        }
        if (health == 0)
        {
            PlayerMovement.pMovement.canMove = false;
        }
        else
        {
            PlayerMovement.pMovement.canMove = true;
        }
        

	    //FOR TESTING PURPOSES ONLY, WILL DAMAGE YOU BY 1 IF YOU PRESS G
        if(Input.GetKeyDown(KeyCode.G))
        {
            health = 0;
        }

        //FOR TESTING PURPOSES ONLY, WILL HEAL YOU BY 1 IF YOU PRESS H
          if (Input.GetKeyDown(KeyCode.H))
          {
              heal(1);
          }

        //If our health ever hits 0
        if (health == 0)
        {
            //Disable the score countdown
            Scoring.scoreKeeper.countdown = false;
            //We die
            anim.SetBool("isDead", true);
            isDead = true;
            PlayerMovement.pMovement.canMove = false;
        }
        else
        {
            //Enable the score countdown.
            Scoring.scoreKeeper.countdown = true;
            //If our health is not 0, we are alive.
            anim.SetBool("isDead", false);
            isDead = false;
            PlayerMovement.pMovement.canMove = true;
        }
	}

    /// <summary>
    /// damages player by amount.
    /// </summary>
    /// <param name="amount"> The amount to damage the player by. </param>
    public void damage(float amount)
    {
        if (canDamage == true && health > 0)
        {
            //Starts our delay timer to prevent us from being damaged until the delay is complete.
            StartCoroutine(afterDamageDelay());
            health = health - amount;
            soundSource.PlaySound(2);
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
    public void heal(int amount)
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

    public void addMana(int amount)
    {
        if ((mana + amount) >= maxMana)
        {
            mana = maxMana;
        }
        else
        {
            mana = mana + amount;
        }
    }

    /// <summary>
    /// Delays for delayTime seconds before allowing the player to be damaged again.
    /// </summary>
    /// <returns></returns>
    private IEnumerator afterDamageDelay()
    {
        //Prevent us from taking damage until the initial delay is complete.
        canDamage = false;
        delayTimer.started = true;
        //Will wait for delayTime seconds.
        while (delayTimer.complete == false)
        {
            delayTimer.countdownUpdate();
            yield return null;
        }
        delayTimer.complete = false;
        canDamage = true;
        yield break;
    }

    /// <summary>
    /// Delays for healthRegenTime seconds and then heals us by 1 point of health.
    /// </summary>
    /// <returns></returns>
    private IEnumerator healthRegen()
    {
        healthRegenCooldown = true;
        healthRegenTimer.started = true;
        while (healthRegenTimer.complete == false)
        {
            healthRegenTimer.countdownUpdate();
            yield return null;
        }
        healthRegenTimer.complete = false;
        if (health != 0)
        {
            heal(1);
        }
        healthRegenCooldown = false;
        yield break;
    }

    public IEnumerator manaRegen()
    {
        manaRegenCooldown = true;
        manaRegenTimer.started = true;
        while (manaRegenTimer.complete == false)
        {
            manaRegenTimer.countdownUpdate();
            yield return null;
        }
        manaRegenTimer.complete = false;
        if (mana < maxMana)
        {
            mana = mana + manaRegenAmount;
        }
        manaRegenCooldown = false;
        yield break;
    }

    /// <summary>
    /// Delays a few seconds after death before throwing the player to the game over screen.
    /// </summary>
    private IEnumerator gameOver()
    {
        gameOverTimer.started = true;
        //Show the game over text.
        TextBoxScript.textScript.textbox.text = "Dr. Evil: Dead! He is dead at last! Victory is mine!";
        TextBoxScript.textScript.showTextbox();
        while (gameOverTimer.complete == false)
        {
            gameOverTimer.countdownUpdate();
            yield return null;
        }
        gameOverTimer.complete = false;
        //Remove the game over text.
        TextBoxScript.textScript.hideTextbox();
        if (health == 0)
        {
            //Experience and skills object is redundant on game over, so destroy them before loading the next scene.
            Destroy(Experience.playerExperience.transform.root.gameObject);
            Destroy(Skills.pSkills.transform.root.gameObject);
            SoundScript.exists = false;
            Destroy(FindObjectOfType<SoundScript>().transform.gameObject);
            Destroy(transform.root.gameObject);
            Destroy(TextBoxScript.textScript.transform.root.gameObject);
            SceneManager.LoadScene("GameOver");
        }
    }
}
