using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFloorScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Text>().text = "Floor:\n" + DifficultyManager.dManager.floor;
    }
}
