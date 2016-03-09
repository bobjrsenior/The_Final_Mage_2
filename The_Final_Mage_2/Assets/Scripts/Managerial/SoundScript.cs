using UnityEngine;
using System.Collections;

public class SoundScript : MonoBehaviour {

    public AudioClip[] audioClip;
    public AudioSource source;


	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaySound(int clip)
    {
        source.clip = audioClip[clip];
        source.Play();
    }
}
