using UnityEngine;
using System.Collections;

public class DebugUtility : MonoBehaviour {

    /// <summary>
    /// Whether or not debugging is enabled.
    /// </summary>
    public bool enable;
	// Use this for initialization

    public void Log(string message)
    {
        if (enable == true)
        {
            Debug.Log(message);
        }
    }
}
