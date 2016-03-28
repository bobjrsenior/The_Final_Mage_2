using UnityEngine;
using System.Collections;

public class Keycard : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            DifficultyManager.dManager.retrievedKeyCard();
            Destroy(this.gameObject);
        }
    }
}
