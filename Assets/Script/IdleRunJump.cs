using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IdleRunJump : MonoBehaviour {


	protected Animator animator;
	private bool gen;
	protected CapsuleCollider col;
	//public GameObject ragdoll;
	public float DirectionDampTime = .25f;
	public bool ApplyGravity = true;
	Vector3 movement; // // // // //
	Rigidbody playerRigidbody; // // // // //





	private GameObject disguiseObj;
	public SkinnedMeshRenderer mr1;
	public SkinnedMeshRenderer mr2;
	public CapsuleCollider theCollider;
	private bool notDisguise;//whether it is disguising or not
	private Vector3 lastPos;
	public GameObject obj1;
	public GameObject obj2;
	// // // // //

	float scale = 1.0f;//scale for speed

	/*
	public IEnumerator die()
	{
		playerRigidbody.mass = 0;
		if (gen == false) {
			gen = true;
			Vector3 old_pos = transform.position;
			animator.enabled = false;
			movement.Set (0f, -10000f, 0f);
			playerRigidbody.MovePosition (transform.position + movement);
			//Instantiate (ragdoll, (old_pos + new Vector3 (0, 0.1f, 0)), transform.rotation);	//Instantiates the prefab at the left foot + 0.05f (slightly above ground).

		}
		//movement.Set (0f, 10000f, 0f);
		//playerRigidbody.MovePosition (transform.position + movement);

		//Instantiate (ragdoll, (transform.position + new Vector3 (0, 0, 0)), transform.rotation);	//Instantiates the prefab at the left foot + 0.05f (slightly above ground).
		yield return new WaitForSeconds (5);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	*/

	// Use this for initialization
	void Start () 
	{
		notDisguise = true;

		gen = false;
		animator = GetComponent<Animator>();
		col = GetComponent<CapsuleCollider> ();
		if(animator.layerCount >= 2)
			animator.SetLayerWeight(1, 1);

		playerRigidbody = GetComponent <Rigidbody> (); // // // // //
	}

	// Update is called once per frame


	void Update () 
	{
		







		if (!notDisguise) {
			playerRigidbody.MovePosition (lastPos);
			if (Input.GetKeyDown ("0")) {
				//delete the obj
				Destroy(disguiseObj.gameObject);
				//
				theCollider.enabled = true;
				mr1.enabled = true;
				mr2.enabled = true;
				playerRigidbody.MovePosition (lastPos);
				notDisguise = true;
			}
			return;
		}


		if(notDisguise){
			if (Input.GetKeyDown ("1") || Input.GetKeyDown ("2") || Input.GetKeyDown ("3") || Input.GetKeyDown ("4") || Input.GetKeyDown ("5") || Input.GetKeyDown ("6") || Input.GetKeyDown ("7") || Input.GetKeyDown ("8") || Input.GetKeyDown ("9")) {
				lastPos = transform.position;

				theCollider.enabled = false;
				mr1.enabled = false;
				mr2.enabled = false;
				if (Input.GetKeyDown ("1")) {
					disguiseObj=Instantiate (obj1, lastPos, transform.rotation);
				}
				if (Input.GetKeyDown ("2")) {
					disguiseObj=Instantiate (obj2, lastPos, transform.rotation);
				}
				notDisguise = false;
				return;
			}







			
		// Raycast down from the center of the character.. 
		Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
		RaycastHit hitInfo = new RaycastHit();

		if (Physics.Raycast(ray, out hitInfo))
		{
			// ..if distance to the ground is more than 1.75, use Match Target
			if (hitInfo.distance > 1.75f)
			{

				// MatchTarget allows us to take over animation and smoothly transition our character towards a location - the hit point from the ray.
				// Here we're telling the Root of the character to only be influenced on the Y axis (MatchTargetWeightMask) and only occur between 0.35 and 0.5
				// of the timeline of our animation clip

				animator.MatchTarget(hitInfo.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), 0.35f, 0.5f);
			}
		}

		if (animator)
		{
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);			

			if (stateInfo.IsName("Base Layer.Locomotion"))
			{
				if (Input.GetButton ("Fire1")) {
					animator.SetBool ("Jump", true); 
				}
			}
			else
			{
				animator.SetBool("Jump", false);                
			}

			if(Input.GetButtonDown("Fire2") && animator.layerCount >= 2)
			{
				animator.SetBool("Hi", !animator.GetBool("Hi"));
			}

			float h = Input.GetAxis("Horizontal");
			float h2 = Input.GetAxis("Horizontal2");
			float v = Input.GetAxis("Vertical");

			//GUILayout.Label("Speed: " + h);

			if (v < 0) {
				animator.SetFloat("Speed", -(v*v)*scale);
			} 
			else {				
				animator.SetFloat("Speed", v*v*scale);
			}

			if (h != 0.0f) {
				animator.SetFloat ("Direction", h, DirectionDampTime, Time.deltaTime);	
			} 
			//else if (h2 != 0.0f) {
			//	animator.SetFloat ("Direction", h2 / 2, DirectionDampTime, Time.deltaTime);	
			//} 
			else {
				animator.SetFloat ("Direction", 0.0f, DirectionDampTime, Time.deltaTime);	
			}

		}  
		}
	}

}
