using UnityEngine;
using System.Collections;

public class Skills : MonoBehaviour {

    public const int MAX_SKILL_POINTS = 4;

    /// <summary>
    /// The current number of skill points the player possesses.
    /// </summary>
    public int skillPoints;

    /// <summary>
    /// Boolean representations of our four skills. True for unlocked, false otherwise.
    /// </summary>
    public bool skill1;
    public bool skill2;
    public bool skill3;
    public bool skill4;

    public static Skills pSkills;

    void OnGUI()
    {
        GUI.color = Color.yellow;
        GUI.Box(new Rect(0, 140, 100, 25), "Skill points:" + skillPoints);
    }

	// Use this for initialization
	void Start () {
        if (pSkills != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            pSkills = this;
            DontDestroyOnLoad(transform.root.gameObject);
            skillPoints = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    /// <summary>
    /// Spends skillpoints to unlock a tageted skill.
    /// </summary>
    /// <param name="skillID">the ID number of the skil that we wish to obtain by spending this point.</param>
    public void spend(int skillID)
    {
        if (skillPoints != 0)
        {
            skillPoints--;
            if (skillID == 1)
            {
                skill1 = true;
            }
            else if (skillID == 2)
            {
                skill2 = true;
            }
            else if (skillID == 3)
            {
                skill3 = true;
            }
            else if (skillID == 4)
            {
                skill4 = true;
            }
            else
            {
                print("SKILL SPENDING FAILED");
                skillPoints++;
            }
        }
    }
}
