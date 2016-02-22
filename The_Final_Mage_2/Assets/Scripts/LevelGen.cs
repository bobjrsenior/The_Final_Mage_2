using UnityEngine;
using System.Collections.Generic;

public class LevelGen : MonoBehaviour {

    private int level = 1;

    private int difficulty = 1;

    public GameObject[] rooms;

    private int defaultMaxSize = 15;

    private int curMaxSize;

	// Use this for initialization
	void Start () {
        generateLevel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void generateLevel()
    {
        curMaxSize =(int) (defaultMaxSize + (defaultMaxSize * 0.5f * (difficulty * level)));
        Dictionary<Vector2, Room> map = new Dictionary<Vector2, Room>();
        Room room1 = new Room();
        room1.setPosition(Vector2.zero);
        map.Add(room1.position, room1);
        int totalRooms = 1;
        for(int e = 0; e < 4; ++e)
        {
            if(Random.Range(0.0f, 10.0f) > 5.0f)
            {
                room1.addDoor(e);
                totalRooms = addNode(map, room1.position, e, totalRooms);
            }
        }
        if(totalRooms == 1)
        {
            room1.addDoor(0);
        }
        spawnLevel(map, room1);
    }

    private void spawnLevel(Dictionary<Vector2, Room> map, Room room)
    {
        print(room.position);
        map.Remove(room.position);
        Instantiate(rooms[room.getPrefabIndex()], room.position, Quaternion.identity);
        for(int e = 0; e < 4; ++e)
        {
            if (room.doors[e])
            {
                Room nextRoom;
                if (map.TryGetValue(dirToPos(room.position, e), out nextRoom))
                {
                    spawnLevel(map, nextRoom);
                }
            }
        }
    }

    private int addNode(Dictionary<Vector2, Room> map, Vector2 oldPos, int direction, int totalRooms)
    {
        ++totalRooms;
        Room room;
        Vector2 position = dirToPos(oldPos, direction);
        if(!map.TryGetValue(position, out room))
        {
            room = new Room();
            room.setPosition(position);
            map.Add(room.position, room);
        }
        room.addDoor(oppositeDirection(direction));
        if(totalRooms > curMaxSize)
        {
            return totalRooms;
        }
        for (int e = 0; e < 4; ++e)
        {
            if (!room.doors[e])
            {
                if(Random.Range(Mathf.Log(difficulty * level), 10.0f) > 5.0f)
                {
                    room.addDoor(e);
                    totalRooms = addNode(map, room.position, e, totalRooms);
                    if (totalRooms > curMaxSize)
                    {
                        return totalRooms;
                    }
                }
            }
        }

        return totalRooms;
    }

    private Vector2 dirToPos(Vector2 oldPos, int direction)
    {
        if(direction == 0)
        {
            oldPos.y += 15;
        }
        else if(direction == 1)
        {
            oldPos.y -= 15;
        }
        else if(direction == 2)
        {
            oldPos.x -= 15;
        }
        else
        {
            oldPos.x += 15;
        }
        return oldPos;
    }

    private int oppositeDirection(int direction)
    {
        if (direction == 0)
        {
            return 1;
        }
        else if (direction == 1)
        {
            return 0;
        }
        else if (direction == 2)
        {
            return 3;
        }
        else
        {
            return 2;
        }
    }
}

public class Room
{
    /// <summary>
    /// Which sides of the room has a door
    /// [Up, Down, Left, Right]
    /// </summary>
    public bool[] doors = { false, false, false, false };

    public Vector2 position;

    public void setPosition(Vector2 position)
    {
        this.position = position;
    }

    public void addDoor(int direction)
    {
        doors[direction] = true;
    }

    public int getPrefabIndex()
    {
        return 0;
    }
}
