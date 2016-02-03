using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rbody;
    public float speed = 1f;


    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));//Gets our X and Y axis from raw input

        rbody.MovePosition(rbody.position + movement_vector * speed * Time.deltaTime);
    }
}
