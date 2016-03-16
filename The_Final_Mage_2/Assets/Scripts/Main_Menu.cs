using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour {

	public string startLevel;
	public string levelSelect;

	public void newGame ()
	{
        SceneManager.LoadScene(startLevel);
	}

    public void levelLoad()
    {
        SceneManager.LoadScene(levelSelect);
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
