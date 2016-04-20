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
    /// 
    /// </summary>
    private Dictionary<Vector2, SpriteRenderer> miniMap = new Dictionary<Vector2, SpriteRenderer>();

    // Use this for initialization
    void Awake () {
        cur = this;
	}
	
    public void addRoom(Vector3 position)
    {
        print(position);

        position.z = 0;

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

        print(position);

        if (!LevelGen.gen.visit(position))
        {
            position /= LevelGen.gen.roomDistance;

            miniMap.Add(position, (Instantiate(roomPrefab, position, Quaternion.identity) as GameObject).GetComponent<SpriteRenderer>());
        }
        else
        {

        }
    }
}
