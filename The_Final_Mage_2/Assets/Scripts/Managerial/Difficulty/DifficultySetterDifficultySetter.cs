using UnityEngine;
using System.Collections;

/// <summary>
/// Sets the difficulty in difficultyTracker to the appropriate difficulty on click. This allows difficultyTracker to persist beyond the options menu.
/// </summary>
public class DifficultySetterDifficultySetter : MonoBehaviour {

    public void setNormal()
    {
        DifficultyTracker.difficultyTrack.setNormal();
    }

    public void setHard()
    {
        DifficultyTracker.difficultyTrack.setHard();
    }
}
