using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    void Start()
    {
        //Shrinks sprite to the appropriate size.
        transform.localScale = new Vector3(.10f, .10f, transform.localScale.z);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            print(other.gameObject.GetComponent<EnemyHealth>());
            print(PlayerAttack.pAttack.rangeDamage);
            other.gameObject.GetComponent<EnemyHealth>().damage(PlayerAttack.pAttack.rangeDamage);
            Destroy(transform.gameObject);
        }
        else if (other.gameObject.CompareTag("Walls") || other.gameObject.tag.Contains("Exit"))
        {
            Destroy(transform.gameObject);
        }
    }

}