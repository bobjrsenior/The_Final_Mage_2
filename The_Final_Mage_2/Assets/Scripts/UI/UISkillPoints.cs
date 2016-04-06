using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISkillPoints : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Text>().text = "Skill: " + Skills.pSkills.skillPoints;
	}
}
