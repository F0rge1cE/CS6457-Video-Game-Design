using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class TimeManager : MonoBehaviour {

	float startTime = 0f;
	float endTime = 0f;

	Text remainTime;
	Slider timeSlider;

	public GameObject message;
	public GameObject mainCamera;

	UnityStandardAssets.ImageEffects.BlurOptimized blurControl;

	PostProcess.BlinkEffect eyeBlinkControl;

	public bool isReadyFlag = false;
	public bool timeOut = false;

	public float runTime;

	// Use this for initialization
	void Start () {

//		startTime = Time.time;
//		endTime = startTime + runTime;

		remainTime = GameObject.Find ("TimeNotify").GetComponent<Text>();
//		remainTime = remainTimeNotifier.GetComponent<Text>();
		remainTime.text = "Time Remain: " + runTime + "s";

		timeSlider = GameObject.Find ("Sanity").GetComponent<Slider>();
		timeSlider.maxValue = runTime;

		blurControl = mainCamera.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>();
		if (blurControl != null) {
			// Initialize the blur effect as NONE EFFECT
			// the effect will grow as the time passes
			blurControl.downsample = 0;
			blurControl.blurSize = 0f;
			blurControl.blurIterations = 1;

		}

		// Init the eye blink effect
		eyeBlinkControl = mainCamera.GetComponent<PostProcess.BlinkEffect> ();
		if (blurControl != null)
			eyeBlinkControl.time = 1f;

//		isReadyFlag = message.activeSelf;



	}

	// Update is called once per frame
	void Update () {

		float timeRemain;
		float timePassed;



		// Wait until the maze is ready
//		isReadyFlag = message.activeSelf;
		if (message.activeSelf) {
			startTime = Time.time;
			endTime = startTime + runTime;
			timeSlider.maxValue = runTime;

			isReadyFlag = true;
			message.SetActive(false);

		}
	
		if ((endTime - Time.time) >= 0f && isReadyFlag) {

			timeRemain = endTime - Time.time;
			timePassed = Time.time - startTime;

			// Blink eyes at the begining
			if (timePassed <= 1f && eyeBlinkControl.time >= 0f) {
				eyeBlinkControl.time = 1f - timePassed;
			} else {
				eyeBlinkControl.time = 0f;
			}

//			 // Blink eyes at the end
//			if (timeRemain <= 1f && eyeBlinkControl.time <= 1f) {
//				eyeBlinkControl.time = 1f - timeRemain;
//			} else {
//				eyeBlinkControl.time = 1f;
//			}
			
			remainTime.text = "Time Remain: " + (int)(timeRemain) + "s";
//		remainTime.text = string.Format("Time Remain: {0} s", int(endTime - Time.time));
			timeSlider.value = endTime - Time.time;

			// Update the blur effect
//			blurControl.downsample = (int)(2f * timePassed / runTime);
			blurControl.blurSize = (6f * timePassed / runTime); // Set max blursize to 6, or you can see nothing.
//			blurControl.blurIterations = 1 + (int)(3f * timePassed / runTime);
//
			if ((endTime - Time.time) <= 0.1f) {
				timeOut = true;
			}

		}

	}


}
