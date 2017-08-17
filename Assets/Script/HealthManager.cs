using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour {


	public int hitpoints;
	public int totalTries;
	public int successTries;
	private GameObject[] disabledHP;
	private Animator anim;

	private bool isDead = false;

 	// Use this for initialization
	void Start () {
		hitpoints = 10;
		totalTries = 0;
		successTries = 0;
		disabledHP = new GameObject[11];
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (Input.GetMouseButtonDown (0)) {
			AnimatorStateInfo currentPlayerState = anim.GetCurrentAnimatorStateInfo (0);
			int attackState = Animator.StringToHash("Base.Attack");
			if (!currentPlayerState.IsName ("Attack")) {
				totalTries += 1;
			}
		}
		int lostHP = totalTries - successTries;
		if (lostHP == 10 && !isDead) {
			isDead = true;
			totalTries = 0;
			successTries = 0;
//			enterMaze ();
		}

		hitpoints = 10 - lostHP;
		for (int i = 1; i <= hitpoints; i++) {
			GameObject existHP = GameObject.Find ("HP" + i);
			if (!existHP) {
				disabledHP [i].SetActive (true);
			}
		}
		for (int i = hitpoints + 1; i <= 10; i++) {
			GameObject hp = GameObject.Find ("HP" + i);
			if (hp) {
				disabledHP [i] = hp;
				hp.SetActive (false);
			}
		}
		Debug.Log (isDead);
		enterMaze ();
	}

//	void Update(){
//		int lostHP = totalTries - successTries;
//		if (lostHP == 10 && !isDead) {
//			isDead = true;
//			totalTries = 10;
//			successTries = 0;
//			//			enterMaze ();
//		}
//	}


	void enterMaze(){

		if (isDead) {
			Debug.Log ("Go to Maze");
//			SceneManager.LoadScene (1, LoadSceneMode.Additive);
			SceneManager.LoadScene ("gameOver");
			isDead = false;
		}

	}
}
