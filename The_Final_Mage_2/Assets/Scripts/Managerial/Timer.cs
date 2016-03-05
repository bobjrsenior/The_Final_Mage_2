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
    /// Should the timer repeat automatically after it has run?
    /// </summary>
    public bool continuous;

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
        if (started == true)
        {
            //Counts down our timer based on game time.
            time = time - Time.deltaTime;
        }
        if (time <= 0)
        {
            //If our timer has reached 0, disable it, reset it, an if it is set to automatically count down again, re enable it.
            resetTimer();
            if (continuous == true)
            {
                enable();
            }
            else
            {
                disable();
            }
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
        }
        if (time <= 0)
        {
            //If our timer has reached 0, disable it, reset it, an if it is set to automatically count down again, re enable it.
            resetTimer();
            if (continuous == true)
            {
                enable();
            }
            else
            {
                disable();
            }
        }
    }

    /// <summary>
    /// Start the timer.
    /// </summary>
    public void enable()
    {
        started = true;
    }

    /// <summary>
    /// Stop the timer. Saves its current time.
    /// </summary>
    public void disable()
    {
        started = false;
    }

    /// <summary>
    /// Sets this as a continuous timer that will run over and over.
    /// </summary>
    public void setContinuous()
    {
        continuous = true;
    }

    /// <summary>
    /// Resets the timer to its initial value.
    /// </summary>
    public void resetTimer()
    {
        time = initialTime;
    }

    /// <summary>
    /// Returns the current time.
    /// </summary>
    /// <returns>Returns the current time.</returns>
    public float getCurrentTime()
    {
        return time;
    }
}
