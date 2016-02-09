using UnityEngine;
using System.Collections;

public class RoomWarp : MonoBehaviour
{
    /// <summary>
    /// The target we want the player to warp to.
    /// </summary>
    public Transform warpTarget;
    /// <summary>
    /// The target we want the camera to warp to.
    /// </summary>
    public Transform cameraTarget;

    public void OnTriggerEnter2D(Collider2D other)//When a 2d collider enters our trigger zone
    {
        if (other.gameObject.name == "Player")//Only trigger the collider is attached to a game object named "Player".
        {
            other.gameObject.transform.position = warpTarget.transform.position;//Moves the player to our warp target
            Camera.main.transform.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, -10); //Moves the camera to the camera target. We add the -10 to the Z axis to move the camera back from the game field in the 3d view. 
        }
    }
}
