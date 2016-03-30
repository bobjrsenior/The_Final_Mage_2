using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    Text txt;
    public static UIManager uiManager;

	// Use this for initialization
	void Start () {
        txt = GetComponent<Text>();
	
	}
	
	// Update is called once per frame
	void Update () {

        txt.text = "" + PlayerHealth.pHealth.health;
	
	}

    
}
