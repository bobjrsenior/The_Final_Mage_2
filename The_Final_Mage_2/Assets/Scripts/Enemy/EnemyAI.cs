using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    /// <summary>
    /// The move speed for the enemy.
    /// </summary>
    public float enemySpeed = 1f;

    /// <summary>
    /// The rigidBody2D object attached to the enemy.
    /// </summary>
    private Rigidbody2D selfRigid;

    /// <summary>
    /// The enemy script attached to the object this script is attached to.
    /// </summary>
    private Enemy self;

    /// <summary>
    /// The rigidBody2D object attached to the player.
    /// </summary>
    private Rigidbody2D player;

    /// <summary>
    /// Allows this script itself to prevent enemy movement (For stopping melee type enemies when they are next to the player)
    /// </summary>
    private bool noMove;

    /// <summary>
    /// Allows an outside source to lock all movement from this enemy.
    /// </summary>
    public bool lockMovement;

    /// <summary>
    /// How much damage does the melee attack do?
    /// </summary>
    public float meleeDamage;

    /// <summary>
    /// How much damage does the ranged attack do?
    /// </summary>
    public float rangeDamage;

    /// <summary>
    /// Can the enemy shoot?
    /// </summary>
    public bool canShoot;

    /// <summary>
    /// How long do we wait before firing shots at the player?
    /// </summary>
    public float shootWaitTime;

    /// <summary>
    /// The speed of a ranged attack
    /// </summary>
    public float rangeSpeed;

    public GameObject rangeProjectile;


	// Use this for initialization
	void Start () {

        self = GetComponent<Enemy>();

        //Sets our rigidBody to our own rigidBody2D component.
        selfRigid = GetComponent<Rigidbody2D>();

        //Sets the player rigidbody2D object to the gameObject that the playerMovement script is attached to(AKA, the player)
        player = GameObject.FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate () {

        //If movement is locked on this enemy
        if (lockMovement == true || PlayerHealth.pHealth.isDead == true)
        {
            //Keep them where they are
            selfRigid.MovePosition(selfRigid.position);
        }
        else
        {
            //Determine the AI type and call the correct type of movement action for that type.
            if (self.meleeType == true)
            {
                meleeMovementAction();
            }
            else if (self.rangedType == true)
            {
                rangedMovementAction();
            }
        }
	}

    private void meleeMovementAction()
    {
        //If we are in the players detection zone(and therefore, allowed to move)
        if (self.inRadius == true)
        {
            //If the enemy is allowed to move...
            if (noMove == false)
            {
                //Get the direction to the player
                Vector2 direction = player.transform.position - transform.position;
                //Normalize it so that the magnitude is 1 (5 * direction gives a total value of 5 in that direction)
                direction.Normalize();

                //Move towards player at enemySpeed units/second
                selfRigid.MovePosition(selfRigid.position + direction * enemySpeed * Time.fixedDeltaTime);
            }
        }
    }

    private void rangedMovementAction()
    {
        //If we are in the players detection zone(and therefore, allowed to move)
        if (self.inRadius == true)
        {
            if (canShoot == false)
            {
                StartCoroutine(timeToShoot());
            }
            //Get the direction to the player
            Vector2 direction = player.transform.position - transform.position;

            //If the distance between our player and the enemy attached to this script is less than two...
            if (Vector2.Distance(player.transform.position, selfRigid.position) > 2.1)
            {
                //Normalize it so that the magnitude is 1 (5 * direction gives a total value of 5 in that direction)
                direction.Normalize();

                //Move towards player at enemySpeed units/second
                selfRigid.MovePosition(selfRigid.position + direction * enemySpeed * Time.fixedDeltaTime);
            }
            //If the distance to the player is between 2.1 and 2
            else if (Vector2.Distance(player.transform.position, selfRigid.position) <= 2.1 && Vector2.Distance(player.transform.position, selfRigid.position) >= 2)
            {
                //Remain where we are.
                selfRigid.MovePosition(selfRigid.position);
            }
            else
            {
                //Normalize it so that the magnitude is 1 (5 * direction gives a total value of 5 in that direction)
                direction.Normalize();

                //Move away from the player at enemySpeed units/second
                selfRigid.MovePosition(selfRigid.position - direction * enemySpeed * Time.fixedDeltaTime);
            }
        }
    }

    /// <summary>
    /// How long until the AI will shoot?
    /// </summary>
    /// <returns></returns>
    private IEnumerator timeToShoot()
    {
        canShoot = true;
        //Randomizes our wait time by subtracting the wait time by a random number between -2 to 0.
        yield return new WaitForSeconds(shootWaitTime - (float)Random.Range(-2, 1));
        Shoot();
        canShoot = false;
    }

    /// <summary>
    /// The AI shoot function.
    /// </summary>
    private void Shoot()
    {
        canShoot = false;
        Vector2 attack_vector = ((Vector3)player.position - transform.position).normalized;
        GameObject rAttack = Instantiate(rangeProjectile, (Vector2)transform.position, Quaternion.identity) as GameObject;
        rAttack.GetComponent<Rigidbody2D>().velocity = attack_vector * rangeSpeed * Time.fixedDeltaTime;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (self.meleeType == true)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                //Stop the enemy from moving if they are doing damage to us.
                lockMovement = true;

                PlayerHealth.pHealth.damage(meleeDamage);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Allow the enemy to move again since they are no longer damaging us.
            lockMovement = false;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {

        //This only applies to melee types, so if we are a ranged type, we will not worry about this.
        if (self.rangedType == false)
        {

            //Ensure the players rigidBody stays awake.
            PlayerMovement.pMovement.doNotSleep = true;
            //Debug.Log("Collision Detected with " + other.gameObject.name);
            //If we have collided with the player
            if (other.gameObject.tag == "Player")
            {
                //No more movement is allowed from the update function.
                noMove = true;
                //Ensures that the enemy will not move by moving their position to their current position.
                selfRigid.MovePosition(selfRigid.position);
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        //If we are no longer colliding with the player
        if (other.gameObject.tag == "Player")
        {

            //Let the players rigidBody go back to sleep.
            PlayerMovement.pMovement.doNotSleep = true;
            //Restore the update functions ability to move the enemy.
            noMove = false;
        }
    }
}
