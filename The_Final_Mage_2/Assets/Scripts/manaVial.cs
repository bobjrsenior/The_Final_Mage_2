using UnityEngine;
using System.Collections;

public class manaVial : MonoBehaviour {

    /// <summary>
    /// How much will the vial heal by?
    /// </summary>
    public int amount;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth.pHealth.addMana(amount);
            Destroy(transform.gameObject);
        }
    }
}
