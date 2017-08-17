using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.ThirdPerson;

public class CountDown : MonoBehaviour {

	public Camera flyCam;
	public Camera playerCam;
	public GameObject countdownText;
	public int hideTime = 10;
	public int seekTime = 300;
	public static float timeSinceLevelLoad;
	public bool hideTimeEnded;
	public GameObject player;
	public GameObject mutant;

	// Use this for initialization
	void Start () {
		print ("lalalalalalalalalalalala");
		flyCam = GameObject.Find ("FlyCamera").GetComponent<Camera>();
		playerCam = GameObject.Find ("PlayerCamera").GetComponent<Camera>();
		countdownText = GameObject.Find ("CountdownText");
		flyCam.enabled = true;
		playerCam.enabled = false;
		hideTimeEnded = false;
		player = GameObject.FindGameObjectWithTag ("Player");
		mutant = GameObject.Find ("Mutant");
		mutant.SetActive (false);
		player.GetComponent<ThirdPersonUserControl> ().enabled = false;
		player.GetComponent<ThirdPersonCharacter> ().enabled = false;
		player.GetComponent<HealthManager> ().enabled = false;

//		GameObject.Find ("PlayerCamera").GetComponent<OrbitCamera> ().enabled = false;
//		hideTime = 5;
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceLevelLoad = Time.timeSinceLevelLoad;
		Text text = countdownText.GetComponent<Text> ();
		int remainingSec;
		if (!hideTimeEnded) {
			remainingSec = (int)(hideTime - timeSinceLevelLoad);
		} else {
			remainingSec = (int)(seekTime - timeSinceLevelLoad + hideTime);
		}
		text.text = "Hider Time remaining: " + remainingSec + "  seconds";
		if (!hideTimeEnded && remainingSec < 0) {
			hideTimeEnded = true;
			//text.enabled = false;
			flyCam.enabled = false;
			GameObject.Find ("FlyCamera").SetActive (false);

			playerCam.enabled = true;
			player.GetComponent<ThirdPersonUserControl> ().enabled = true;
			player.GetComponent<ThirdPersonCharacter> ().enabled = true;
			player.GetComponent<HealthManager> ().enabled = true;

			mutant.SetActive (true);

//			GameObject.Find ("PlayerCamera").GetComponent<OrbitCamera> ().enabled = true;
		}
		if (hideTimeEnded) {
			remainingSec = (int)(seekTime - timeSinceLevelLoad + hideTime);
			text.text = "Seek Time remaining: " + remainingSec + "  seconds";
			if (remainingSec < 0) {
				text.text = "Time's up!! Game over.";
				SceneManager.LoadScene ("gameOver");
			}
		}
	}
}
