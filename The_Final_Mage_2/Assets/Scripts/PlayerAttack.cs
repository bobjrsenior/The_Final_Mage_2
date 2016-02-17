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

                if (attack_vector.x > 0)
                {
                    //NOTE: THESE VALUES ARE ONLY DIFFERENT TO MAINTAIN THE SIZE OF THE TEST SPRITE. IN THE FINAL PRODUCT, THEY SHOULD ALL WORK AT 1F.
                    transform.localScale = new Vector3(2.33f, 2.27f, 1f);
                }
                else if (attack_vector.x < 0)//Handles flipping player sprites to face left
                {
                    transform.localScale = new Vector3(-2.33f, 2.27f, 1f);
                }

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

    private IEnumerator swapCooldown()
    {
        swapped = true;
        yield return new WaitForSeconds(swapCooldownTime);
        swapped = false;
        debugger.Log("Swap enabled again.");
    }
}
