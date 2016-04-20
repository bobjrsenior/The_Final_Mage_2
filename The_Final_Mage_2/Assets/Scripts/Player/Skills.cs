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

    public Button skill1Button;
    public Button skill2Button;
    public Button skill3Button;
    public Button skill4Button;

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

            up = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space) && cooldown == false)
        {
            if (up == true)
            {
                UIPanel.SetActive(true);
                skill1Button.interactable = true;
                skill2Button.interactable = true;
                skill3Button.interactable = true;
                skill4Button.interactable = true;
                up = false;
            }
            else
            {
                skill1Button.interactable = false;
                skill2Button.interactable = false;
                skill3Button.interactable = false;
                skill4Button.interactable = false;
                UIPanel.SetActive(false);
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
        if (skillID == 1)
        {
            if (skill1 == true)
            {
                skillText.text = "You have already purchased that skill!";
                TextBoxScript.textScript.showWarning();
            }
            else if (skillPoints != 0)
            {
                skillPoints--;
                skill1 = true;
                skill1Button.GetComponent<Image>().color = Color.green;
                skillText.text = "Skill purchased successfully!";
                TextBoxScript.textScript.showWarning();
            }
            else
            {
                skillText.text = "You need a skill point to purchase a skill!";
                TextBoxScript.textScript.showWarning();
            }

        }
        else if (skillID == 2)
        {
            if (skill2 == true)
            {
                skillText.text = "You have already purchased that skill!";
                TextBoxScript.textScript.showWarning();
            }
            else if(skillPoints != 0)
            {
                skillPoints--;
                skill2 = true;
                skill2Button.GetComponent<Image>().color = Color.green;
                skillText.text = "Skill purchased successfully!";
                TextBoxScript.textScript.showWarning();
            }
            else
            {
                skillText.text = "You need a skill point to purchase a skill!";
                TextBoxScript.textScript.showWarning();
            }
        }
        else if (skillID == 3)
        {
            if (skill3 == true)
            {
                skillText.text = "You have already purchased that skill!";
                TextBoxScript.textScript.showWarning();
            }
            else if (skillPoints != 0)
            {
                skillPoints--;
                skill3 = true;
                skill3Button.GetComponent<Image>().color = Color.green;
                skillText.text = "Skill purchased successfully!";
                TextBoxScript.textScript.showWarning();
            }
            else
            {
                skillText.text = "You need a skill point to purchase a skill!";
                TextBoxScript.textScript.showWarning();
            }
        }
        else if (skillID == 4)
        {
            if (skill4 == true)
            {
                skillText.text = "You have already purchased that skill!";
                TextBoxScript.textScript.showWarning();
            }
            else if (skillPoints != 0)
            {
                skillPoints--;
                skill4 = true;
                skill4Button.GetComponent<Image>().color = Color.green;
                skillText.text = "Skill purchased successfully!";
                TextBoxScript.textScript.showWarning();
            }
            else
            {
                skillText.text = "You need a skill point to purchase a skill!";
                TextBoxScript.textScript.showWarning();
            }
        }
    }
}
