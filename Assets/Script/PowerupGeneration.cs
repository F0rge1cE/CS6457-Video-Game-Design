using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupGeneration : MonoBehaviour {

	public GameObject[] spawnPoints;

	public GameObject[] powerUps;

	public bool[] powerupExists;
	public float[] lastSpawnTime;

	private int spawnCD = 30;

	// Use this for initialization
	void Start () {
		
		powerupExists = new bool[spawnPoints.Length];
		lastSpawnTime = new float[spawnPoints.Length];
		for(int i=0;i<powerupExists.Length;i++) {
			powerupExists [i] = true;
			lastSpawnTime [i] = 0;
		}
		if (spawnPoints.Length > 0) {
			foreach(GameObject sp in spawnPoints){
				float rndNum = Random.value;
				int index = (int)(rndNum * (float)powerUps.Length);
				if(index >= 0 && index < powerUps.Length)
					Instantiate(powerUps[index], sp.transform.position, sp.transform.rotation);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < spawnPoints.Length; i++) {
			float curDelta = Time.time - lastSpawnTime [i];
			if (curDelta > spawnCD && !powerupExists [i]) {
				float rndNum = Random.value;
				int index = (int)(rndNum * (float)powerUps.Length);
				if (index >= 0 && index < powerUps.Length) {
					Instantiate (powerUps [index], spawnPoints [i].transform.position, spawnPoints [i].transform.rotation);
					powerupExists [i] = true;
					//lastSpawnTime [i] = Time.time;
				}
				
			}
		}
	}
}
