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
	void Update () {

        //Moves the object this script is attached to toward the player. THIS WILL HAVE TO BE CHANGED OR MODIFIED HEAVILY TO WORK BETTER.
        transform.position = Vector2.MoveTowards(self.position, player.position, enemySpeed * Time.fixedDeltaTime);


	}
}
