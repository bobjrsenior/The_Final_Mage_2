using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour {

	public string startLevel;
	public string levelSelect;
    public string optionsMenu;
    public string mainMenu;
    public string instructions;
    public GameObject difficultyTrackerPrefab;

	public void newGame ()
	{
        //If we have previously started a game already...
        if (DifficultyManager.dManager != null)
        {
            //Start on floor 1.
            DifficultyManager.dManager.floor = 1;
        }
        SceneManager.LoadScene(startLevel);
	}

    public void optionsScreen()
    {
        SceneManager.LoadScene(optionsMenu);
    }

    public void levelLoad()
    {
        SceneManager.LoadScene(levelSelect);
    }

    public void mainScreen()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void instructionsScreen()
    {
        SceneManager.LoadScene(instructions);
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
