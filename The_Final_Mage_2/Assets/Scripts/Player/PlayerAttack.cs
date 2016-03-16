using UnityEngine;
using System.Collections;


public class PlayerAttack : MonoBehaviour {

    /// <summary>
    /// The length of time in seconds we will delay before we can use another melee attack.
    /// </summary>
    public float meleeDelay;
    /// <summary>
    /// The length of time in seconds we will delay before we can use another range attack.
    /// </summary>
    public float rangeDelay;
    /// <summary>
    /// the cooldown for melee attacks
    /// </summary>
    public float meleeCool;
    /// <summary>
    /// Whether or not we can melee attack.
    /// </summary>
    public bool canMelee;
    /// <summary>
    /// Whether or not we can range attack
    /// </summary>
    public bool canRange;
    /// <summary>
    /// True if we are melee type, false otherwise
    /// </summary>
    public bool meleeType;
    /// <summary>
    /// True if we are range type, false otherwise
    /// </summary>
    public bool rangeType;
    /// <summary>
    /// True if we have swapped, false when the cooldown ends.
    /// </summary>
    public bool swapped;
    /// <summary>
    /// True if we have swapped, false when the cooldown ends.
    /// </summary>
    public float swapCooldownTime;

    /// <summary>
    /// How much damage does our melee attack do?
    /// </summary>
    public float meleeDamage;

    /// <summary>
    /// How much damage does our ranged attack do?
    /// </summary>
    public float rangeDamage;

    /// <summary>
    /// How fast does the ranged attack move?
    /// </summary>
    public float rangeSpeed;

    /// <summary>
    /// Whether or not we are attacking.
    /// </summary>
    public bool isAttacking;

    public SoundScript soundSource;

    private Timer meleeDelayTimer;

    private Timer swapCooldownTimer;

    private Timer delayRangeTimer;

    private Timer meleeCooldownTimer;

    public GameObject rangeProjectile;

    RaycastHit2D[] rayHit;

    Animator anim;

    /// <summary>
    /// A playerattack object we can reference anywhere
    /// </summary>
    public static PlayerAttack pAttack;

    void Awake()
    {
        pAttack = this;
    }
	// Use this for initialization	void Start () {

    void Start () {
        //Start in melee type
        meleeType = true;
        canMelee = true;
        canRange = true;
        meleeCooldownTimer = gameObject.AddComponent<Timer>();
        meleeCooldownTimer.initialize(meleeCool, false);
        delayRangeTimer = gameObject.AddComponent<Timer>();
        delayRangeTimer.initialize(rangeDelay, false);
        swapCooldownTimer = gameObject.AddComponent<Timer>();
        swapCooldownTimer.initialize(swapCooldownTime, false);
        meleeDelayTimer = gameObject.AddComponent<Timer>();
        meleeDelayTimer.initialize(meleeDelay, false);
        anim = transform.GetComponent<Animator>();
        soundSource = FindObjectOfType<SoundScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            switchType();
        }
        if (canMelee == true && meleeType == true && PlayerHealth.pHealth.health != 0)
        {
            //Melee attack
            Vector2 attack_vector = new Vector2(Input.GetAxisRaw("FireHorizontal"), Input.GetAxisRaw("FireVertical"));
            if (attack_vector != Vector2.zero)
            {
                anim.SetBool("isMelee", true);
                anim.SetFloat("attackX", attack_vector.x);
                anim.SetFloat("attackY", attack_vector.y);
                isAttacking = true;
                soundSource.PlaySound(0);

                if (attack_vector.x > 0)
                {
                    //NOTE: THESE VALUES ARE ONLY DIFFERENT TO MAINTAIN THE SIZE OF THE TEST SPRITE. IN THE FINAL PRODUCT, THEY SHOULD ALL WORK AT 1F.
                    transform.localScale = new Vector3(2.33f, 2.27f, 1f);
                }
                else if (attack_vector.x < 0)//Handles flipping player sprites to face left
                {
                    transform.localScale = new Vector3(-2.33f, 2.27f, 1f);
                }

                //Draws a ray in the form of a circle from the player in the direction we are attacking, extending at a distance of .5f units, shifting the layer's bit from layer 9.
                rayHit = Physics2D.CircleCastAll(transform.position, .1f, attack_vector, .4f, 1 << 9);
                //If our ray has collided
                for (int x = 0; x < rayHit.Length; x++)
                {
                    if (rayHit[x].collider != null)
                    {
                        //If the ray's tag is an enemy
                        if (rayHit[x].collider.CompareTag("Enemy"))
                        {
                            //Get and damage the enemy by our melee damage
                            Rigidbody2D enemyRigid = rayHit[x].collider.GetComponent<Rigidbody2D>();
                            EnemyHealth enemyHP = rayHit[x].collider.GetComponent<EnemyHealth>();
                            enemyHP.damage(meleeDamage);
                            //Searches for a wall, starting at the enemy's position and moving along the same vector as our attack - the direction of a knockback.

                            enemyRigid.AddForce(attack_vector * 300);

                        }
                    }
                }
                
                //Handle our delays
                StartCoroutine(delayMelee());
                StartCoroutine(meleeCooldown());
            }
        }
        else if (canRange == true && rangeType == true && PlayerHealth.pHealth.health != 0 && PlayerHealth
            .pHealth.mana != 0)
        {
            Vector2 attack_vector = new Vector2(Input.GetAxisRaw("FireHorizontal"), Input.GetAxisRaw("FireVertical"));
            if (attack_vector != Vector2.zero)
            {
                anim.SetBool("isRange", true);
                anim.SetFloat("attackX", attack_vector.x);
                anim.SetFloat("attackY", attack_vector.y);
                isAttacking = true;
                if (attack_vector.x > 0)
                {
                    //NOTE: THESE VALUES ARE ONLY DIFFERENT TO MAINTAIN THE SIZE OF THE TEST SPRITE. IN THE FINAL PRODUCT, THEY SHOULD ALL WORK AT 1F.
                    transform.localScale = new Vector3(2.33f, 2.27f, 1f);
                }
                else if (attack_vector.x < 0)//Handles flipping player sprites to face left
                {
                    transform.localScale = new Vector3(-2.33f, 2.27f, 1f);
                }

                if (canRange == true && PlayerHealth.pHealth.mana >= PlayerHealth.pHealth.manaCost)
                {
                    StartCoroutine(delayRange());
                    shoot(attack_vector);
                    PlayerHealth.pHealth.mana = PlayerHealth.pHealth.mana - PlayerHealth.pHealth.manaCost;
                    soundSource.PlaySound(1);
                }

            }
                
        }
        
	}
    //TEMPORARY FOR DISPLAYING TYPE
    void OnGUI()
    {
        GUI.color = Color.yellow;
        if (meleeType == true)
        {
            GUI.Box(new Rect(0, 60, 100, 25), "Type: Melee");
        }
        else
        {
            GUI.Box(new Rect(0, 60, 100, 25), "Type: Ranged");
        }
        
    }
    /// <summary>
    /// Delays us from attacking again after a melee for a specified amount of seconds.
    /// </summary>
    /// <returns></returns>
    IEnumerator delayMelee()
    {
        canMelee = false;
        meleeDelayTimer.started = true;
        while (canMelee == false)
        {
            //Countdown sequence for our timer.
            meleeDelayTimer.countdownUpdate();
            //Break out once the timer is complete.
            if (meleeDelayTimer.complete == true)
                break;
            yield return null;
        }
        //Once the timer is done, we can melee again.
        canMelee = true;
        //End the coroutine.
        yield break;
    }

    /// <summary>
    /// Delays us from shooting a ranged attack again for a specified number of seconds.
    /// </summary>
    /// <returns></returns>
    IEnumerator delayRange()
    {
        canRange = false;
        delayRangeTimer.started = true;
        while (delayRangeTimer.complete == false)
        {
            delayRangeTimer.countdownUpdate();
            yield return null;
        }
        delayRangeTimer.complete = false;
        canRange = true;
        anim.SetBool("isRange", false);
        isAttacking = false;
        yield break;
    }

    /// <summary>
    /// The cooldown before our melee animation will stop after a melee attack.
    /// </summary>
    /// <returns></returns>
    IEnumerator meleeCooldown()
    {
        meleeCooldownTimer.started = true;
        while (meleeCooldownTimer.complete == false)
        {
            meleeCooldownTimer.countdownUpdate();
            yield return null;
        }
        meleeCooldownTimer.complete = false;
        anim.SetBool("isMelee", false);
        isAttacking = false;
        yield break;
    }

    /// <summary>
    /// Switches between melee and range type/
    /// </summary>
    public void switchType()
    {
        if (meleeType == true && swapped == false)
        {
            meleeType = false;
            rangeType = true;
            StartCoroutine(swapCooldown());
        }
        else if (rangeType == true && swapped == false)
        {
            rangeType = false;
            meleeType = true;
            StartCoroutine(swapCooldown());
        }
    }

   
    /// <summary>
    /// Handles the cooldown between swap attacks.
    /// </summary>
    /// <returns></returns>
    private IEnumerator swapCooldown()
    {
        swapped = true;
        swapCooldownTimer.started = true;
        while (swapCooldownTimer.complete == false)
        {
            swapCooldownTimer.countdownUpdate();
            yield return null;
        }
        swapCooldownTimer.complete = false;
        swapped = false;
        yield break;
    }

    /// <summary>
    /// Shoots a projectile in the direction of the attack vector.
    /// </summary>
    /// <param name="attack_vector">The direction the projectile will move in.</param>
    private void shoot(Vector2 attack_vector)
    {
        GameObject rAttack = Instantiate(rangeProjectile, (Vector2)transform.position, Quaternion.identity) as GameObject;
        rAttack.GetComponent<Rigidbody2D>().velocity = attack_vector * rangeSpeed * Time.fixedDeltaTime;
    }
}
