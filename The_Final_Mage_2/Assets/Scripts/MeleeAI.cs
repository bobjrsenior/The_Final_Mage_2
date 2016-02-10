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
    private Rigidbody2D self;

    /// <summary>
    /// The rigidBody2D object attached to the player.
    /// </summary>
    private Rigidbody2D player;

	// Use this for initialization
	void Start () {

        //Sets the self rigidbody2D object to the one this script is attached to.
        self = GetComponent<Rigidbody2D>();
        
        //Sets the player rigidbody2D object to the gameObject that the playerMovement script is attached to(AKA, the player)
        player = GameObject.FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        //Get the direction to the player
        Vector2 direction = player.transform.position - transform.position;
        //Normalize it so that the magnitude is 1 (5 * direction gives a total value of 5 in that direction)
        direction.Normalize();
        
        //Move towards player at enemySpeed units/second   
        self.MovePosition(self.position + direction * enemySpeed * Time.fixedDeltaTime);

	}
}
