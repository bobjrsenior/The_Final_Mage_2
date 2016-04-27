using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{

    Animator anim;
    bool isFading = false;
    public static ScreenFader sf;

    // Use this for initialization
    void Start()
    {

        anim = GetComponent<Animator>();
        if (sf == null) //If the player does not already exist
        {
            sf = this;
            DontDestroyOnLoad(transform.root.gameObject); //When we load a new area, we will not destroy the object this is attached to (the player)
        }
        else//This will prevent duplicate objects when loading into a new area.
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator FadeToClear()//Used to allow us to yield.
    {
        isFading = true;
        anim.SetBool("isFading", true);
        anim.SetTrigger("fadeIn");

        while (isFading)
            yield return null;//This will not return until isFading goes back to false.

    }

    public IEnumerator FadeToBlack()
    {
        isFading = true;
        anim.SetBool("isFading", true);
        anim.SetTrigger("fadeOut");

        while (isFading)
            yield return null;//This will not return until isFading goes back to false.

    }

    private void AnimationComplete()
    {
        isFading = false;
        anim.SetBool("isFading", false);
    }
}