﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextBoxScript : MonoBehaviour {

    private Timer displayTimer;

    public string[] gameScript;

    public Text textbox;

    public static TextBoxScript textScript;

    public GameObject textPanel;

    public bool displaying;

    public bool displayingWarning;

    public bool finalLevel;

    public string[] finalBossScript
        ;
    public bool finalBossDisplayed;

    private int currentMessagePosition;

	// Use this for initialization
	void Start () {
	    if (textScript != null)
        {
            Destroy(transform.root.gameObject);
        }
        else
        {
            textScript = this;
            textPanel.SetActive(false);
            displaying = false;
            DontDestroyOnLoad(transform.root.gameObject);
            displayTimer = gameObject.AddComponent<Timer>();
            displayTimer.initialize(5, false);
            shuffleScript();
        }
	}
	
    //Shuffles the script so it wont be the same for the player every damn time.
    private void shuffleScript()
    {
        // Loops through array
        for (int i = gameScript.Length - 1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            int rnd = Random.Range(0, i);

            // Save the value of the current i, otherwise it'll overright when we swap the values
            string temp = gameScript[i];

        // Swap the new and old values
            gameScript[i] = gameScript[rnd];
            gameScript[rnd] = temp;
        }
    }

    public IEnumerator displayWarning()
    {
        displayTimer.started = true;
        while (displayTimer.complete == false)
        {
            displayTimer.countdownUpdate();
            yield return null;
        }
        displayTimer.complete = false;
        //Move the message off the screen.
        textPanel.SetActive(false);
        displaying = false;
        displayingWarning = false;
        yield break;
    }

    public IEnumerator displayMessage()
    {
        //Only show if we are not already displaying.
        if (currentMessagePosition > (gameScript.Length - 1))
        {
            yield break;
        }

        if (displayTimer.started == false)
        {
            //Move the text panel pack into position.
            textPanel.SetActive(true);
            displaying = true;
            //Show the current message.
            textbox.text = gameScript[currentMessagePosition];
            displayTimer.started = true;
            while (displayTimer.complete == false)
            {
                displayTimer.countdownUpdate();
                yield return null;
            }
            displayTimer.complete = false;
            //Move the message off the screen.
            textPanel.SetActive(false);
            displaying = false;
            //Move the message position up by one.
            currentMessagePosition++;
            yield break;
        }
    }

    public void showTextbox()
    {
        //If the textbox is already active...
        if (displayTimer.started == true)
        {
            //Stop it.
            displayTimer.complete = true;
        }

        //Show text box.
        displaying = true;
        textPanel.SetActive(true);
    }

    public void hideTextbox()
    {
        //If the textbox is already active...
        if (displayTimer.started == true)
        {
            //Stop it.
            displayTimer.complete = true;
        }

        //Hide text box only if it is being displayed already.
        if (displaying == true)
        {
            textPanel.SetActive(false);
            displaying = false;
        }
    }

    public void showWarning()
    {
        if (displaying == true && displayingWarning == false)
        {
            displayTimer.time = 2;
            displayingWarning = true;
            StartCoroutine(displayWarning());
        }
        else if (displayingWarning == false)
        {
            //Show text box.
            displaying = true;
            textPanel.SetActive(true);
            displayTimer.time = 2;
            displayingWarning = true;
            StartCoroutine(displayWarning());
        }
        else if (displayingWarning == true)
        {
            displayTimer.time = 2;
        }
    }

    public IEnumerator finalBossSequence()
    {
        Debug.Log("There");
        for (int currPosition = 0; currPosition <= (finalBossScript.Length - 1); currPosition++)
        {
            Debug.Log("Here");
            if (displayTimer.started == false)
            {
                //Move the text panel pack into position.
                textPanel.SetActive(true);
                displaying = true;
                //Show the current message.
                textbox.text = finalBossScript[currPosition];
                displayTimer.started = true;
                while (displayTimer.complete == false)
                {
                    displayTimer.countdownUpdate();
                    yield return null;
                }
                displayTimer.complete = false;
            }
        }
        //Move the message off the screen.
        textPanel.SetActive(false);
        displaying = false;
        finalBossDisplayed = true;
    }
}

