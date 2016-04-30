using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

    /// <summary>
    /// Sprite of an open elevator
    /// </summary>
    [SerializeField]
    private Sprite openSprite;

    /// <summary>
    /// This objects Sprite Renderer
    /// </summary>
    private SpriteRenderer renderer;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            DifficultyManager.dManager.wonFloor();
        }
    }

    public void open()
    {
        renderer.sprite = openSprite;
    }
}
