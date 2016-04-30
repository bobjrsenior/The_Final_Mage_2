using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{

    /// <summary>
    /// Whether or not we the rigidbody must stay awake or not.
    /// </summary>
    public bool doNotSleep;

    /// <summary>
    /// Holds the objects rigidbody (used for physics)
    /// </summary>
    private Rigidbody2D rbody;

    /// <summary>
    /// The speed at which this object moves in Units/Second
    /// </summary>
    public float speed = 1.0f;

    /// <summary>
    /// The animator for the player character.
    /// </summary>
    private Animator anim;

    /// <summary>
    /// The boolean that controls whether or not the player can move.
    /// </summary>
    public bool canMove;

    /// <summary>
    /// Our last X input
    /// </summary>
    public float inputX;

    /// <summary>
    /// Our last Y input
    /// </summary>
    public float inputY;

    public static PlayerMovement pMovement;

    public static Rigidbody2D playerRBody;

    void Awake()
    {
        if (pMovement == null)
        {
            pMovement = this;
        }
    }
    void Start()
    {   
        //Default our canMove boolean to true at game start.
        canMove = true;
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If we do not want the rigidbody to sleep, make sure it is awake.
        if (doNotSleep == true)
        {
            rbody.WakeUp();
        }
        
        //Stops player from moving if they are dead.
        if (PlayerHealth.pHealth.health == 0)
        {
            canMove = false;
        }

        //Gets the direction you want to move based on input
        Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


        if (movement_vector != Vector2.zero && canMove != false)//If we can move the vector from our input is not equal to zero (AKA, we are trying to move)
        {
            anim.SetBool("isWalking", true);//Tell the animator that we are trying to walk.
            anim.SetFloat("input_x", movement_vector.x);
            anim.SetFloat("input_y", movement_vector.y);

            //Stores our last input
            inputX = movement_vector.x;
            inputY = movement_vector.y;
            //Move in the direction in speed Units/Second
            //Time.fixedDeltaTime smooths it to Units/Second instead of Units/Frame
            rbody.MovePosition(rbody.position + movement_vector * speed * Time.fixedDeltaTime);
        }
        else
        {
            anim.SetBool("isWalking", false);
            //Stops our movement.
            rbody.MovePosition(rbody.position);
            //We do not update the input_x and input_y here because we want them to retain their last input so that we will continue facing the same direction.
        }

        //If we are not attacking...
        if (PlayerAttack.pAttack.isAttacking == false && canMove == true)
        {
            //Handles flipping player sprites to face right.
            if (movement_vector.x > 0)
            {
                //NOTE: THESE VALUES ARE ONLY DIFFERENT TO MAINTAIN THE SIZE OF THE TEST SPRITE. IN THE FINAL PRODUCT, THEY SHOULD ALL WORK AT 1F.
                transform.localScale = new Vector3(2.33f, 2.27f, 1f);
            }
            else if (movement_vector.x < 0)//Handles flipping player sprites to face left
            {
                transform.localScale = new Vector3(-2.33f, 2.27f, 1f);
            }
        }
    }
}
