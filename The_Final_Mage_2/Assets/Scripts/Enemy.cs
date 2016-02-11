﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    /// <summary>
    /// Whether or not the enemy attached to this script is in radius of the players detection zone.
    /// </summary>
    public bool inRadius = false;

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
