using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class AIController : MonoBehaviour {
	public Transform[] waypointSetHide; //hide area
	public Transform waypointStart;

	//the following members are for the "disguise"
	private GameObject disguiseObj;//the object you disguised, I will take care of it - Shiyi
	public SkinnedMeshRenderer mr1;//mesh render 1, I will take care of it
	public SkinnedMeshRenderer mr2;//mesh render 2, I will take care of it
	public CapsuleCollider theCollider;//capsucollider of AI
	private Vector3 lastPos;//use it to keep it fixed
	private Vector3 tmp;
	public GameObject[] listObj;//list of objects you can disguise
	Rigidbody botRigidbody;//the rigid body of this bot
	//end
	public int choice;
	public GameObject player;
	private Animator playerAnim;
	private bool firstTime;
	public GameObject hunter;

	public AIManager aiManager;

	public int MAXBOTAMOUNT;//INPUT!!!!!!!!!!!!!!!!!!!!!!!!!!
	public int order;//INPUT!!!!!!!!!!!!!!!!!!!!!!!!!!! 1-BASE

	NavMeshAgent nav;               // Reference to the nav mesh agent.


	public enum AIState
	{
		Start,
		GoToHide,
		Transform,
		Hiding,
		Caught,
		Chase
	};
	public AIState aiState;
	AINavSteeringController aiSteer;

	// Use this for initialization
	void Start () {
		firstTime = true;
		aiSteer = GetComponent<AINavSteeringController>();
		botRigidbody = GetComponent<Rigidbody> ();
		//print (aiSteer);
		aiSteer.Init();
		aiSteer.waypointLoop = false;
		aiSteer.stopAtNextWaypoint = false;
		aiSteer.setWayPoint (waypointStart);
		aiState = AIState.Start;
		player = GameObject.FindGameObjectWithTag("Player");
		playerAnim = player.GetComponent<Animator>();
		nav = GetComponent <NavMeshAgent> ();
	}

	// Update is called once per frame
	void Update () {
		print (aiState);
		switch (aiState) 
		{
		case AIState.Start:
			//GoToStart ();
			if (aiSteer.waypointsComplete ()) {
				print (aiManager.specialFlag);
				if(aiManager.specialFlag==233)
					TransitionToState_GoToHide ();
			}
			break;

		case AIState.GoToHide:
			if (aiSteer.waypointsComplete ()&&firstTime) {
				aiManager.ready += 1;
				firstTime = false;
			}
			if(aiManager.ready==MAXBOTAMOUNT)
				TransitionToState_Transform ();
			break;
		case AIState.Transform:
			ToTransform ();
			break;
		case AIState.Hiding:
			//assume AI bot has finished disguising
			botRigidbody.MovePosition(lastPos);//move it to the last position before disguising, in case it will sink
			AnimatorStateInfo currentPlayerState = playerAnim.GetCurrentAnimatorStateInfo (0);
			int attackState = Animator.StringToHash("Base.Attack");
			if (currentPlayerState.IsName("Attack")) {
				//print (Vector3.Distance (player.transform.position, this.transform.position));
				Vector3 center = disguiseObj.GetComponent<Renderer>().bounds.center;
				float radius = disguiseObj.GetComponent<Renderer> ().bounds.extents.magnitude;
				if (Vector3.Distance (player.transform.position, center) < 1.5*radius) {
					HealthManager hm = player.GetComponent<HealthManager> ();
					hm.successTries += 1;
					changeBack ();
					//ToCaught ();
					StartCoroutine(showPopupText(10));
					aiManager.ManDown += 1;
				} 
			}
			break;
		case AIState.Chase:
			chase ();
			break;
		default:
			break;
		}

	}
	void GoToStart(){
		aiState = AIState.Start;
		aiSteer.setWayPoint (waypointStart);
		aiSteer.useNavMeshPathPlanning = true;
	}
	void TransitionToState_GoToHide(){
		aiState = AIState.GoToHide;
		//int hideplace = Random.Range (0, waypointSetHide.Length);
		int hideplace = aiManager.randList[order-1];
		if (hideplace>=0 && hideplace<=5)
			choice = 1;
		if (hideplace>=6 && hideplace<=10)
			choice = 2;
		if (hideplace>=11 && hideplace<=13)
			choice = 3;
		if (hideplace>=14 && hideplace<=15)
			choice = 4;
		if (hideplace>=16 && hideplace<=17)
			choice = 5;
		if (hideplace>=18 && hideplace<=22)
			choice = 6;
		tmp = new Vector3 (waypointSetHide[hideplace].position.x,transform.position.y,waypointSetHide[hideplace].position.z);
		transform.position = tmp;
		aiSteer.setWayPoint (waypointSetHide[hideplace]);
	}
	void TransitionToState_Transform(){
		aiState = AIState.Transform;
	}
	void ToTransform(){
		disguise (choice, new Vector3(0.0f,-0.1f,0.0f));
		aiState = AIState.Hiding;
	}

	void ToCaught(){
		aiState = AIState.Caught;
	}

	//the following 2 methods are for disguise and change-back
	void disguise(int choice, Vector3 shift){
		/*
        function:
        given the decision of which object to disguise, this part will:
        1.disable the collider
        2.make the AI bot invisible
        3.generate a object at your position

        @parameters:
        int choice: which object you want to disguise, this should allign with the list of the disguising game object
        */
		lastPos = transform.position;//keep record of last position
		theCollider.enabled = false;//disable collider
		mr1.enabled = false;//disable mesh render
		mr2.enabled = false;//disable mesh render
		disguiseObj=Instantiate (listObj[choice-1], lastPos+shift, listObj[choice-1].transform.rotation);//disguise obj, and keep it
		return;
	}

	void changeBack(){
		/*
        function:
        when being called, this part will:
        1.destroy the game object (the one you "disguise" as)
        2.enable the AI bot's collider
        3.make the AI bot visible again
        */
		Destroy(disguiseObj.gameObject);// delete the disguised game object
		//theCollider.enabled = true;//enable collider
		//mr1.enabled = true;//mesh render
		//mr2.enabled = true;//mesh render
		//botRigidbody.MovePosition (lastPos);//fix position
		hunter = GameObject.Find ("Mutant");
		Instantiate(hunter, transform.position, hunter.transform.rotation);
		aiState = AIState.Chase;
		return;
	}
	void chase(){
		//change to enemy
		// ... set the destination of the nav mesh agent to the player.
		//nav.SetDestination (hunter.position);
	}

	IEnumerator showPopupText(float duration){
		GameObject caughtText = GameObject.Find ("CaughtText");

		Text text = caughtText.GetComponent<Text> ();
		text.text = "You successfully caught an alien hider!!";
		text.enabled = true;
		yield return new WaitForSeconds(duration);
		text.enabled = false;
	}
}
