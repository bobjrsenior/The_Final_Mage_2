using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {



    /// <summary>
    /// The point we want our camera to start at in a given level.
    /// </summary>
    public Transform initialPoint;

	// Use this for initialization
	void Start () {

        //Ensure that our camera warps to its initial position when it is first created.
        transform.position = new Vector3(3.0f, -3.0f, -10);

        if (DifficultyManager.dManager.floor >= 5)
        {
            GetComponent<AudioSource>().clip = FindObjectOfType<SoundScript>().audioClip[4];
            GetComponent<AudioSource>().Play();
        }
        else
        {
            GetComponent<AudioSource>().clip = FindObjectOfType<SoundScript>().audioClip[3];
            GetComponent<AudioSource>().Play();
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
