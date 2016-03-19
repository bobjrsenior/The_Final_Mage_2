using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {



    /// <summary>
    /// The point we want our camera to start at in a given level.
    /// </summary>
    public Transform initialPoint;

    public SoundScript BGM;

	// Use this for initialization
	void Start () {

        //Play the appropriate music for the last two floors.
        if (DifficultyManager.dManager.floor >= 5)
        {
            BGM.PlaySound(1);
        }
        else
        {
            BGM.PlaySound(0);
        }
        //Ensure that our camera warps to its initial position when it is first created.
        transform.position = new Vector3(3.0f, -3.0f, -10);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
