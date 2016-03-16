using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class scoreText : MonoBehaviour {

    /// <summary>
    /// The text that will display to show the score on the victory or loss screen.
    /// </summary>
    public Text endText;

    void Start()
    {
        endText.text = endText.text + Scoring.scoreKeeper.score;
    }
}
