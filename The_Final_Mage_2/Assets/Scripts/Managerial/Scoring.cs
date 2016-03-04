using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour {

    /// <summary>
    /// The score.
    /// </summary>
    public int score;

    /// <summary>
    /// The value our score starts at.
    /// </summary>
    public int initialScore;

    /// <summary>
    /// The amount of time in seconds it takes for our score to degenerate.
    /// </summary>
    public float degenerationTime;

    /// <summary>
    /// The amount that we degenerate the score by.
    /// </summary>
    public int degenerationAmount;

    /// <summary>
    /// The value that we base our score increase off of for killing a melee enemy.
    /// </summary>
    public int meleeScore;

    /// <summary>
    /// The value that we base our score increase off of for killing a ranged enemy.
    /// </summary>
    public int rangedScore;

    /// <summary>
    /// Controls our degeneration cooldown time.
    /// </summary>
    private bool degenerationCooldown;

    public static Scoring scoreKeeper;


    void Awake()
    {
        scoreKeeper = this;
    }
    // Use this for initialization
    void Start () {

        score = initialScore;
	}
	
	// Update is called once per frame
	void Update () {

        if (degenerationCooldown == false && score != 0)
        {
            StartCoroutine(degenerate());
        }
	}

    //Temporary means to track score.
    void OnGUI()
    {
        GUI.color = Color.yellow;
        GUI.Box(new Rect(0, 0, 100, 20), "Score: " + score);
    }
    private IEnumerator degenerate()
    {
        degenerationCooldown = true;
        yield return new WaitForSeconds(degenerationTime);
        if ((score - degenerationAmount) < 0)
        {
            score = 0;
        }
        else
        {
            score = score - degenerationAmount;
        }
        degenerationCooldown = false;
    }
}
