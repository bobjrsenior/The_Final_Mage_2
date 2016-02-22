using UnityEngine;
using System.Collections.Generic;

public class LevelGen : MonoBehaviour {

    /// <summary>
    /// The current level
    /// </summary>
    private int level = 1;

    /// <summary>
    /// The current difficulty
    /// </summary>
    private int difficulty = 1;

    /// <summary>
    /// The distance between rooms
    /// </summary>
    private float roomDistance = 15.0f;

    /// <summary>
    /// An array of all the room prefabs that can spawn
    /// TODO: Standardize list order
    /// </summary>
    public GameObject[] rooms;

    /// <summary>
    /// Default room size limit
    /// </summary>
    private int defaultMaxSize = 15;

    /// <summary>
    /// Current room size limit
    /// </summary>
    private int curMaxSize;

	// Use this for initialization
	void Start () {
        generateLevel();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Generates a new level based on the difficulty and level number
    /// Then calls spawnLevel to actually insantiate it
    /// </summary>
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

    /// <summary>
    /// Take a map of a level and a current room, then, recursivly spawns the level
    /// </summary>
    /// <param name="map">Dictionary of every room on the map</param>
    /// <param name="room">The current room to spawn</param>
    private void spawnLevel(Dictionary<Vector2, Room> map, Room room)
    {
        //Remove the current room from the map to avoid duplicate spawning
        map.Remove(room.position);
        //Create the rrom
        Instantiate(rooms[room.getPrefabIndex()], room.position, Quaternion.identity);
        //Go through each door for the room
        for(int e = 0; e < 4; ++e)
        {
            //If there is a door in that direction and the corresponding room hasn't already been spawned
            if (room.doors[e])
            {
                Room nextRoom;
                if (map.TryGetValue(dirToPos(room.position, e), out nextRoom))
                {
                    //Spawn corresponding room
                    spawnLevel(map, nextRoom);
                }
            }
        }
    }

    /// <summary>
    /// Takes a map of the level and adds a new room based on a door from an old room
    /// Then sees if the new room wants any doors and spawns those rooms as well
    /// </summary>
    /// <param name="map">Dictionary of every room on the map</param>
    /// <param name="oldPos">Position of the adjacent room spawning this one</param>
    /// <param name="direction">Which door of the adjacent room points to this one</param>
    /// <param name="totalRooms">The cuurent number of rooms on the map (used to prevent overspawn)</param>
    /// <returns>The current total number of rooms on the map</returns>
    private int addNode(Dictionary<Vector2, Room> map, Vector2 oldPos, int direction, int totalRooms)
    {
       
        Room room;
        //Get the new position
        Vector2 position = dirToPos(oldPos, direction);

        //If this room already exists, get it
        if(!map.TryGetValue(position, out room))
        {
            //Otherwise, create a new one and add it to the map
            ++totalRooms;
            room = new Room();
            room.setPosition(position);
            map.Add(room.position, room);
        }
        else
        {
            //Add the door to the adjacent room creating this one
            room.addDoor(oppositeDirection(direction));

            //If the room already exists, then we have already checke
            return totalRooms;
        }

        //Add the door to the adjacent room creating this one
        room.addDoor(oppositeDirection(direction));
        //Check if we are at the room limit
        if(totalRooms > curMaxSize)
        {
            return totalRooms;
        }

        //How many doors this room has
        int doorCount = 0;

        //Go through each door for the room
        for (int e = 0; e < 4; ++e)
        {
            //If a door here does not already exist
            if (!room.doors[e])
            {
                //Check to see if this room wants another door based on an algorithm
                if(Random.Range(Mathf.Log(difficulty * level), 10.0f + (0.5f * doorCount)) > 5.0f)
                {
                    ++doorCount;
                    //If it wants another door, create on and a corresponding room
                    room.addDoor(e);
                    totalRooms = addNode(map, room.position, e, totalRooms);

                    //Check that the new room didn't make us reach the room limit
                    if (totalRooms > curMaxSize)
                    {
                        return totalRooms;
                    }
                }
            }
            else
            {
                ++doorCount;
            }
        }

        return totalRooms;
    }

    /// <summary>
    /// Takes a position and direction and find the adjacent position in that direction
    /// </summary>
    /// <param name="oldPos">The origin</param>
    /// <param name="direction">Which way you want to travel</param>
    /// <returns>The new position</returns>
    private Vector2 dirToPos(Vector2 oldPos, int direction)
    {
        if(direction == 0)
        {
            oldPos.y += roomDistance;
        }
        else if(direction == 1)
        {
            oldPos.y -= roomDistance;
        }
        else if(direction == 2)
        {
            oldPos.x -= roomDistance;
        }
        else
        {
            oldPos.x += roomDistance;
        }
        return oldPos;
    }

    /// <summary>
    /// Takes a direction and flips is (ex: up->down, left->right)
    /// </summary>
    /// <param name="direction">The direction to flip</param>
    /// <returns>The flipped directopm</returns>
    private int oppositeDirection(int direction)
    {
        //Directions:
        //0: Up
        //1: Down
        //2: Left
        //3: Right

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


/// <summary>
/// Class to hold information about rooms while generating a map
/// </summary>
public class Room
{
    /// <summary>
    /// Which sides of the room has a door
    /// [Up, Down, Left, Right]
    /// </summary>
    public bool[] doors = { false, false, false, false };

    /// <summary>
    /// Where the room is on the map
    /// </summary>
    public Vector2 position;

    /// <summary>
    /// Set the rooms position
    /// </summary>
    /// <param name="position">Position of the room</param>
    public void setPosition(Vector2 position)
    {
        this.position = position;
    }

    /// <summary>
    /// Adds a door to the given direction
    /// </summary>
    /// <param name="direction">Direction of the door</param>
    public void addDoor(int direction)
    {
        doors[direction] = true;
    }

    /// <summary>
    /// Converts this room into it's correpsonding index in the room prefab array
    /// </summary>
    /// <returns>The index that corresponds to gthis room in the room prefab array</returns>
    public int getPrefabIndex()
    {
        //Placeholder
        return 0;
    }
}
