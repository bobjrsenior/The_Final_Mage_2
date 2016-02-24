using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().damage(PlayerAttack.pattack.rangeDamage);
            Destroy(transform.gameObject);
        }
        else if (other.gameObject.CompareTag("Walls"))
        {
            Destroy(transform.gameObject);
        }
    }

}