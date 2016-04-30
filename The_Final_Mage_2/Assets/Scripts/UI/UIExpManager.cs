using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIExpManager : MonoBehaviour {

    public Image expbar;
	// Use this for initialization
	void Start () {
        expbar = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        //If your experience is greater than 0 and less than or equal to 10, display this exp bar, etc.
        if (Experience.playerExperience.playerEXP >= 0 && Experience.playerExperience.playerEXP <= 10)
        {
            expbar.sprite = PlayerHealth.pHealth.__exps[10];
        }
        else if (Experience.playerExperience.playerEXP >= 11 && Experience.playerExperience.playerEXP <= 20)
        {
            expbar.sprite = PlayerHealth.pHealth.__exps[9];
        }
        else if (Experience.playerExperience.playerEXP >= 21 && Experience.playerExperience.playerEXP <= 30)
        {
            expbar.sprite = PlayerHealth.pHealth.__exps[8];
        }
        else if (Experience.playerExperience.playerEXP >= 31 && Experience.playerExperience.playerEXP <= 40)
        {
            expbar.sprite = PlayerHealth.pHealth.__exps[7];
        }
        else if (Experience.playerExperience.playerEXP >= 41 && Experience.playerExperience.playerEXP <= 50)
        {
            expbar.sprite = PlayerHealth.pHealth.__exps[6];
        }
        else if (Experience.playerExperience.playerEXP >= 51 && Experience.playerExperience.playerEXP <= 60)
        {
            expbar.sprite = PlayerHealth.pHealth.__exps[5];
        }
        else if (Experience.playerExperience.playerEXP >= 61 && Experience.playerExperience.playerEXP <= 70)
        {
            expbar.sprite = PlayerHealth.pHealth.__exps[4];
        }
            else if (Experience.playerExperience.playerEXP >= 71 && Experience.playerExperience.playerEXP <= 80)
        {
            expbar.sprite = PlayerHealth.pHealth.__exps[3];
        }
            else if (Experience.playerExperience.playerEXP >= 81 && Experience.playerExperience.playerEXP <= 90)
        {
            expbar.sprite = PlayerHealth.pHealth.__exps[2];
        }
        else if (Experience.playerExperience.playerEXP >= 91 && Experience.playerExperience.playerEXP <= 100)
        {
            expbar.sprite = PlayerHealth.pHealth.__exps[0];
        }
	}
}
