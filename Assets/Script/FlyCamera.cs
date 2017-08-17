﻿using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour {

	/*
    */


	float mainSpeed = 30.0f; //regular speed
	float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
	float maxShift = 1000.0f; //Maximum speed when holdin gshift
	float camSens = 0.25f; //How sensitive it with mouse
	float screenWidth = Screen.width;
	float screenHeight = Screen.height;
	float edgeBoundary = 5;
	private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
	private float totalRun= 1.0f;

	void Update () {
		lastMouse = Input.mousePosition - lastMouse ;
		lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 );
		lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x , transform.eulerAngles.y + lastMouse.y, 0);
		transform.eulerAngles = lastMouse;
		lastMouse =  Input.mousePosition;
		//Mouse  camera angle done.  

		//Keyboard commands
		float f = 0.0f;
		Vector3 p = GetBaseInput();
		if (Input.GetKey (KeyCode.LeftShift)){
			totalRun += Time.deltaTime;
			p  = p * totalRun * shiftAdd;
			p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
			p.y = Mathf.Max( Mathf.Clamp(p.y, -maxShift, maxShift), 1);
			p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
		}
		else{
			totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
			p = p * mainSpeed;
		}

		if (Input.mousePosition.x > screenWidth-edgeBoundary)
		{
			transform.Rotate(Vector3.up * Time.deltaTime * 50f);
		}
		if (Input.mousePosition.x < 0 + edgeBoundary)
		{
			transform.Rotate(-Vector3.up * Time.deltaTime * 50f);
		}

		p = p * Time.deltaTime;
		//p.y = 10;
		Vector3 newPosition = transform.position;
		transform.Translate(p);
		newPosition.x = transform.position.x;
		newPosition.z = transform.position.z;
		newPosition.y = 2;
		transform.position = newPosition;

	}

	private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
		Vector3 p_Velocity = new Vector3();
		if (Input.GetKey (KeyCode.W)){
			p_Velocity += new Vector3(0, 0 , 1);
		}
		if (Input.GetKey (KeyCode.S)){
			p_Velocity += new Vector3(0, 0, -1);
		}
		if (Input.GetKey (KeyCode.A)){
			p_Velocity += new Vector3(-1, 0, 0);
		}
		if (Input.GetKey (KeyCode.D)){
			p_Velocity += new Vector3(1, 0, 0);
		}
		return p_Velocity;
	}
}