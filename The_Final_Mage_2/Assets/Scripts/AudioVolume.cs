using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioVolume : MonoBehaviour {

    /// <summary>
    /// Audio mixer that contains the master volume
    /// </summary>
    [SerializeField]
    private AudioMixer audioMixer;

    /// <summary>
    /// Slider UI object
    /// </summary>
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();

        //Get the current value and update the slider
        float temp;

        audioMixer.GetFloat("volume", out temp);

        slider.value = temp + 80;
    }

    /// <summary>
    /// Updates the master volume to to match the slider value
    /// </summary>
	public void updateVolume()
    {
        audioMixer.SetFloat("volume", slider.value - 80);
    }
}
