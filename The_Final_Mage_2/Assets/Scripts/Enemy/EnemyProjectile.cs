using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Damage the player by the enemies standard range damage.
            PlayerHealth.pHealth.damage(DifficultyManager.dManager.enemyStandardRangeDamage);
            Destroy(transform.gameObject);
        }
        else if (other.gameObject.CompareTag("Walls"))
        {
            Destroy(transform.gameObject);
        }
    }
}
