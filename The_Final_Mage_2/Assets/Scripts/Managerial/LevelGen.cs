using UnityEngine;
using System.Collections.Generic;

public class LevelGen : MonoBehaviour {

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
        
	}
	

    /// <summary>
    /// Generates a new level based on the difficulty and level number
    /// Then calls spawnLevel to actually insantiate it
    /// </summary>
    public void generateLevel()
    {
        curMaxSize =(int) (defaultMaxSize + (defaultMaxSize * 0.5f * (manager.difficulty * manager.floor)));
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
        }//Make sure there are atleast 2 rooms
        if(totalRooms == 2)
        {
            room1.addDoor(0);
        }
        //Find which room the keykard is (will be revised)
        int keyCardIndex = Random.Range(1, totalRooms);
        //Generate the rooms
        spawnLevel(map, room1, keyCardIndex);
        //Create the camera
        Instantiate(cameraPrefab, new Vector3(0.0f, 0.0f, -10.0f), Quaternion.identity);
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
                if(Random.Range(Mathf.Log(manager.difficulty * manager.floor), 10.0f + (0.5f * doorCount)) > 5.0f)
                {
                    Room temp;
                    //Make it harder to punch into existing rooms
                    if (!map.TryGetValue(dirToPos(room.position, e), out temp) || Random.Range(0.0f, 10.0f) > 2.0f)
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
            }
            else
            {
                ++doorCount;
            }
        }

        return totalRooms;
    }

    /// <summary>
    /// Take a map of a level and a current room, then, recursivly spawns the level
    /// </summary>
    /// <param name="map">Dictionary of every room on the map</param>
    /// <param name="room">The current room to spawn</param>
    private void spawnLevel(Dictionary<Vector2, Room> map, Room room, int keyCountDown)
    {
        --keyCountDown;

        //Remove the current room from the map to avoid duplicate spawning
        map.Remove(room.position);
        //Create the rrom
        Instantiate(rooms[room.getPrefabIndex()], room.position, Quaternion.identity);
        if (!room.position.Equals(Vector2.zero))
        {
            generateEnemies(room.position);
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
                if (map.TryGetValue(dirToPos(room.position, e), out nextRoom))
                {
                    //Spawn corresponding room
                    spawnLevel(map, nextRoom, keyCountDown);

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
    private void generateEnemies(Vector2 roomPosition)
    {
        //Determine number of enemies to spawn based on floor number and difficulty
        int numEnemies = (int) (Random.Range(0.0f, 4.0f + (0.5f * (manager.floor + (manager.floor * 0.25f * manager.difficulty)))));

        //Spawn enemies in random place in room
        for(int e = 0; e < numEnemies; ++e)
        {
            Instantiate(enemyPrefab, new Vector2(roomPosition.x + Random.Range(0.2f, 5.8f), roomPosition.y + Random.Range(-5.8f, -0.2f)), Quaternion.identity);
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
