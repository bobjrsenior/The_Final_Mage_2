using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Sets the difficulty in difficultyTracker to the appropriate difficulty on click. This allows difficultyTracker to persist beyond the options menu.
/// </summary>
public class DifficultySetterDifficultySetter : MonoBehaviour {

    public Button normalButton;
    public Button hardButton;

    public void Start()
    {
        buttonSelect();
    }

    public void setNormal()
    {
        DifficultyTracker.difficultyTrack.setNormal();
    }

    public void setHard()
    {
        DifficultyTracker.difficultyTrack.setHard();
    }

    /// <summary>
    /// Defaults to the selected button at start so the player can see which difficulty is currently set when entering the options menu.
    /// </summary>
    private void buttonSelect()
    {
        if (DifficultyTracker.difficultyTrack.getDifficulty() == 1)
        {
            normalButton.Select();
        }
        else if (DifficultyTracker.difficultyTrack.getDifficulty() == 2)
        {
            hardButton.Select();
        }
    }


}
