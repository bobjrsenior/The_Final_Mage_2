using UnityEngine;
using System.Collections;

/// <summary>
/// This class is utilized in place of coroutines to allow timers to count down with a pausable time element that can be resumed when unpaused.
/// </summary>
public class Timer : MonoBehaviour {

    /// <summary>
    /// What will our timer initially start at?
    /// </summary>
    public float initialTime;

    /// <summary>
    /// What is the current time of our timer?
    /// </summary>
    public float time;

    /// <summary>
    /// Is our timer currently active or not?
    /// </summary>
    public bool started;

    /// <summary>
    /// Should we reset to the initial time?
    /// </summary>
    public bool reset;

    /// <summary>
    /// Is the timer complete?
    /// </summary>
    public bool complete;
    
    /// <summary>
    /// The basic constructor for a timer that sets up the initial time, start condition, and continuous nature of the timer. Able to be called as a method to allow usage with monoBehavior.
    /// </summary>
    /// <param name="initTime">What is the initial time for the timer? This is what it will be reset to after reaching zero each time.</param>
    /// <param name="start">Is the timer going to start as soon as it is made? True for yes, false otherwise.</param>
    /// <param name="cont">Is the timer continuous? That is, will it start again as soon as it reaches zero? True for yes, false otherwise.</param>
    public void initialize(float initTime, bool start)
    {
        initialTime = initTime;
        time = initialTime;
        started = start;
    }
    /// <summary>
    /// Sets the initial time for the timer.
    /// </summary>
    /// <param name="timeToSet">The time to set the initial timer to.</param>
    public void setTime(float timeToSet)
    {
        initialTime = timeToSet;
    }

    /// <summary>
    /// Called in an update function, this is the countdown sequence of the timer. This should be called in the update method.
    /// </summary>
    public void countdownUpdate()
    {
        complete = false;
        if (started == true)
        {
            //Counts down our timer based on game time.
            time = time - Time.deltaTime;
            //If we are on a repeat timer, we must set it back to incomplete as soon as we start counting down again.
            complete = false;
        }
        if (time <= 0)
        {
            //If our timer has reached 0, disable it, reset it, and if it is set to automatically count down again, re enable it.
            resetTimer();
            complete = true;
        }
    }

    /// <summary>
    /// Called in a update function, this is the countdown sequence of the timer. This should be called in the fixed update method.
    /// </summary>
    public void countdownFixedUpdate()
    {
        if (started == true)
        {
            //Counts down our timer based on game time.
            time = time - Time.fixedDeltaTime;
            complete = false;
        }
        if (time <= 0)
        {
            //If our timer has reached 0, disable it, reset it, an if it is set to automatically count down again, re enable it.
            complete = true;
            resetTimer();
        }
    }

    /// <summary>
    /// Resets the timer to its initial value.
    /// </summary>
    private void resetTimer()
    {
        time = initialTime;
        started = false;
    }
}
