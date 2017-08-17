using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WalkRun : MonoBehaviour {

	public float v;
	public float h;
	public float qe;
	public float sprint;
	private Animator animator;
	private float turnspeed = 100f;
	private float lerpSpeed = 0.1f;
	private AnimatorStateInfo currentBaseState;
	private CharacterController controller;
	private Vector3 moveVector;
	private GameObject terrain;
	private Text speedText;
	private float timePassed = 0f;
	private float keyDelay = 0.3f;
	static int runState = Animator.StringToHash("Base Layer.Sprinting");


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		controller = GetComponent<CharacterController> ();
		speedText = GameObject.Find ("Speed").GetComponent<Text> ();
		speedText.text = "Current Speed Mode: 1";
		terrain = GameObject.Find("Terrain");
	}


	void FixedUpdate(){
		timePassed += Time.deltaTime;
		currentBaseState = animator.GetCurrentAnimatorStateInfo (0);

		v = Input.GetAxis ("Vertical");
		h = Input.GetAxis ("Horizontal");
		qe = Input.GetAxis ("QE");
		Sprinting ();
		animator.SetFloat ("Walk", v);
		if(h>0 || qe>0)  
			animator.SetFloat ("Turn", Mathf.Max(h,qe*0.5f));
		if (h < 0 || qe < 0)
			animator.SetFloat ("Turn", Mathf.Min (h, qe * 0.5f));
		if (Mathf.Abs (h) < 1e-6 && Mathf.Abs (qe) < 1e-6)
			animator.SetFloat ("Turn", 0);
		animator.SetBool ("Jump", false);

		if (Input.GetKeyDown ("0")) {
			animator.SetFloat ("Sprint", 1);
			speedText.text = "Current Speed Mode: 10";
		}
		if (Input.GetKeyDown ("1")) {
			animator.SetFloat ("Sprint", 0f);
			speedText.text = "Current Speed Mode: 1";
		}
		if (Input.GetKeyDown ("2")) {
			animator.SetFloat ("Sprint", 0.1f);
			speedText.text = "Current Speed Mode: 2";
		}
		if (Input.GetKeyDown ("3")) {
			animator.SetFloat ("Sprint", 0.2f);
			speedText.text = "Current Speed Mode: 3";
		}
		if (Input.GetKeyDown ("4")) {
			animator.SetFloat ("Sprint", 0.3f);
			speedText.text = "Current Speed Mode: 4";
		}
		if (Input.GetKeyDown ("5")) {
			animator.SetFloat ("Sprint", 0.4f);
			speedText.text = "Current Speed Mode: 5";
		}
		if (Input.GetKeyDown ("6")) {
			animator.SetFloat ("Sprint", 0.5f);
			speedText.text = "Current Speed Mode: 6";
		}
		if (Input.GetKeyDown ("7")) {
			animator.SetFloat ("Sprint", 0.6f);
			speedText.text = "Current Speed Mode: 7";
		}
		if (Input.GetKeyDown ("8")) {
			animator.SetFloat ("Sprint", 0.7f);
			speedText.text = "Current Speed Mode: 8";
		}
		if (Input.GetKeyDown ("9")) {
			animator.SetFloat ("Sprint", 0.8f);
			speedText.text = "Current Speed Mode: 9";
		}


		

		if (Mathf.Abs (v) < 1e-6 && timePassed >= keyDelay) {
			
			if (Input.GetKeyDown ("a")) {
				transform.Rotate (0, -90, 0);
				timePassed = 0f;
			}
			if (Input.GetKeyDown ("d")) {
				transform.Rotate (0, 90, 0);
				timePassed = 0f;
			}
			if (Input.GetKey ("q")) {
				transform.Rotate(0,-1f,0);
			}
			if (Input.GetKey ("e")) {
				transform.Rotate(0,1f,0);
			}
		}
			
		if(currentBaseState.fullPathHash == runState)
			if (Input.GetButtonDown ("Jump"))
				animator.SetBool ("Jump", true);

		if (controller.isGrounded) {
			moveVector.y = 0.001f;
			animator.SetBool ("Falling", false);
		}
		else {
			moveVector += 0.01f*Physics.gravity * Time.deltaTime;
			RaycastHit hit;
			if (Physics.Raycast (transform.position, -transform.up, out hit, 500f)) {
				print (hit.distance);
				if (hit.distance > 10) {  
					animator.SetBool ("Falling", true);
				}
			}
		}
		controller.Move(moveVector);


	}

	void Sprinting(){
		if (Input.GetButton ("Fire1")) {
			sprint = 0.2f;
		} else {
			sprint = 0.0f;
		}
	}
}
