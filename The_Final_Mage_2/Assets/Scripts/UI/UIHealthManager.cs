using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIHealthManager : MonoBehaviour {

    Text txt;
    Image image; //Image to be brought from PlayerHealth
    public static UIHealthManager uiManager;

    // Use this for initialization
    void Start()
    {
        //txt = GetComponent<Text>();
        image = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {

       // txt.text = "" + PlayerHealth.pHealth.health;
        image.sprite = PlayerHealth.pHealth.images[((int)PlayerHealth.pHealth.health)]; // Get the image at index of health and convert to a sprite
       

    }
}
