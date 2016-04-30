using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFloorScript : MonoBehaviour {

    public static UIFloorScript UIFloor;
	// Use this for initialization
	void Start () {
        UIFloor = this;
        gameObject.GetComponent<Text>().text = "Floor:\n" + DifficultyManager.dManager.floor;
    }
}
