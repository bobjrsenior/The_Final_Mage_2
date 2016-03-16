using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DifficultyManager : MonoBehaviour {

    /// <summary>
    /// Which floor have we reached?
    /// </summary>
    public int floor = 1;

    /// <summary>
    /// How fast do the enemies move?
    /// </summary>
    public float enemySpeed;

    /// <summary>
    /// How fast do ranged enemies move?
    /// </summary>
    public float rangeEnemyMoveSpeed;

    /// <summary>
    /// True when we go to a new floor, false otherwise.
    /// </summary>
    public bool newFloor;

    /// <summary>
    /// The difficulty of the current game
    /// </summary>
    public int difficulty = 1;

    /// <summary>
    /// Our base standard melee health.
    /// </summary>
    public float enemyStandardMeleeHP = 4.0f;

    /// <summary>
    /// Our base standard ranged health.
    /// </summary>
    public float enemyStandardRangedHP = 5.0f;

    /// <summary>
    /// Our base standard melee damage
    /// </summary>
    public float enemyStandardMeleeDamage = 1.0f;

    /// <summary>
    /// Our base standard range damage
    /// </summary>
    public float enemyStandardRangeDamage = 1.0f;

    /// <summary>
    /// Have we set our stats for the enemies on this floor?
    /// </summary>
    public bool statsSet = false;

    public bool pStatsSet;

    /// <summary>
    /// The players standard health
    /// </summary>
    public float playerStandardHealth = 5.0f;

    /// <summary>
    /// The players standard max health
    /// </summary>
    public float playerStandardMaxHealth = 5.0f;

    /// <summary>
    /// The players standard melee damage.
    /// </summary>
    public float playerStandardMeleeDam = 0.5f;

    /// <summary>
    /// The players standard ranged damage.
    /// </summary>
    public float playerStandardRangedDam = 1.0f;

    /// <summary>
    /// Melee sprite
    /// </summary>
    public Sprite meleeType;

    /// <summary>
    /// Range sprite
    /// </summary>
    public Sprite rangeType;

    /// <summary>
    /// The speed of an enemy ranged attack.
    /// </summary>
    public float enemyRangeSpeed;

    /// <summary>
    /// How long is the delay between enemy ranged attacks?
    /// </summary>
    public float enemyShootWaitTime;

    /// <summary>
    /// The ranged projectile prefab enemies shoot.
    /// </summary>
    public GameObject enemyRangeProjectile;

    /// <summary>
    /// Whether or not the player has the keycard
    /// </summary>
    public bool gotKeyCard = false;

    /// <summary>
    /// The index the level is on
    /// </summary>
    private int buildIndex;

    public static DifficultyManager dManager;

    void OnGUI()
    {
        GUI.color = Color.yellow;
        GUI.Box(new Rect(0, 80, 100, 25), "Floor: " + floor);

    }

    void Start()
    {
        if(dManager != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            dManager = this;
            DontDestroyOnLoad(this.gameObject);

            //Start on floor 1
            floor = 1;
            buildIndex = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(setUpFloor());
        }
        
    }


    void OnLevelWasLoaded(int level)
    {
        
        if (dManager != null && dManager.Equals(this) && buildIndex == SceneManager.GetActiveScene().buildIndex)
        {
            StartCoroutine(setUpFloor());
        }

    }

    // Update is called once per frame
    void Update () {


	}

    /// <summary>
    /// Setup the current floor
    /// </summary>
    private IEnumerator setUpFloor()
    {
        gotKeyCard = false;
        if (SceneManager.GetActiveScene().name.Equals("LevelGen"))
        {
            LevelGen.gen.generateLevel();
        }
        while (PlayerHealth.pHealth == null || PlayerAttack.pAttack == null) { yield return null; }
        setPlayerStats();
        setEnemyStats();
    }

    /// <summary>
    /// Called when a floor has been completed
    /// </summary>
    public void wonFloor()
    {
        if (gotKeyCard)
        {
            ++floor;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }

    /// <summary>
    /// Called when you have retrieved a keycard
    /// </summary>
    public void retrievedKeyCard()
    {
        gotKeyCard = true;
    }

    /// <summary>
    /// Sets the enemy stats to the appropriate values.
    /// </summary>
    private void setEnemyStats()
    {
        newFloor = false;
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        //For every enemy in the gamespace
        for (int x = 0; x < enemies.Length; x++)
        {
            EnemyHealth enemyHP = enemies[x].GetComponent<EnemyHealth>();
            EnemyAI enemyAI = enemies[x].GetComponent<EnemyAI>();
            
            //Randomly generates enemy type.
            int type = Random.Range(0, 2);
            if (type == 0)
            {
                enemies[x].rangedType = false;
                enemies[x].meleeType = true;
                enemies[x].GetComponent<SpriteRenderer>().sprite = meleeType;
            }
            else if (type == 1)
            {
                enemies[x].meleeType = false;
                enemies[x].rangedType = true;
                enemies[x].GetComponent<SpriteRenderer>().sprite = rangeType;
            }

            //If the current enemy is a melee type
            if (enemies[x].meleeType == true)
            {
                //No additional floor modifiers on health and attack power for the time being.
                enemyHP.maxHealth = enemyStandardMeleeHP;
                enemyHP.health = enemyStandardMeleeHP;
                enemyAI.enemySpeed = enemySpeed;

                enemyAI.meleeDamage = enemyStandardMeleeDamage;
            }
            else
            {
                //No additional floor modifiers on health and attack power for the time being.
                enemyHP.maxHealth = enemyStandardRangedHP;
                enemyHP.health = enemyStandardRangedHP;

                //Ranged types always move at 1f speed to allow players to reach them for melee attacks.
                enemyAI.enemySpeed = rangeEnemyMoveSpeed;
                enemyAI.rangeDamage = enemyStandardRangeDamage;
                enemyAI.rangeSpeed = enemyRangeSpeed;
                enemyAI.shootWaitTime = enemyShootWaitTime;
                enemyAI.rangeProjectile = enemyRangeProjectile;
            }

        }
        statsSet = true;
    }

    private void setPlayerStats()
    {
        PlayerHealth.pHealth.maxHealth = playerStandardMaxHealth;
        PlayerHealth.pHealth.health = playerStandardHealth;
        PlayerAttack.pAttack.meleeDamage = playerStandardMeleeDam;
        PlayerAttack.pAttack.rangeDamage = playerStandardRangedDam;
        pStatsSet = true;
    }
}
