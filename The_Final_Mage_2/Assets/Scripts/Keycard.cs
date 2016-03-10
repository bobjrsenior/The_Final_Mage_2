using UnityEngine;
using System.Collections;

public class Keycard : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            DifficultyManager.dManager.retrievedKeyCard();
            Destroy(this.gameObject);
        }
    }
}
