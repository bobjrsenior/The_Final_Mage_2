using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            DifficultyManager.dManager.wonFloor();
        }
    }
}
