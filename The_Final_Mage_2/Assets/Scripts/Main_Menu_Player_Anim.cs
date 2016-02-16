using UnityEngine;
using System.Collections;

public class Main_Menu_Player_Anim : MonoBehaviour {

	private Animator anim;
	public float Speed;
	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator> ();
	
	
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetFloat ("Speed", GetComponent<Rigidbody2D> ().velocity.x);
	}
}
