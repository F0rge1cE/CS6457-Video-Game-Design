using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "Player")
		{
			if (this.tag == "Lightning") {
				col.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter> ().m_MoveSpeedMultiplier *= 1.2f;
			} else if (this.tag == "Heart") {
				int totaltries = col.gameObject.GetComponent<HealthManager> ().totalTries;
				int successtries = col.gameObject.GetComponent<HealthManager> ().successTries;
				if (totaltries > successtries) {
					col.gameObject.GetComponent<HealthManager> ().successTries += 1;
				}
			}

			// Play the player's powerUP audio.
			player.GetComponent<AudioManager> ().powerUp();


			GameObject sp1 = GameObject.Find ("sp1");
			GameObject sp2 = GameObject.Find ("sp2");
			GameObject sp3 = GameObject.Find ("sp3");
			GameObject sp4 = GameObject.Find ("sp4");

			GameObject[] sps = { sp1, sp2, sp3, sp4 };

			int counter = 0;
			foreach (GameObject sp in sps) {
				if (Vector3.Distance (this.gameObject.transform.position, sp.transform.position) < 1f) {
					GameObject.Find ("PowerupManager").GetComponent<PowerupGeneration> ().powerupExists [counter] = false;
					GameObject.Find ("PowerupManager").GetComponent<PowerupGeneration> ().lastSpawnTime [counter] = Time.time;
					break;
				}
				counter += 1;
			}

			Destroy(this.gameObject);
		}
	}
}
