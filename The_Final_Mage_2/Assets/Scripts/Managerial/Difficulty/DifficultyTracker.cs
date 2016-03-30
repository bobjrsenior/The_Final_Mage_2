using UnityEngine;
using System.Collections;

public class DifficultyTracker : MonoBehaviour {

    public int difficulty;

    public static DifficultyTracker difficultyTrack;
	// Use this for initialization
	void Start () {

        if (difficultyTrack != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            difficultyTrack = this;
            DontDestroyOnLoad(this.gameObject);
            //default to normal
            difficulty = 1;
        }
        
	}
	
    public int getDifficulty()
    {
        return difficulty;
    }

    public void setNormal()
    {
        difficulty = 1;
    }

    public void setHard()
    {
        difficulty = 2;
    }
}
