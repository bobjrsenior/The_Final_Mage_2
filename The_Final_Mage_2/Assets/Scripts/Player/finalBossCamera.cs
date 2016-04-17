using UnityEngine;
using System.Collections;

public class finalBossCamera : MonoBehaviour {

    public GameObject target;

    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
    public void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
        }
    }
}
