using UnityEngine;
using System.Collections.Generic;

public class LevelGen : MonoBehaviour {

    /// <summary>
    /// The current level generator
    /// </summary>
    public static LevelGen gen;

    /// <summary>
    /// References the levels game manager
    /// </summary>
    public DifficultyManager manager;

    /// <summary>
    /// Prefab of the enemy object
    /// </summary>
    public GameObject enemyPrefab;

    /// <summary>
    /// The player's prefab gameobject
    /// </summary>
    public GameObject playerPrefab;

    /// <summary>
    /// The camera's prefab gameobject
    /// </summary>
    public GameObject cameraPrefab;

    /// <summary>
    /// An array of all the room prefabs that can spawn
    /// TODO: Standardize list order
    /// </summary>
    public GameObject[] rooms;

    /// <summary>
    /// A map of the current floor
    /// </summary>
    private Dictionary<Vector2, Room> map = new Dictionary<Vector2, Room>();

    /// <summary>
    /// The distance between rooms
    /// </summary>
    private float roomDistance = 15.0f;

    /// <summary>
    /// Default room size limit
    /// </summary>
    private int defaultMaxSize = 15;

    /// <summary>
    /// Current room size limit
    /// </summary>
    private int curMaxSize;

    /// <summary>
    /// Corresponds to the up direction in level generation
    /// </summary>
    const int UP = 0;

    /// <summary>
    /// Corresponds to the down direction in level generation
    /// </summary>
    const int DOWN = 1;

    /// <summary>
    /// Corresponds to the left direction in level generation
    /// </summary>
    const int LEFT = 2;

    /// <summary>
    /// Corresponds to the right direction in level generation
    /// </summary>
    const int RIGHT = 3;


    // Use this for initialization
    void Awake () {
        if(gen != null)
        {
            Destroy(this.gameObject);
            return;
        }
        gen = this;

	}

    void OnApplicationQuit()
    {
        gen = null;
    }

    /// <summary>
    /// Is the current room unlocked?
    /// </summary>
    /// <param name="position">Position of the door (will be normalized to the room pos)</param>
    /// <returns>Whether or not the room is unlocked</returns>
    public bool unlocked(Vector2 position)
    {
        //Snap position to the enclosed room
        if (position.x > 0)
        {
            position.x -= position.x % roomDistance;
        }
        else
        {
            position.x -= (15 - (Mathf.Abs(position.x) % roomDistance));
        }

        if (position.y > 0)
        {
            position.y -= position.y % roomDistance;
            position.y += 15;
        }
        else
        {
            position.y -= (15 - (Mathf.Abs(position.y) % roomDistance));
            position.y += roomDistance;
        }

        Room room;
        //Get that room and check it
        if (map.TryGetValue(position, out room))
        {
            return room.unlocked;
        }
        return false;
    }

    /// <summary>
    /// Kills an enemy in the specified room
    /// </summary>
    /// <param name="position"Position of the enemy (will be normalized to the room pos)></param>
    /// <returns>Is the room unlocked?</returns>
    public bool enemyDied(Vector2 position)
    {
        //Snap position to the enclosed room
        if (position.x > 0)
        {
            position.x -= position.x % roomDistance;
        }
        else
        {
            position.x -= (15 - (Mathf.Abs(position.x) % roomDistance));
        }

        if (position.y > 0)
        {
            position.y -= position.y % roomDistance;
            position.y += 15;
        }
        else
        {
            position.y -= (15 - (Mathf.Abs(position.y) % roomDistance));
            position.y += roomDistance;
        }

        Room room;
        //Get that room and update it
        if(map.TryGetValue(position, out room))
        {
            return room.removeEnemies(1);
        }
        return false;
    }

    /// <summary>
    /// Generates a new level based on the difficulty and level number
    /// Then calls spawnLevel to actually insantiate it
    /// </summary>
    public void generateLevel()
    {
        //Figure out the max room count for this floor
        curMaxSize = (int)(defaultMaxSize + (defaultMaxSize * 0.5f * (manager.difficulty * manager.floor)));

        //Create a map for the floor
        map.Clear();

        //Queue of rooms that want to be added to the floor
        List<Room> toBeGenerated = new List<Room>();

        //Create the first room, set it's position and add it to the queue
        Room firstRoom = new Room();
        firstRoom.position = Vector2.zero;
        toBeGenerated.Add(firstRoom);

        //Start generating the rooms
        addNode(map, toBeGenerated, 1);

        List<Room> spawned = new List<Room>();

        //Spawn the rooms
        spawnLevel(map, firstRoom, spawned, 5);

        spawned.Clear();

        //Create the camera
        Instantiate(cameraPrefab, new Vector3(0.0f, 0.0f, -10.0f), Quaternion.identity);

    }

    /// <summary>
    /// Generate the level recursively using breadth first generate
    /// </summary>
    /// <param name="map">Map of the entire level</param>
    /// <param name="toBeGenerated">List (Queue) of rooms that are waiting to be added</param>
    /// <param name="roomCount">Current number of rooms</param>
    private void addNode(Dictionary<Vector2, Room> map, List<Room> toBeGenerated, int roomCount)
    {
        //If no rooms want to be added, then we are done
        if(toBeGenerated.Count == 0)
        {
            return;
        }

        //Grab to top room and remove it from toBeGenerated
        Room current = toBeGenerated[0];
        toBeGenerated.RemoveAt(0);

        //If the room is already in the map
        Room inMap;
        if (map.TryGetValue(current.position, out inMap))
        {
            //Give it any new doors
            for (int e = 0; e < current.doors.Length; ++e)
            {
                if (current.doors[e] && !inMap.doors[e])
                {
                    inMap.addDoor(e);
                }
            }
        }//If the room isn't already in the map, we need to generate it
        else
        {
            //Add it to the map
            map.Add(current.position, current);

            //How many doors this room has
            int doorCount = 0;

            //Buffs the chance of getting new rooms
            float buff = 0;
            //Buff the first room to make a spiderweb instead of a pole
            if(roomCount == 1)
            {
                buff = 3.0f;
            }
            //Go through each door for the room
            for (int e = 0; e < 4; ++e)
            {
                //If we are at the max room count, break
                if (roomCount == curMaxSize)
                {
                    break;
                }
                //If a door here does not already exist
                if (!current.doors[e])
                {

                    //Check to see if this room wants another door based on an algorithm
                    if (Random.Range(Mathf.Log(manager.difficulty * manager.floor) + buff, 10.0f + Mathf.Log(manager.difficulty * manager.floor) + (0.5f * doorCount)) > 5.0f)
                    {
                        Room temp;
                        //Make it harder to punch into existing rooms
                        if (!map.TryGetValue(dirToPos(current.position, e), out temp) || Random.Range(0.0f, 10.0f) > 2.0f)
                        {
                            //If the room in the given direction does exist, connect them
                            if (temp != null)
                            {
                                temp.addDoor(oppositeDirection(e));
                                current.addDoor(e);
                            }//If the room in the given direction doesn't exist
                            else if(roomCount < curMaxSize)
                            {
                                //Increment room count
                                ++roomCount;
                                
                                //Create the new room and connect them
                                Room newRoom = new Room();
                                newRoom.position = dirToPos(current.position, e);
                                newRoom.addDoor(oppositeDirection(e));
                                current.addDoor(e);

                                //Add the new room to the queue
                                toBeGenerated.Add(newRoom);

                                //If we are at the max room count, break
                                if (roomCount == curMaxSize)
                                {
                                    break;
                                }
                            }

                        }
                    }
                }
            }
        }
        //Add the next room
        addNode(map, toBeGenerated, roomCount);
    }

    /// <summary>
    /// Take a map of a level and a current room, then, recursivly spawns the level
    /// </summary>
    /// <param name="map">Dictionary of every room on the map</param>
    /// <param name="room">The current room to spawn</param>
    /// <param name="spawned">A list of already spawned rooms</param>
    private void spawnLevel(Dictionary<Vector2, Room> map, Room room, List<Room> spawned, int keyCountDown)
    {

        --keyCountDown;

        spawned.Add(room);
        //Create the room
        Instantiate(rooms[room.getPrefabIndex()], room.position, Quaternion.identity);
        if (!room.position.Equals(Vector2.zero))
        {
            generateEnemies(room);
        }
        else
        {
            Instantiate(playerPrefab, new Vector2(room.position.x + 3.0f, room.position.y - 3.0f), Quaternion.identity);
        }

        if (keyCountDown == 0)
        {
            //This room has the elevator's keycard
        }

        //Go through each door for the room
        for (int e = 0; e < 4; ++e)
        {
            //If there is a door in that direction and the corresponding room hasn't already been spawned
            if (room.doors[e])
            {
                Room nextRoom;
                if (map.TryGetValue(dirToPos(room.position, e), out nextRoom) && !spawned.Contains(nextRoom))
                {
                    //Spawn corresponding room
                    spawnLevel(map, nextRoom, spawned, keyCountDown);

                }
            }
        }

    }

    /// <summary>
    /// Takes a position and direction and find the adjacent position in that direction
    /// </summary>
    /// <param name="oldPos">The origin</param>
    /// <param name="direction">Which way you want to travel</param>
    /// <returns>The new position</returns>
    private Vector2 dirToPos(Vector2 oldPos, int direction)
    {
        if(direction == UP)
        {
            oldPos.y += roomDistance;
        }
        else if(direction == DOWN)
        {
            oldPos.y -= roomDistance;
        }
        else if(direction == LEFT)
        {
            oldPos.x -= roomDistance;
        }//RIGHT
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

        if (direction == UP)
        {
            return DOWN;
        }
        else if (direction == DOWN)
        {
            return UP;
        }
        else if (direction == LEFT)
        {
            return RIGHT;
        }//RIGHT
        else
        {
            return LEFT;
        }
    }

    /// <summary>
    /// Spawns enemies in a specified room location
    /// </summary>
    /// <param name="roomPosition"Location of the rooms to spawn enemies in></param>
    private void generateEnemies(Room room)
    {
        //Determine number of enemies to spawn based on floor number and difficulty
        int numEnemies = (int) (Random.Range(0.0f, 4.0f + (0.5f * (manager.floor + (manager.floor * 0.25f * manager.difficulty)))));

        if (numEnemies > 0)
        {
            room.addEnemies(numEnemies);
        }

        //Spawn enemies in random place in room
        for(int e = 0; e < numEnemies; ++e)
        {
            Instantiate(enemyPrefab, new Vector2(room.position.x + Random.Range(0.2f, 5.8f), room.position.y + Random.Range(-5.8f, -0.2f)), Quaternion.identity);
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
    /// The current number of enemies in the room
    /// </summary>
    private int enemies = 0;

    /// <summary>
    /// Are this room's doors unlocked?
    /// </summary>
    public bool unlocked = true;

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
    /// Adds enemies to the room
    /// </summary>
    /// <param name="amount">Amount of enemies to add</param>
    public void addEnemies(int amount)
    {
        enemies += amount;
        unlocked = false;
    }

    /// <summary>
    /// Removes enemies from the roo,
    /// </summary>
    /// <param name="amount">Amount of enemies to remove</param>
    /// <returns>Are the doors unlocked?</returns>
    public bool removeEnemies(int amount)
    {
        enemies -= amount;
        Debug.Log(enemies);
        if(enemies <= 0)
        {
            enemies = 0;
            unlocked = true;
        }
        return unlocked;
    }

    /// <summary>
    /// Converts this room into it's correpsonding index in the room prefab array
    /// </summary>
    /// <returns>The index that corresponds to this room in the room prefab array</returns>
    public int getPrefabIndex()
    {
        //If Up door
        if (doors[0])
        {
            //If up and down door
            if (doors[1])
            {
                //If up, down, and left door
                if (doors[2])
                {   
                    //If up, down, left and right door
                    if (doors[3])
                    {
                        return 14;
                    }//If up, down and left door only
                    else
                    {
                        return 13;
                    }
                }
                else
                {
                    //If up, down, and right door only
                    if (doors[3])
                    {
                        return 11;
                    }//If up and down door only
                    else
                    {
                        return 9;
                    }
                }
            }
            else
            {
                //If up and left door
                if (doors[2])
                {
                    //If up, left, and right door only
                    if (doors[3])
                    {
                        return 10;
                    }//If up and left door only
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    //If up and right door only
                    if (doors[3])
                    {
                        return 5;
                    }//If up door only
                    else
                    {
                        return 0;
                    }
                }
            }
        }
        else
        {
            //If down door
            if (doors[1])
            {
                //If down and left door
                if (doors[2])
                {
                    //If down, left, and right door
                    if (doors[3])
                    {
                        return 12;
                    }//If down and left door only
                    else
                    {
                        return 7;
                    }
                }
                else
                {
                    //If down and right door only
                    if (doors[3])
                    {
                        return 6;
                    }//If down door only
                    else
                    {
                        return 2;
                    }
                }
            }
            else
            {
                //If left door
                if (doors[2])
                {
                    //If left and right door only
                    if (doors[3])
                    {
                        return 8;
                    }//If left door only
                    else
                    {
                        return 3;
                    }
                }
                else
                {
                    //If right door only
                    if (doors[3])
                    {
                        return 1;
                    }
                }
            }
        }
        return 0;
    }
}
