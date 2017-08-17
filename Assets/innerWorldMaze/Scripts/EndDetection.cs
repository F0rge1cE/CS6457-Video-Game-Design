using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class EndDetection : MonoBehaviour {

	public Vector3 startPos;
	public float fallHeight = 0;
	public AudioClip fallSound;
	public GameObject GaspSound;

	CharacterController CharControl;

	public bool isDead = false; 
	public bool isEscape = false;
	public bool shouldGameStop = false;

	TimeManager Timer;
	Animator anim;

	public GameObject isGetOut;

	float beginDelayTime = 0f;
	public float EndDelay = 3f;

	// Use this for initialization
	void Start () {
		Timer = GameObject.Find ("UIManager").GetComponent<TimeManager>();
		anim = GameObject.Find ("Canvas").GetComponent<Animator> ();

		CharControl = GameObject.Find ("First Person Controller").GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isGetOut.activeSelf && !isEscape && !isDead) {
			// Get out of the maze
			Debug.Log("Get Out! C#");
			anim.SetTrigger ("isGetOut");
			isEscape = true;

			shouldGameStop = true;

//			if(delaySomeTime())
			// Uncomment to connect with the city scene
//			goBacktoMainMenu(); // go back to the real world
		
		}


		if (Timer.timeOut == true && !isDead && !isEscape) {
			// Die!!
			Debug.Log ("Time out");
			anim.SetTrigger ("isDead");
//			timeStopStart (shouldGameStop);

			GaspSound.SetActive (false);  // Stop playing gasp audio
			isDead = true;

			shouldGameStop = true;

//			if(delaySomeTime())
			// Uncomment to connect with the main, add some delay please
//			goBacktoCity(); // go back to the real world
		}



		timeStopStart (shouldGameStop); // Is the game ended?

		if (isEscape || isDead) {
			if (delaySomeTime ()) {
				if(isDead)
				goBacktoMainMenu (); 
				if(isEscape)
				goBacktoCity ();
			}
		}

	}


	void timeStopStart(bool shouldGameStop){
		// Stop the game
		if (shouldGameStop){
//			Time.timeScale = 0f;
			CharControl.enabled = false;
		}
		else{
			// Resume game
//			Time.timeScale = 1f;
			CharControl.enabled = true;
		}
	}


	bool delaySomeTime(){
		beginDelayTime += Time.deltaTime;
//		Debug.Log (beginDelayTime);
		if (beginDelayTime >= EndDelay) {
			return true;
		}else{
			return false;
		}
	}

	void goBacktoMainMenu(){
		SceneManager.LoadScene ("MainMenu"); // Load the city scene
//		SceneManager.UnloadScene(1);
		Debug.Log("GO back to menu");
	}

	void goBacktoCity(){
		SceneManager.LoadScene ("City"); // Load the main menu
//		SceneManager.UnloadScene(1);
		Debug.Log("another chance of seeking...");
	}


}





