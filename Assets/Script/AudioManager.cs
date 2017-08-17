using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

//	float footOnGround;
//	bool attacking;
//	float hurted;
//
//	float footLevel;

	Animator anim; 
	public AudioSource audio;
	public AudioClip footStep;
	public AudioClip ahhhhh;
	public AudioClip faQ;
	public AudioClip axeSlash;
	public AudioClip powerup;

	public AudioClip enemyRespawn;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		audio = GetComponent<AudioSource> ();

	}

	// Update is called once per frame
	void Update () {

//		footOnGround =  anim.GetFloat ("JumpLeg");
//		attacking = anim.GetBool ("Attack");
//
//		if (footOnGround >= 0.999f || footOnGround <= -0.999f) {
//			print ("jinlaile");
////			if (!audio.isPlaying){
//				audio.Stop();
//				audio.PlayOneShot (footStep);
//				print ("Moving Step sound0");
////			}
//		}
//
//
//		if (attacking ) {
//			audio.Stop();
//			audio.PlayOneShot (faQ);
//		}
	}

	public void hurted(){
		audio.PlayOneShot (ahhhhh);

		}

	public void attackAxe(){
		audio.PlayOneShot (faQ);
		audio.PlayOneShot (axeSlash);

		}

	public void powerUp(){
		audio.PlayOneShot (powerup);

	}

	public void enenmyrespawn(){
		audio.PlayOneShot (enemyRespawn);

	}

}

