using UnityEngine;
using System.Collections;

public class Keycard : MonoBehaviour {

    /// <summary>
    /// This floor's elevator
    /// </summary>
    private Elevator elevator;

    void Awake()
    {
        elevator = GameObject.FindObjectOfType<Elevator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            DifficultyManager.dManager.retrievedKeyCard();
            elevator.open();
            Destroy(this.gameObject);
        }
    }
}
