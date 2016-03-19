﻿using UnityEngine;
using System.Collections;

public class Experience : MonoBehaviour {

    /// <summary>
    /// The maximum level of the player can never exceed 4.
    /// </summary>
    private const int MAX_LEVEL = 5;

    /// <summary>
    /// How much EXP does it take to reach the next level?
    /// </summary>
    public const int MAX_EXP_TO_LEVEL = 100;

    public int playerLevel = 1;

    public int playerEXP = 0;

    public static Experience playerExperience;

    void Start()
    {
        if (playerExperience != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            playerExperience = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
    }

    void OnGUI()
    {
        GUI.color = Color.yellow;
        GUI.Box(new Rect(0, 100, 100, 25), "EXP: " + playerEXP + "/" + MAX_EXP_TO_LEVEL);
        GUI.Box(new Rect(0, 120, 100, 25), "Level: " + playerLevel);
    }

    /// <summary>
    /// Adds a certain amount of EXP to the player.
    /// </summary>
    /// <param name="amount">How much EXP to add</param>
    public void addEXP(int amount)
    {
        //Only perform EXP operations if the player level is not the max level.
        if (playerLevel != MAX_LEVEL)
        {
            print("Here");
            //If we would exceep our exp cap through this level sequence...
            //If an EXP gain of this kind would boost our level to max...
            playerEXP = playerEXP + amount;
            if (playerEXP >= MAX_EXP_TO_LEVEL)
            {
                playerLevel++;
                Skills.pSkills.skillPoints++;
                if (playerLevel == MAX_LEVEL)
                {
                    playerEXP = 100;
                }
                else
                {
                    playerEXP =  playerEXP - MAX_EXP_TO_LEVEL;
                }
            }
        }
    }


}
