﻿using UnityEngine;
using System.Collections;


public class PlayerAttack : MonoBehaviour {

    /// <summary>
    /// The length of time in seconds we will delay before we can use another melee attack.
    /// </summary>
    public float meleeDelay;

    /// <summary>
    /// the cooldown for melee attacks
    /// </summary>
    public float meleeCool;
    /// <summary>
    /// Whether or not we can melee attack.
    /// </summary>
    public bool canMelee;

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
    /// Whether or not we are attacking.
    /// </summary>
    public bool isAttacking;

    /// <summary>
    /// Array to hold all of our sounds for the player so they can be accessed.
    /// </summary>
    public AudioClip[] audioClip;


    RaycastHit2D[] rayHit;
    RaycastHit2D rayHitWall;
    /// <summary>
    /// Lets us control debugging so we do not have to delete the debug code.
    /// </summary>
    DebugUtility debugger;
    Animator anim;
    PlayerMovement movement;
	// Use this for initialization
	void Start () {

        //Start in melee type
        meleeType = true;
        debugger = FindObjectOfType<DebugUtility>();
        canMelee = true;
        anim = transform.GetComponent<Animator>();
        movement = transform.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
	    
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            switchType();
        }
        if (canMelee == true && meleeType == true)
        {
            //Melee attack
            Vector2 attack_vector = new Vector2(Input.GetAxisRaw("FireHorizontal"), Input.GetAxisRaw("FireVertical"));
            if (attack_vector != Vector2.zero)
            {
                anim.SetBool("isMelee", true);
                anim.SetFloat("attackX", attack_vector.x);
                anim.SetFloat("attackY", attack_vector.y);
                isAttacking = true;
                PlaySound(0);

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

                        debugger.Log("Player melee attack collided with " + rayHit[x].collider.name);
                        //If the ray's tag is an enemy
                        if (rayHit[x].collider.CompareTag("Enemy"))
                        {
                            //Get and damage the enemy by our melee damage
                            Rigidbody2D enemyRigid = rayHit[x].collider.GetComponent<Rigidbody2D>();
                            EnemyHealth enemyHP = rayHit[x].collider.GetComponent<EnemyHealth>();
                            enemyHP.damage(meleeDamage);
                            //Searches for a wall, starting at the enemy's position and moving along the same vector as our attack - the direction of a knockback.
                            rayHitWall = Physics2D.Raycast(rayHit[x].collider.transform.position, attack_vector, .2f, 1 << 10);

                            //If we did not touch a wall
                            if (rayHitWall.collider == null)
                            {
                                //Knockback effect of melee.
                                enemyRigid.AddForce(attack_vector * 200);
                            }
                        }
                    }
                }
                
                //Handle our delays
                StartCoroutine(delayMelee());
                StartCoroutine(meleeCooldown());
            }
        }
        
	}
    //TEMPORARY FOR DISPLAYING TYPE
    void OnGUI()
    {
        GUI.color = Color.yellow;
        if (meleeType == true)
        {
            GUI.Box(new Rect(0, 40, 100, 25), "Type: Melee");
        }
        else
        {
            GUI.Box(new Rect(0, 40, 100, 25), "Type: Ranged");
        }
        
    }
    /// <summary>
    /// Delays us from attacking again after a melee for a specified amount of seconds.
    /// </summary>
    /// <returns></returns>
    IEnumerator delayMelee()
    {
        canMelee = false;
        yield return new WaitForSeconds(meleeDelay);
        canMelee = true;
    }

    /// <summary>
    /// The cooldown before our melee animation will stop after a melee attack.
    /// </summary>
    /// <returns></returns>
    IEnumerator meleeCooldown()
    {
        yield return new WaitForSeconds(meleeCool);
        anim.SetBool("isMelee", false);
        isAttacking = false;
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
            debugger.Log("Swapped to range type.");
            StartCoroutine(swapCooldown());
        }
        else if (rangeType == true && swapped == false)
        {
            rangeType = false;
            meleeType = true;
            debugger.Log("Swapped to melee type.");
            StartCoroutine(swapCooldown());
        }
    }

    /// <summary>
    /// Plays a sound from the array when given an integer.
    /// </summary>
    /// <param name="clip"></param>
    void PlaySound(int clip)
    {
        GetComponent<AudioSource>().clip = audioClip[clip];
        GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// Handles the cooldown between swap attacks.
    /// </summary>
    /// <returns></returns>
    private IEnumerator swapCooldown()
    {
        swapped = true;
        yield return new WaitForSeconds(swapCooldownTime);
        swapped = false;
        debugger.Log("Swap enabled again.");
    }
}
