using UnityEngine;
using System.Collections;

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

        Vector2 temp = position;

        if (!LevelGen.gen.visit(temp))
        {
            position /= LevelGen.gen.roomDistance;

            Instantiate(roomPrefab, position, Quaternion.identity);
        }
    }
}
