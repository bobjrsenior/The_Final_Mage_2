using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

    /// <summary>
    /// The length of time in seconds we will delay before we can use another melee attack.
    /// </summary>
    public float meleeDelay;

    /// <summary>
    /// Whether or not we can melee attack.
    /// </summary>
    public bool canMelee;
    Animator anim;
    PlayerMovement movement;
	// Use this for initialization
	void Start () {

        canMelee = true;
        anim = transform.GetComponent<Animator>();
        movement = transform.GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
	    
        if (canMelee == true)
        {
            //Melee attack
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("isMelee", true);
                StartCoroutine(delayMelee());
                StartCoroutine(meleeCooldown());
            }
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
        yield return new WaitForSeconds(.2f);
        anim.SetBool("isMelee", false);
    }
}
