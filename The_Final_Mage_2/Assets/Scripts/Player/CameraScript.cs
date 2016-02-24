using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    /// <summary>
    /// Will be attached to our main camera, allowing us to call camera functions on this camera.
    /// </summary>
    Camera cam;
    /// <summary>
    /// The point we want our camera to start at in a given level.
    /// </summary>
    public Transform initialPoint;

	// Use this for initialization
	void Start () {

        cam = GetComponent<Camera>();

        //Sets the camera's size to the desired size for our game.
        cam.orthographicSize = cam.orthographicSize = (Screen.height / 85f);
        //Ensure that our camera warps to its initial position when it is first created.
        transform.position = new Vector3(3.0f, -3.0f, -10);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
