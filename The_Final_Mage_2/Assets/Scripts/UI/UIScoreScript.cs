using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScoreScript : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Text>().text = "Score: " + Scoring.scoreKeeper.score;
	}
}
