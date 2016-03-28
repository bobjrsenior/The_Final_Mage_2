using UnityEngine;
using System.Collections;

public class SoundScript : MonoBehaviour {

    public AudioClip[] audioClip;
    public AudioSource source;

    public static bool exists;

	// Use this for initialization
	void Start () {

        if (exists == false)
        {
            DontDestroyOnLoad(this.gameObject);
            source = GetComponent<AudioSource>();
            exists = true;
        }
        else
        {
            Destroy(this.gameObject);
        }

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaySound(int clip)
    {
        source.clip = audioClip[clip];
        source.PlayOneShot(audioClip[clip]);
    }
}
