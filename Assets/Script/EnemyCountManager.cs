using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyCountManager : MonoBehaviour {

	public AIManager aimanager;
	public AIController aicontroller;
	public int enemiesKilled = 0;
	private bool isSuc = false;

	public Text enemyCountText;

	// Use this for initialization
	void Start () {
		enemyCountText = GameObject.Find ("EnemyCountText").GetComponent<Text> ();
		aicontroller = GameObject.Find ("Y_Bot_I_CAN_HIDE").GetComponent<AIController> ();
	}
	
	// Update is called once per frame
	void Update () {
		enemyCountText.text = "Hiders Caught: " + aimanager.ManDown + "\n Total Hiders: " + aicontroller.MAXBOTAMOUNT +
			"\n Enemies Killed: " + enemiesKilled; 
		
		if (aimanager.ManDown == aicontroller.MAXBOTAMOUNT) {
			isSuc = true;
			success ();
		}
	}

	void success(){

		if (isSuc) {
			Debug.Log ("Go to Succ");
			//			SceneManager.LoadScene (1, LoadSceneMode.Additive);
			SceneManager.LoadScene ("gameSuccess");
			isSuc = false;
		}

	}
}
