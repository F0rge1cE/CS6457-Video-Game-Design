using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TutorialManager : MonoBehaviour {

	public List<string> tutorialTexts;
	private int curTextIndex;
	private GameObject textObject;
	private GameObject demoRobot;
	private GameObject player;
	private bool playerEnabled;
	public bool AICaught;

	// Use this for initialization
	void Start () {
		tutorialTexts.Add ("Now you have become our hero, Brute. \nThe brave and masculine man with a huge axe.");
		tutorialTexts.Add ("Note on your left, one blue alien is trying to envade your city!!!");
		tutorialTexts.Add("The alien can hide from your hunt by transforming into another object in the city.");
		tutorialTexts.Add ("Here is an example.");
		tutorialTexts.Add ("Now you can run towards the alien's disguise and attack it!");
		tutorialTexts.Add ("Congratulations! You just caught an alien hider.");
		tutorialTexts.Add ("But please note every wrong attack will cause Brute to lose 1 HP.");
		tutorialTexts.Add ("You have 10 inital HP, as indicated at the lower left corner. Now, catch them all!");
		tutorialTexts.Add ("Now, catch them all!");

		curTextIndex = 0;
		textObject = GameObject.Find("TutorialText");
		demoRobot = GameObject.FindGameObjectWithTag ("AI");
		demoRobot.GetComponent<OldAIController> ().enabled = false;
		player = GameObject.FindGameObjectWithTag ("Player");
		playerEnabled = false;
		AICaught = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			if (curTextIndex <= 3)
				curTextIndex += 1;
			else if (curTextIndex == 4 && AICaught) {
				curTextIndex += 1;
			}else if(curTextIndex >= 5){
				player.GetComponent<HealthManager> ().enabled = true;
				curTextIndex += 1;
				curTextIndex = Mathf.Min (curTextIndex, tutorialTexts.Count-1);
			}
		}
		Text text = textObject.GetComponent<Text> ();
		text.text = tutorialTexts [curTextIndex];

		if (curTextIndex == 3) {
			AlienHideDemo ();
		}

		if (curTextIndex == 4) {
			if (!playerEnabled) {
				EnablePlayer ();
			}
		}

		if (curTextIndex == tutorialTexts.Count - 1) {
			StartCoroutine(Pause());
			enterGame ();
		}
	}

	void AlienHideDemo(){
		demoRobot.GetComponent<OldAIController> ().enabled = true;
	}

	void EnablePlayer(){
		player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl> ().enabled = true;
		playerEnabled = true;
	}

	void enterGame(){
		SceneManager.LoadScene ("City");
	}
	IEnumerator Pause()
	{
		print (Time.time);
		yield return new WaitForSeconds(3.0f);
	}
}
