using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScrollZ : MonoBehaviour {

	public float scrollSpeed = 20f;

	public bool autoScroll = true;

	private float mouseScroll = 0f;
	private float increase = 0f;

	Toggle toggle;

	// Use this for initialization
	void Start () {
		toggle = GameObject.Find ("Toggle").GetComponent<Toggle> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey ("escape")) {
			SceneManager.LoadScene ("MainMenu");
		}

		autoScroll = toggle.isOn;
		Vector3 pos = transform.position;

		Vector3 localVectorUp = transform.TransformDirection (0, 1, 0);
	

		if (autoScroll) {
			pos += localVectorUp * scrollSpeed * Time.deltaTime;
			transform.position = pos;
		} 
		else {
			pos += localVectorUp * mouseScroll;
			transform.position = pos;
		}

		increase = Input.GetAxis ("Mouse ScrollWheel");
		if (increase != 0) {
//			if (increase > 60f)
//				increase = 60f;
			mouseScroll += increase * 4;
			Debug.Log (mouseScroll);
		} 
		else {
			mouseScroll = mouseScroll / 1.5f;
		}



	}
}
