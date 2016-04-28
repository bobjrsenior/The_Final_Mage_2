using UnityEngine;
using System.Collections;

public class finalBossCamera : MonoBehaviour {

    public GameObject target;

    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        GetComponent<AudioSource>().clip = FindObjectOfType<SoundScript>().audioClip[4];
        GetComponent<AudioSource>().Play();
    }
    public void Update()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
        }
    }
}
