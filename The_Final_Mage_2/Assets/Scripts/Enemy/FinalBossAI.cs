﻿using UnityEngine;
using System.Collections;

public class FinalBossAI : MonoBehaviour {

    /// <summary>
    /// How fast this object moves in Units/Second
    /// </summary>
    private float movementSpeed = 4.0f;

    /// <summary>
    /// The player's transform component
    /// </summary>
    private Transform player;

    /// <summary>
    /// This objects Rigidbody2D
    /// </summary>
    private Rigidbody2D rigidbody;

    /// <summary>
    /// Prefab of this object's range projectile
    /// </summary>
    [SerializeField]
    private GameObject rangeProjectile;

    /// <summary>
    /// The speed at which projectiles fired will fly
    /// </summary>
    private float projectileSpeed = 5.0f;

    /// <summary>
    /// Where this object can reside (xMin, xMax, yMin, yMax)
    /// </summary>
    [SerializeField]
    private Vector4 bounds;

    /// <summary>
    /// The different major phases of this object
    /// </summary>
    private enum Phase { Phase1, Phase2, Dead }

    /// <summary>
    /// The current major phase
    /// </summary>
    [SerializeField]
    private Phase phase;

    /// <summary>
    /// The different states of this object (each major phase can have multiple states)
    /// </summary>
    private enum State { Idle, Moving, Attacking, MovingAndAttacking, Hurt}

    /// <summary>
    /// The current state of this object
    /// </summary>
    [SerializeField]
    private State state;


    ////// General Variables

    /// <summary>
    /// This float values is used to represent a timer with no value
    /// </summary>
    private float defaultTimerValue = -999.0f;

    /// <summary>
    /// A general timer for timing things
    /// </summary>
    private float generalTimer1;

    /// <summary>
    /// A general timer for timing things
    /// </summary>
    private float generalTimer2;

    /// <summary>
    /// This Vector2 value is used to represent a Vector2 with no value
    /// </summary>
    private Vector2 defaultVector2 = new Vector2(-999, -999);

    /// <summary>
    /// A general Vector2 used for targets
    /// </summary>
    private Vector2 generalTarget1;

    /// <summary>
    /// A general Vector2 used for targets
    /// </summary>
    private Vector2 generalTarget2;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        generalTarget1 = defaultVector2;
        generalTarget2 = defaultVector2;

        Random.seed = (int) System.DateTime.Now.Ticks;

        phase1Setup();

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (state.Equals(State.Attacking))
        {
            //state = State.Idle;
        }
        else if (state.Equals(State.MovingAndAttacking))
        {
            //state = State.Moving;
        }

        if (phase.Equals(Phase.Phase1))
        {
            phase1();
        }
        else if (phase.Equals(Phase.Phase2))
        {
            phase2();
        }
        else if (phase.Equals(Phase.Dead))
        {
            dead();
        }
    }

    private void phase1()
    {
        if (state.Equals(State.Idle))
        {
            if(generalTimer1 == defaultTimerValue)
            {
                generalTimer1 = Random.Range(2.0f, 4.8f);
            }

            generalTimer1 -= Time.fixedDeltaTime;
            if(generalTimer1 <= 0)
            {
                changeState();
            }
        }
        else if (state.Equals(State.Moving) || state.Equals(State.MovingAndAttacking))
        {
            if (generalTarget1.Equals(defaultVector2))
            {
                generalTarget1 = new Vector2(Random.Range(bounds[0], bounds[1]), Random.Range(bounds[2], bounds[3]));
                print(generalTarget1);
            }

            Vector2 direction = generalTarget1 - (Vector2) transform.position;
            if(direction.sqrMagnitude < 0.1f)
            {
                rigidbody.velocity = Vector2.zero;
                changeState();
            }
            else
            {
                direction.Normalize();
                rigidbody.velocity = direction * movementSpeed;
            }

        }
        if (state.Equals(State.Attacking) || state.Equals(State.MovingAndAttacking))
        {
            if(generalTimer2 == defaultTimerValue)
            {
                if (state.Equals(State.Attacking))
                {
                    generalTimer1 = Random.Range(2.0f, 9.5f);
                    generalTimer2 = Random.Range(0.2f, 1.9f);
                }
                else {
                    generalTimer2 = Random.Range(0.2f, 0.8f);
                }
            }

            if (state.Equals(State.Attacking))
            {
                generalTimer1 -= Time.fixedDeltaTime;
            }
            if(generalTimer1 <= 0 && state.Equals(State.Attacking))
            {
                changeState();
            }
            else
            {
                generalTimer2 -= Time.fixedDeltaTime;
                if(generalTimer2 <= 0)
                {
                    Shoot();
                    if (state.Equals(State.Attacking))
                    {
                        generalTimer2 = Random.Range(0.2f, 1.9f);
                    }
                    else {
                        generalTimer2 = Random.Range(0.2f, 0.8f);
                    }
                }
            }
        }
        //else if (state.Equals(State.MovingAndAttacking))
        //{
        //
        //}
    }

    private void changeState()
    {
        generalTarget1 = defaultVector2;
        generalTarget2 = defaultVector2;

        generalTimer1 = defaultTimerValue;
        generalTimer2 = defaultTimerValue;

        float chance = Random.Range(0.0f, 10.0f);

        //Idle
        if (chance < 1.5f)
        {
            state = State.Idle;
        }//Attack
        else if (chance < 4.9f)
        {
            state = State.Attacking;
        }//Move
        else if (chance < 7.3f)
        {
            state = State.Moving;
        }//Move and Attack
        else
        {
            state = State.MovingAndAttacking;
        }
    }

    private void phase2()
    {

    }

    private void dead()
    {

    }

    private void phase1Setup()
    {
        phase = Phase.Phase1;
        state = State.Idle;
    }

    private void phase2Setup()
    {
        phase = Phase.Phase2;
    }

    private void deadSetup()
    {
        phase = Phase.Dead;
    }

    /// <summary>
    /// The AI shoot function.
    /// </summary>
    private void Shoot()
    {
        Vector2 attack_vector = ((Vector2)player.position - (Vector2)transform.position).normalized;
        GameObject rAttack = Instantiate(rangeProjectile, (Vector2)transform.position, Quaternion.identity) as GameObject;
        rAttack.GetComponent<Rigidbody2D>().velocity = attack_vector * projectileSpeed;
    }
}
