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

        //If the enemy is a melee type enemy
        if (self.meleeType == true)
        {
            //If we are in the players detection zone(and therefore, allowed to move)
            if (self.inRadius == true)
            {
                //Get the direction to the player
                Vector2 direction = player.transform.position - transform.position;
                //Normalize it so that the magnitude is 1 (5 * direction gives a total value of 5 in that direction)
                direction.Normalize();

                //Move towards player at enemySpeed units/second
                selfRigid.MovePosition(selfRigid.position + direction * enemySpeed * Time.fixedDeltaTime);
            }
        }
        else if (self.rangedType == true)
        {
            //If we are in the players detection zone(and therefore, allowed to move)
            if (self.inRadius == true)
            {
                //Get the direction to the player
                Vector2 direction = player.transform.position - transform.position;

                //If the distance between our player and the enemy attached to this script is less than two...
                if (Vector2.Distance(player.transform.position, selfRigid.position) > 2)
                {
                    //Normalize it so that the magnitude is 1 (5 * direction gives a total value of 5 in that direction)
                    direction.Normalize();

                    //Move towards player at enemySpeed units/second
                    selfRigid.MovePosition(selfRigid.position + direction * enemySpeed * Time.fixedDeltaTime);
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
	}
}
