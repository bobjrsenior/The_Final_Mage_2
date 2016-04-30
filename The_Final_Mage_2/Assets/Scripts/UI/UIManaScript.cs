using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManaScript : MonoBehaviour {
    public Text manaText;
	// Use this for initialization
	void Start () {
        manaText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        manaText.text = PlayerHealth.pHealth.mana.ToString();
	}
}
