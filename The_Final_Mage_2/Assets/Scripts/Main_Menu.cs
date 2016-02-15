using UnityEngine;
using System.Collections;

public class Main_Menu : MonoBehaviour {

	public string startLevel;
	public string levelSelect;

	public void newGame ()
	{
		Application.LoadLevel (startLevel);
	}

	//public void levelSelect()
//	{
//
//	}

}
