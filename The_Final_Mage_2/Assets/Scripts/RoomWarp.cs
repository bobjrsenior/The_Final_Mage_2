using UnityEngine;
using System.Collections;

public class RoomWarp : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D other)//When a 2d collider enters our trigger zone
    {
        if (other.CompareTag("Player"))//Only trigger the collider is attached to a game object named "Player".
        {
            if (other.isTrigger == false)//If the collider is not a trigger (Or, in simpler terms, if we are detecting player collision and not the radius that governs whether enemies can move or not)
            {

                print("Hit Exit");
                //Top door
                if(this.CompareTag("Up_Exit"))
                {
                    other.transform.Translate(0.0f, 15.0f - 5.0f, 0.0f);
                    Camera.main.transform.Translate(0.0f, 15.0f, 0.0f);
                }//Bottom door
                else if(this.CompareTag("Down_Exit"))
                {
                    other.transform.Translate(0.0f, -15.0f + 5.0f, 0.0f);
                    Camera.main.transform.Translate(0.0f, -15.0f, 0.0f);
                }//Right door
                else if(this.CompareTag("Left_Exit"))
                {
                    other.transform.Translate(-15.0f + 5.0f, 0.0f, 0.0f);
                    Camera.main.transform.Translate(-15.0f, 0.0f, 0.0f);
                }//Left door
                else
                {
                    other.transform.Translate(15.0f - 5.0f, 0.0f, 0.0f);
                    Camera.main.transform.Translate(15.0f, 0.0f, 0.0f);
                }
            }
        }
    }
}
