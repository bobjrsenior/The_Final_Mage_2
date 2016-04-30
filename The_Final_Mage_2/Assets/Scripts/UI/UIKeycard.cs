using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIKeycard : MonoBehaviour {

    public Image keycardImage;

    void Start()
    {
        keycardImage.enabled = false;
    }

    void OnGUI()
    {
        if (DifficultyManager.dManager.gotKeyCard)
        {
            keycardImage.enabled = true;
        }
        else keycardImage.enabled = false;
    }
}
