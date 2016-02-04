using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroText : MonoBehaviour {

    /// <summary>
    /// The rate at which the text scroll in Units/Second
    /// </summary>
    public float scrollSpeed = 50.0f;

	// Use this for initialization
	void Start () {
        print(transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        //Move the text up
        transform.Translate(0, scrollSpeed * Time.deltaTime, 0);

        //If it is off the screen, load next scene (3050 was found by moving the text up and finding it's y position)
        if(transform.position.y > 3050)
        {
            SceneManager.LoadScene("Test");
        }
	}
}
