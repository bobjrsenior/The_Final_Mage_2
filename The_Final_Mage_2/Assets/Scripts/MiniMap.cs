using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour {

    /// <summary>
    /// A static reference to this script
    /// </summary>
    public static MiniMap cur;

    /// <summary>
    /// The prefab for rooms in the minimap
    /// </summary>
    [SerializeField]
    private GameObject roomPrefab;

    /// <summary>
    /// A Dictionary containing every room in the minimap
    /// </summary>
    private Dictionary<Vector2, SpriteRenderer> miniMap = new Dictionary<Vector2, SpriteRenderer>();

    // Use this for initialization
    void Awake () {
        cur = this;
	}
	
    public void addRoom(Vector2 position)
    {

        //Snap position to the enclosed room
        if (position.x > 0)
        {
            position.x -= position.x % LevelGen.gen.roomDistance;
        }
        else if(position.x < 0)
        {
            position.x -= (LevelGen.gen.roomDistance - (Mathf.Abs(position.x) % LevelGen.gen.roomDistance));
        }

        if (position.y > 0)
        {
            position.y -= position.y % LevelGen.gen.roomDistance;
            position.y += LevelGen.gen.roomDistance;
        }
        else if(position.y < 0)
        {
            position.y -= (LevelGen.gen.roomDistance - (Mathf.Abs(position.y) % LevelGen.gen.roomDistance));
            position.y += LevelGen.gen.roomDistance;
        }

        

        if (!LevelGen.gen.visit(position))
        {
            position /= LevelGen.gen.roomDistance;

            SpriteRenderer renderer = (Instantiate(roomPrefab, position, Quaternion.identity) as GameObject).GetComponent<SpriteRenderer>();

            miniMap.Add(position, renderer);

            renderer.color = Color.red;

        }
        else
        {
            position /= LevelGen.gen.roomDistance;

            SpriteRenderer renderer;

            if (miniMap.TryGetValue(position, out renderer))
            {
                renderer.color = Color.red;
            }
        }
        print(position);

    }

    public void leaveRoom(Vector2 position)
    {
        //Snap position to the enclosed room
        if (position.x > 0)
        {
            position.x -= position.x % LevelGen.gen.roomDistance;
        }
        else if (position.x < 0)
        {
            position.x -= (LevelGen.gen.roomDistance - (Mathf.Abs(position.x) % LevelGen.gen.roomDistance));
        }

        if (position.y > 0)
        {
            position.y -= position.y % LevelGen.gen.roomDistance;
            position.y += LevelGen.gen.roomDistance;
        }
        else if (position.y < 0)
        {
            position.y -= (LevelGen.gen.roomDistance - (Mathf.Abs(position.y) % LevelGen.gen.roomDistance));
            position.y += LevelGen.gen.roomDistance;
        }

        position /= LevelGen.gen.roomDistance;

        print(position);

        SpriteRenderer renderer;

        if (miniMap.TryGetValue(position, out renderer))
        {
            renderer.color = Color.white;
        }
        else
        {
            print("Failed to find room in minimap");
        }
    }
}
