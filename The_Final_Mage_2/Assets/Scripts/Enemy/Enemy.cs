using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    /// <summary>
    /// Whether or not the enemy attached to this script is in radius of the players detection zone.
    /// </summary>
    public bool inRadius = false;

    /// <summary>
    /// This will be used to tell that the enemy is of melee type.
    /// </summary>
    public bool meleeType = false;

    /// <summary>
    /// This will be ued to tell that the enemy is of ranged type.
    /// </summary>
    public bool rangedType = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	   
	}
    /// <summary>
    /// Sets the inRadius boolean to false.
    /// </summary>
    public void setInRadiusFalse()
    {
        inRadius = false;
    }
    /// <summary>
    /// Sets the inRadius boolean to true.
    /// </summary>
    public void setInRadiusTrue()
    {
        inRadius = true;
    }
}
