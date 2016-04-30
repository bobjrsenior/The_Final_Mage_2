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
        sf = this;
    }

    public IEnumerator FadeToClear()//Used to allow us to yield.
    {
        isFading = true;
        if (anim == null) yield break;
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