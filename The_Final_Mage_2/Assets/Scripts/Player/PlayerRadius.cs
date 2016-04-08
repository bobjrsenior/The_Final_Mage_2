using UnityEngine;
using System.Collections;
/// <summary>
/// A script that allows a trigger volume larger than the size of any given room to follow the player and enable enemy movement when they enter its radius.
/// </summary>
public class PlayerRadius : MonoBehaviour {

    /// <summary>
    /// The player object.
    /// </summary>
    private GameObject player;

	// Use this for initialization
	void Start () {
        //The player object is an object tagged as Player
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        //This moves the object attached to the PlayerRadius script to the same position as the player at every frame, ensuring the radius is always centered on the player.
        transform.position = player.GetComponent<Rigidbody2D>().position;

	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //If the object that entered our trigger zone is tagged as an enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            try
            {
                //Set that enemy's "inRadius" boolean to true.
                other.gameObject.GetComponent<Enemy>().setInRadiusTrue();
            }
            catch { }
            StartCoroutine(TextBoxScript.textScript.displayMessage());
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        //If the object that exited our trigger zone is tagged as an enemy
        if (other.gameObject.tag == "Enemy")
        {
            try
            {
                //Set that enemy's "inRadius" boolean to false.
                other.gameObject.GetComponent<Enemy>().setInRadiusFalse();
            }
            catch { }
        }
    }
}
