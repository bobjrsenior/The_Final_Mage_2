using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().damage(PlayerAttack.pattack.rangeDamage);
            Destroy(transform.gameObject);
        }
        else if (other.gameObject.CompareTag("Walls"))
        {
            Destroy(transform.gameObject);
        }
    }
}
