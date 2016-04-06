using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    public GameObject UIPanel;

    public Text skillText;
    public Timer UIDropdownCool;

    private bool cooldown;

    //True if and only if the UI is up.
    private bool up;

    public static Skills pSkills;

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
            UIDropdownCool = gameObject.AddComponent<Timer>();
            UIDropdownCool.initialize(.2f, false);

            UIPanel.transform.position = new Vector2(UIPanel.transform.position.x, UIPanel.transform.position.y + 2750);
            up = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space) && cooldown == false)
        {
            if (up == true)
            {
                UIPanel.transform.position = new Vector2(UIPanel.transform.position.x, UIPanel.transform.position.y - 2750);
                skillText.text = "Select a skill button to purchase that skill.";
                up = false;
            }
            else
            {
                UIPanel.transform.position = new Vector2(UIPanel.transform.position.x, UIPanel.transform.position.y + 2750);
                skillText.text = "Press space bar to spend skill points.";
                up = true;
            }
        }
	}

    private IEnumerable dropdownCoolTimer()
    {
        cooldown = true;
        UIDropdownCool.started = true;
        while (UIDropdownCool.complete == false)
        {
            UIDropdownCool.countdownUpdate();
            yield return null;
        }
        UIDropdownCool.complete = false;
        cooldown = false;
        yield break;
    }
    /// <summary>
    /// Spends skillpoints to unlock a tageted skill.
    /// </summary>
    /// <param name="skillID">the ID number of the skil that we wish to obtain by spending this point.</param>
    public void spend(int skillID)
    {
        if (skillPoints != 0)
        {
            if (skillID == 1)
            {
                if (skill1 == true)
                {
                    skillText.text = "You have already purchased that skill!";
                }
                else
                {
                    skillPoints--;
                    skill1 = true;
                    skillText.text = "Skill purchased successfully!";
                }
                
            }
            else if (skillID == 2)
            {
                if (skill2 == true)
                {
                    skillText.text = "You have already purchased that skill!";
                }
                else
                {
                    skillPoints--;
                    skill2 = true;
                    skillText.text = "Skill purchased successfully!";
                }
            }
            else if (skillID == 3)
            {
                if (skill3 == true)
                {
                    skillText.text = "You have already purchased that skill!";
                }
                else
                {
                    skillPoints--;
                    skill3 = true;
                    skillText.text = "Skill purchased successfully!";
                }
            }
            else if (skillID == 4)
            {
                if (skill4 == true)
                {
                    skillText.text = "You have already purchased that skill!";
                }
                else
                {
                    skillPoints--;
                    skill4 = true;
                    skillText.text = "Skill purchased successfully!";
                }
            }
            else
            {
                print("SKILL SPENDING FAILED");
            }
        }
        else
        {
            skillText.text = "You need a skill point to purchase a skill!";
        }
    }
}
