using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RoomWarp : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D coll)
    {
        
        if (coll.gameObject.CompareTag("Player"))//Only trigger the collider is attached to a game object named "Player".
        {
            //Get the other objects transform component
            Transform other = coll.transform;
            
            //Get the distance between rooms
            float roomDistance;

            if (coll.collider.isTrigger == false)//If the collider is not a trigger (Or, in simpler terms, if we are detecting player collision and not the radius that governs whether enemies can move or not)
            {
                //If not on levelgen, proceed as usual
                if (!SceneManager.GetActiveScene().name.Equals("LevelGen"))
                {
                    roomDistance = 15.0f;

                    print("Hit Exit");
                    //Top door
                    if (this.CompareTag("Up_Exit"))
                    {
                        other.transform.Translate(0.0f, roomDistance - 5.0f, 0.0f);
                        Camera.main.transform.Translate(0.0f, roomDistance, 0.0f);
                    }//Bottom door
                    else if (this.CompareTag("Down_Exit"))
                    {
                        other.transform.Translate(0.0f, -roomDistance + 5.0f, 0.0f);
                        Camera.main.transform.Translate(0.0f, -roomDistance, 0.0f);
                    }//Right door
                    else if (this.CompareTag("Left_Exit"))
                    {
                        other.transform.Translate(-roomDistance + 5.0f, 0.0f, 0.0f);
                        Camera.main.transform.Translate(-roomDistance, 0.0f, 0.0f);
                    }//Left door
                    else
                    {
                        other.transform.Translate(roomDistance - 5.0f, 0.0f, 0.0f);
                        Camera.main.transform.Translate(roomDistance, 0.0f, 0.0f);
                    }
                }//If in LevelGen
                else
                {
                    roomDistance = LevelGen.gen.roomDistance;

                    if (LevelGen.gen.unlocked(transform.position))
                    {
                        //Top door
                        if (this.CompareTag("Up_Exit"))
                        {
                            other.transform.Translate(0.0f, roomDistance - 5.0f, 0.0f);
                            Camera.main.transform.Translate(0.0f, roomDistance, 0.0f);
                        }//Bottom door
                        else if (this.CompareTag("Down_Exit"))
                        {
                            other.transform.Translate(0.0f, -roomDistance + 5.0f, 0.0f);
                            Camera.main.transform.Translate(0.0f, -roomDistance, 0.0f);
                        }//Right door
                        else if (this.CompareTag("Left_Exit"))
                        {
                            other.transform.Translate(-roomDistance + 5.0f, 0.0f, 0.0f);
                            Camera.main.transform.Translate(-roomDistance, 0.0f, 0.0f);
                        }//Left door
                        else
                        {
                            other.transform.Translate(roomDistance - 5.0f, 0.0f, 0.0f);
                            Camera.main.transform.Translate(roomDistance, 0.0f, 0.0f);
                        }
                    }
                }
            }
        }
    }
}
