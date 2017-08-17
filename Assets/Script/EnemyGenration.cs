using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenration : MonoBehaviour {

	public GameObject enemy;
	public GameObject player;
	public DiffRecord diffs;

	public float SpawnCD;
	public float spawnDistance = 20;
	private float lastSpwanTime;
	private int diff;

	// Use this for initialization
	void Start () {
		
		enemy = GameObject.Find ("Attacker");
		player = GameObject.FindGameObjectWithTag ("Player");
		diffs = GameObject.Find ("DifficultyModel").GetComponent<DiffRecord> ();
		diff = diffs.difficulty;
		if (diff == 0) {
			SpawnCD = 99999999999;
		}
		if (diff == 1) {
			SpawnCD = 20;
		}
		if (diff == 2) {
			SpawnCD = 10;
		}
//		SpawnCD = 20;
	}
	
	// Update is called once per frame
	void Update () {
		print (SpawnCD+"diren");
		float randValue = Random.value;
		if (randValue < .05f && (Time.time - lastSpwanTime) > SpawnCD) // 5% of the time
		{
			Vector3 playerPos = player.transform.position;
			Vector3 playerDirection = player.transform.forward;
			Quaternion playerRotation = player.transform.rotation;
//			float spawnDistance = 20;

			Vector3 spawnPos = playerPos + playerDirection*spawnDistance;

			Instantiate(enemy, spawnPos, playerRotation );
			// Play the player's enemy respawn audio.
			player.GetComponent<AudioManager> ().enenmyrespawn();


			lastSpwanTime = Time.time;
		}
	}
}
