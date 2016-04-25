using UnityEngine;
using System.Collections;

public class Barrier : MonoBehaviour {

    /// <summary>
    /// Is this barrier active?
    /// </summary>
    private bool active = false;

    /// <summary>
    /// How much health does this barrier have left?
    /// </summary>
    private float health;

    public Timer flashTimer;

    public void Start()
    {
        flashTimer = gameObject.AddComponent<Timer>();
        flashTimer.initialize(.1f, false);
    }

    public bool isActive()
    {
        return active;
    }

    public void activate()
    {
        active = true;
        health = 3;
        gameObject.SetActive(true);
    }

    public void deactivate()
    {
        active = false;
        gameObject.SetActive(false);
    }

    public void damage(float damage)
    {
        StartCoroutine(damageFlash());
        health -= damage;
        if (--health <= 0)
        {
            deactivate();
        }
    }

    private IEnumerator damageFlash()
    {
        transform.GetComponent<SpriteRenderer>().color = Color.red;
        flashTimer.started = true;
        {
            while (flashTimer.complete == false)
            {
                flashTimer.countdownUpdate();
                yield return null;
            }
            flashTimer.complete = false;
            transform.GetComponent<SpriteRenderer>().color = Color.white;
            yield return null;
        }
    }
}
