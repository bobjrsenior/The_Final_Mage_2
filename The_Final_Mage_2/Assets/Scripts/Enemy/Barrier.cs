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
        health -= damage;
        if (--health <= 0)
        {
            deactivate();
        }
    }
}
