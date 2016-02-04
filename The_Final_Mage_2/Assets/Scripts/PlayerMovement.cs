using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{

    /// <summary>
    /// Holds the objects rigidbody (used for physics)
    /// </summary>
    Rigidbody2D rbody;

    /// <summary>
    /// The speed at which this object moves in Units/Second
    /// </summary>
    public float speed = 1.0f;


    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Gets the direction you want to move based on input
        Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //Move in the direction in speed Units/Second
        //Time.fixedDeltaTime smooths it to Units/Second instead of Units/Frame
        rbody.MovePosition(rbody.position + movement_vector * speed * Time.fixedDeltaTime);
        
    }
}
