﻿using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {

    void Start()
    {
        //Shrinks sprite to the appropriate size.
        transform.localScale = new Vector3(.10f, .10f, transform.localScale.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Damage the player by the enemies standard range damage.
            PlayerHealth.pHealth.damage(DifficultyManager.dManager.enemyStandardRangeDamage);
            Destroy(transform.gameObject);
        }
        else if (other.gameObject.CompareTag("Walls") || other.gameObject.tag.Contains("Exit"))
        {
            Destroy(transform.gameObject);
        }
    }
}
