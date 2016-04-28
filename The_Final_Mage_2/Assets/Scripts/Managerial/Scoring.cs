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

    /// <summary>
    /// True will allow score to count down, false will not.
    /// </summary>
    public bool countdown;

    public bool pauseDegeneration;

    private Timer degenerationTimer;

    public static Scoring scoreKeeper;

    // Use this for initialization
    void Start () {

        if (scoreKeeper != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            scoreKeeper = this;
            DontDestroyOnLoad(this.gameObject);
            score = initialScore;
            degenerationTimer = gameObject.AddComponent<Timer>();
            degenerationTimer.initialize(degenerationTime, false);
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (degenerationCooldown == false && score != 0 && pauseDegeneration != true)
        {
            StartCoroutine(degenerate());
        }
	}

    private IEnumerator degenerate()
    {
        degenerationCooldown = true;
        degenerationTimer.started = true;
        countdown = true;
        while (degenerationTimer.complete == false)
        {
            degenerationTimer.countdownUpdate();
            //Checks to see if the player still exists or not
            if (PlayerHealth.pHealth != null)
            {
                yield return null;
            }
            else
            {
                //The player is no longer in existence, thus, the timer should resut and stop.
                degenerationTimer.started = false;
                degenerationTimer.time = degenerationTimer.initialTime;
                countdown = false;
                break;
            }
        }
        degenerationCooldown = false;
        degenerationTimer.complete = false;
        if (PlayerHealth.pHealth.health != 0 && countdown == true)
        {
            if ((score - degenerationAmount) < 0)
            {
                score = 0;
            }
            else
            {
                score = score - degenerationAmount;
            }
        }
    }
}
