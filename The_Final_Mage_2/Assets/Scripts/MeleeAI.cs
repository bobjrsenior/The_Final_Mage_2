using UnityEngine;
using System.Collections;

public class MeleeAI : MonoBehaviour {

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
        
        //Sets our rigidBody to our own rigidBody2D component.
        selfRigid = GetComponent<Rigidbody2D>();

        //Sets the player rigidbody2D object to the gameObject that the playerMovement script is attached to(AKA, the player)
        player = GameObject.FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        //If we are in the players detection zone(and therefore, allowed to move)
        if (self.inRadius == true)
        {
            //Moves the object this script is attached to toward the player. THIS WILL HAVE TO BE CHANGED OR MODIFIED HEAVILY TO WORK BETTER.
            transform.position = Vector2.MoveTowards(selfRigid.position, player.position, enemySpeed * Time.fixedDeltaTime);

        }
    }
}
