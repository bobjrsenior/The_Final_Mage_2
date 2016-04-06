using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UILevel : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Text>().text = "Level: " + Experience.playerExperience.playerLevel;
	}
}
