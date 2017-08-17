//Marshall Heffernan aka itsmars 2015
//Special thanks to Duck, TheKusabi, alucardj, dorpeleg, TonyLi & superpig on the Unity Forums!

//This is the CURVES version of my Footstep script. For this to work, you'll first need to...

//1) Add two floats to your Animator Controller. "FootstepLeft" and "FootstepRight".
//2) Add a CURVE to each footstep animation. See video tutorial for a better explaination.
//3) Tag your Terrain GameObject with "Terrain".
//4) Tag your Non-Terrain GameObject floors with their respective names (for example, a wooden floor would need the tag "Surface_Wood").

//Watch the video tutorial! https://www.youtube.com/watch?v=ISoBKFxQLic

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
//This will add an Audio Source component if you don't have one.

public class FootstepMaster_Curves : MonoBehaviour {

	private GameObject terrainFinder;	//Stores the Terrain GameObject out of the Scene for use later.
	private Terrain terrain;			//Your Terrain (if one's in your scene)
	private TerrainData terrainData;	//Lets us get to the Terrain's splatmap.
	private Vector3 terrainPos;			//Where are we on the Splatmap?
	public static int surfaceIndex = 0;	//The order in which your textures were added to your Terrain.
	private string whatTexture;  		//Holds the FILENAMEs of the Textures in your Terrain.

	private AudioSource mySound;		//The AudioSource component
	private Animator anim;				//Lets us pull floats out of our Animator's curves.
	public static GameObject floor;		//what are we standing on?
	private string currentFoot;			//Each foot does it's own Raycast

	private float currentFrameFootstepLeft;
	private float currentFrameFootstepRight;
	private float lastFrameFootstepLeft;
	private float lastFrameFootstepRight;

	//-----------------------------------------------------------------------------------------

	[Space(5.0f)]
	private float currentVolume;
	[Range(0.0f,1.0f)]
	public float volume = 1.0f;				//Volume slider bar; set this between 0 and 1 in the Inspector.
	[Range(0.0f,0.2f)]
	public float volumeVariance = 0.04f;	//Variance in volume levels per footstep; set this between 0.0 and 0.2 in the inspector. Default is 0.04f.
	private float pitch;
	[Range(0.0f,0.2f)]
	public float pitchVariance = 0.08f;		//Variance in pitch levels per footstep; set this between 0.0 and 0.2 in the inspector. Default is 0.08f.
	[Space(5.0f)]
	public GameObject leftFoot;			//Drag your player's RIG/MESH/BIP/BONE for the left foot here, in the inspector.
	public GameObject rightFoot;		//Drag your player's RIG/MESH/BIP/BONE for the right foot here, in the inspector.
	[Space(5.0f)]
	public AudioClip[] Sgrass = new AudioClip[0];
	public AudioClip[] Sstair = new AudioClip[0];
	public AudioClip[] Sground = new AudioClip[0];
	public AudioClip[] Siron = new AudioClip[0];

	[Space(5.0f)]
	[Tooltip("Choose ONE")]
	public bool instantiatedFX;		//Check this checkbox in the inspector if you want your FX to be instantiated.
	[Tooltip("Choose ONE")]
	public bool toggledFX;			//Check this checkbox in the inspector if you want your FX to be enabled.

	[Space(5.0f)]
	public GameObject grassFX;
	public GameObject stairFX;
	public GameObject groundFX;
	private Quaternion dirtRotation;
	private Quaternion snowRotation;
	private Quaternion waterRotation;
	private Vector3 dirtPos;
	private Vector3 snowPos;
	private Vector3 waterPos;

	//-----------------------------------------------------------------------------------------

	//Start
	void Start () {
		anim = GetComponent<Animator>();
		mySound = gameObject.GetComponent<AudioSource>();
		/*terrainFinder = GameObject.FindGameObjectWithTag ("Terrain");

		if (terrainFinder != null) {	//IS THERE A TERRAIN IN THE SCENE?
			terrain = Terrain.activeTerrain;
			terrainData = terrain.terrainData;
			terrainPos = terrain.transform.position;
		}*/
	}

	//-----------------------------------------------------------------------------------------

	//Update
	void Update () {
		if (terrainFinder != null) {	//IS THERE A TERRAIN IN THE SCENE?
			surfaceIndex = GetMainTexture (transform.position);
			//Not that it matters, but here we determine what position the Terrain Textures are in.
			//For example, If you added a grass texture, then a dirt, then a rock, you'd have grass=0, dirt=1, rock=2.
			whatTexture = terrainData.splatPrototypes [surfaceIndex].texture.name;
			//Instead of messing around with numbers, we'll just check the texture's filename.
		}
		if (grassFX != null) {
			grassFX.transform.rotation = dirtRotation;	//lock the dirt FX particle system's rotation so it doesn't spin (only applies to TOGGLE mode).
			grassFX.transform.position = dirtPos;		//lock the dirt FX particle system's position so it doesn't follow you as you move (only applies to TOGGLE mode).
		}
		if (stairFX != null) {
			stairFX.transform.rotation = snowRotation;
			stairFX.transform.position = snowPos;
		}
		if (groundFX != null) {
			groundFX.transform.rotation = waterRotation;
			groundFX.transform.position = waterPos;
		}

		//-----------------------------------------------------------------------------------------

		//Check THIS FRAME to see if we need to play a sound for the left foot, RIGHT NOW...
		currentFrameFootstepLeft = anim.GetFloat ("FootstepLeft");		//get left foot's CURVE FLOAT from the Animator Controller, from the LAST FRAME.
		if (currentFrameFootstepLeft > 0 && lastFrameFootstepLeft < 0) {		//is this frame's curve BIGGER than the last frames?
			RaycastHit surfaceHitLeft;
			Ray aboveLeftFoot = new Ray (leftFoot.transform.position + new Vector3 (0, 1.5f, 0), Vector3.down);
			LayerMask layerMask = ~(1 << 18) | (1 << 19); 	//Here we ignore layer 18 and 19 (Player and NPCs). We want the raycast to hit the ground, not people.
			if (Physics.Raycast (aboveLeftFoot, out surfaceHitLeft, 2f, layerMask)) {
				floor = (surfaceHitLeft.transform.gameObject);
				currentFoot = "Left";				//This will help us place the Instantiated or Toggled FX at the correct position.
				if (floor != null) {
					Invoke ("CheckTexture", 0);		//Play LEFT FOOTSTEP
				}
			}
		}
		lastFrameFootstepLeft = anim.GetFloat ("FootstepLeft");	//get left foot's CURVE FLOAT from the Animator Controller, from the CURRENT FRAME.

		//-----------------------------------------------------------------------------------------

		//Check THIS FRAME to see if we need to play a sound for the right foot, RIGHT NOW...
		currentFrameFootstepRight = anim.GetFloat ("FootstepRight");	//get right foot's CURVE FLOAT from the Animator Controller, from the LAST FRAME.
		if (currentFrameFootstepRight < 0 && lastFrameFootstepRight > 0) {		//is this frame's curve SMALLER than last frames?
			RaycastHit surfaceHitRight;
			Ray aboveRightFoot = new Ray (rightFoot.transform.position + new Vector3 (0, 1.5f, 0), Vector3.down);
			LayerMask layerMask = ~(1 << 18) | (1 << 19); 	//Here we ignore layer 18 and 19 (Player and NPCs). We want the raycast to hit the ground, not people.
			if (Physics.Raycast (aboveRightFoot, out surfaceHitRight, 2f, layerMask)) {
				floor = (surfaceHitRight.transform.gameObject);
				currentFoot = "Right";				//This will help us place the Instantiated or Toggled FX at the correct position.
				if (floor != null) {
					Invoke ("CheckTexture", 0);		//Play RIGHT FOOTSTEP
				}
			}
		}																																		//???????????????????????????????????????????????????????
		lastFrameFootstepRight = anim.GetFloat ("FootstepRight");	//get right foot's CURVE FLOAT from the Animator Controller, from the CURRENT FRAME.
	} //END OF UPDATE

	//----------------------------------------------------------------------------------------

	//Puts ALL TEXTURES from the Terrain into an array, represented by floats (0=first texture, 1=second texture, etc).
	private float[] GetTextureMix(Vector3 WorldPos){
		if (terrainFinder != null) {	//IS THERE A TERRAIN IN THE SCENE?
			// calculate which splat map cell the worldPos falls within
			int mapX = (int)(((WorldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
			int mapZ = (int)(((WorldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);
			// get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
			float[,,] splatmapData = terrainData.GetAlphamaps (mapX, mapZ, 1, 1);
			float[] cellMix = new float[ splatmapData.GetUpperBound (2) + 1 ]; //turn splatmap data into float array
			for (int n=0; n<cellMix.Length; n++) {
				cellMix [n] = splatmapData [0, 0, n];
			}return cellMix;
		} else return null;	//THERE'S NO TERRAIN IN THE SCENE! DON'T DO THE ABOVE STUFF.
	}

	//Takes the "GetTextureMix" float array from above and returns the MOST DOMINANT texture at Player's position.
	private int GetMainTexture(Vector3 WorldPos){
		if (terrainFinder != null) {	//IS THERE A TERRAIN IN THE SCENE?
			float[] mix = GetTextureMix (WorldPos);
			float maxMix = 0;
			int maxIndex = 0;
			for (int n=0; n<mix.Length; n++){
				if (mix [n] > maxMix){
					maxIndex = n;
					maxMix = mix [n];
				}
			}return maxIndex;
		} else return 0;	//THERE'S NO TERRAIN IN THE SCENE! DON'T DO THE ABOVE STUFF.
	}

	//-----------------------------------------------------------------------------------------

	void CheckTexture(){
		//First we'll check to see if the player just stepped on a NON-TERRAIN GameObject. TAG YOUR FLOORS APPROPRIATELY (for example, tag a wooden floor with "Surface_Wood").
		//Feel free to make your own sound categories.

		if (floor.tag == ("grass"))
			Invoke ("PlayGrass", 0);
		if (floor.tag == ("stair"))
			Invoke ("PlayStair", 0);
		if (floor.tag == ("ground"))
			Invoke ("PlayGround", 0);	
		if (floor.tag == ("iron"))
			Invoke ("PlayIron", 0);
	}

	//-----------------------------------------------------------------------------------------

	//INVOKED FUNCTIONS BELOW. Plays a Random sound from an array at a varied pitch and volume.

	//I've included support for three FX Prefabs below (a dust cloud for dirt, white puffs for snow and splashes for water). Feel free to add more.

	//If you're Instantiating your FX, make sure it has a script to delete itself after a while, to keep your Hierarchy clean. Drag the prefab into the inspector FROM YOUR PROJECT WINDOW.
	//If you're Toggling your FX, just make sure it's a short enough particle system so that it won't look wierd when it turns off, abruptly. Drag the FX into the inspector FROM YOUR PLAYER.

	void PlayGrass(){
		currentVolume = (volume + UnityEngine.Random.Range(-volumeVariance, volumeVariance));
		pitch = (1.0f + Random.Range(-pitchVariance, pitchVariance));
		mySound.pitch = pitch;
		if (Sgrass.Length > 0) {
			mySound.PlayOneShot (Sgrass [Random.Range (0, Sgrass.Length)], currentVolume);
		} else Debug.LogError ("trying to play grass sound, but no grass sounds in array!");

		if (grassFX != null) {		//Is a prefab in the GameObject box, in the Inspector?
			if (instantiatedFX == true) {						//Code for INSTANTIATING a Particle System at the player's feet.
				if (currentFoot == ("Left")) {
					Instantiate (grassFX, (leftFoot.transform.position + new Vector3 (0, 0.05f, 0)), transform.rotation);	//Instantiates the prefab at the left foot + 0.05f (slightly above ground).
				} else if (currentFoot == ("Right")) {
					Instantiate (grassFX, (rightFoot.transform.position + new Vector3 (0, 0.05f, 0)), transform.rotation);	//Instantiates the prefab at the right foot + 0.05f (slightly above ground).
				}
			}
			if (toggledFX == true) {							//Code for ENABLING a Particle System at the player's feet.
				if (currentFoot == ("Left")) {
					if (grassFX != null) {
						grassFX.SetActive (false);
						grassFX.SetActive (true);
						grassFX.transform.position = leftFoot.transform.position;
						dirtRotation = grassFX.transform.rotation;
						dirtPos = grassFX.transform.position;
					}
				} else if (currentFoot == ("Right")) {
					if (grassFX != null) {
						grassFX.SetActive (false);
						grassFX.SetActive (true);
						grassFX.transform.position = rightFoot.transform.position;
						dirtRotation = grassFX.transform.rotation;
						dirtPos = grassFX.transform.position;
					}
				}
			}
		}


	}

	void PlayStair(){
		currentVolume = (volume + UnityEngine.Random.Range(-volumeVariance, volumeVariance));
		pitch = (1.0f + Random.Range(-pitchVariance, pitchVariance));
		mySound.pitch = pitch;
		if (Sstair.Length > 0) {
			mySound.PlayOneShot (Sstair [Random.Range (0, Sstair.Length)], currentVolume);
		} else Debug.LogError ("trying to play grass sound, but no grass sounds in array!");


		if (stairFX != null) {		//Is a prefab in the GameObject box, in the Inspector?
			if (instantiatedFX == true) {						//Code for INSTANTIATING a Particle System at the player's feet.
				if (currentFoot == ("Left")) {
					Instantiate (stairFX, (leftFoot.transform.position + new Vector3 (0, 0.05f, 0)), transform.rotation);	//Instantiates the prefab at the left foot + 0.05f (slightly above ground).
				} else if (currentFoot == ("Right")) {
					Instantiate (stairFX, (rightFoot.transform.position + new Vector3 (0, 0.05f, 0)), transform.rotation);	//Instantiates the prefab at the right foot + 0.05f (slightly above ground).
				}
			}
			if (toggledFX == true) {							//Code for ENABLING a Particle System at the player's feet.
				if (currentFoot == ("Left")) {
					if (stairFX != null) {
						stairFX.SetActive (false);
						stairFX.SetActive (true);
						stairFX.transform.position = leftFoot.transform.position;
						dirtRotation = stairFX.transform.rotation;
						dirtPos = stairFX.transform.position;
					}
				} else if (currentFoot == ("Right")) {
					if (stairFX != null) {
						stairFX.SetActive (false);
						stairFX.SetActive (true);
						stairFX.transform.position = rightFoot.transform.position;
						dirtRotation = stairFX.transform.rotation;
						dirtPos = stairFX.transform.position;
					}
				}
			}
		}



	}

	void PlayGround(){
		currentVolume = (volume + UnityEngine.Random.Range(-volumeVariance, volumeVariance));
		pitch = (1.0f + Random.Range(-pitchVariance, pitchVariance));
		mySound.pitch = pitch;
		if (Sground.Length > 0) {
			mySound.PlayOneShot (Sground [Random.Range (0, Sground.Length)], currentVolume);
		} else Debug.LogError ("trying to play grass sound, but no grass sounds in array!");

		if (groundFX != null) {		//Is a prefab in the GameObject box, in the Inspector?
			if (instantiatedFX == true) {						//Code for INSTANTIATING a Particle System at the player's feet.
				if (currentFoot == ("Left")) {
					Instantiate (groundFX, (leftFoot.transform.position + new Vector3 (0, 0.05f, 0)), transform.rotation);	//Instantiates the prefab at the left foot + 0.05f (slightly above ground).
				} else if (currentFoot == ("Right")) {
					Instantiate (groundFX, (rightFoot.transform.position + new Vector3 (0, 0.05f, 0)), transform.rotation);	//Instantiates the prefab at the right foot + 0.05f (slightly above ground).
				}
			}
			if (toggledFX == true) {							//Code for ENABLING a Particle System at the player's feet.
				if (currentFoot == ("Left")) {
					if (groundFX != null) {
						groundFX.SetActive (false);
						groundFX.SetActive (true);
						groundFX.transform.position = leftFoot.transform.position;
						dirtRotation = groundFX.transform.rotation;
						dirtPos = groundFX.transform.position;
					}
				} else if (currentFoot == ("Right")) {
					if (groundFX != null) {
						groundFX.SetActive (false);
						groundFX.SetActive (true);
						groundFX.transform.position = rightFoot.transform.position;
						dirtRotation = groundFX.transform.rotation;
						dirtPos = groundFX.transform.position;
					}
				}
			}
		}


	}

	void PlayIron(){
		currentVolume = (volume + UnityEngine.Random.Range(-volumeVariance, volumeVariance));
		pitch = (1.0f + Random.Range(-pitchVariance, pitchVariance));
		mySound.pitch = pitch;
		if (Siron.Length > 0) {
			mySound.PlayOneShot (Siron [Random.Range (0, Siron.Length)], currentVolume);
		} else Debug.LogError ("trying to play grass sound, but no grass sounds in array!");


	}

	/*
	void PlayDirt(){		//Supports toggling or instantiating a "dirtFX" AT THE PLAYER'S CURRENT FOOT which could be a dust cloud particle system, or something...
		currentVolume = (volume + UnityEngine.Random.Range (-volumeVariance, volumeVariance));
		pitch = (1.0f + Random.Range (-pitchVariance, pitchVariance));
		mySound.pitch = pitch;
		if (dirt.Length > 0) {
			mySound.PlayOneShot (dirt [Random.Range (0, dirt.Length)], currentVolume);		//play a random sound from it's corresponding array.
		} else Debug.LogError ("trying to play dirt sound, but no dirt sounds in array!");
		if (dirtFX != null) {		//Is a prefab in the GameObject box, in the Inspector?
			if (instantiatedFX == true) {						//Code for INSTANTIATING a Particle System at the player's feet.
				if (currentFoot == ("Left")) {
					Instantiate (dirtFX, (leftFoot.transform.position + new Vector3 (0, 0.05f, 0)), transform.rotation);	//Instantiates the prefab at the left foot + 0.05f (slightly above ground).
				} else if (currentFoot == ("Right")) {
					Instantiate (dirtFX, (rightFoot.transform.position + new Vector3 (0, 0.05f, 0)), transform.rotation);	//Instantiates the prefab at the right foot + 0.05f (slightly above ground).
				}
			}
			if (toggledFX == true) {							//Code for ENABLING a Particle System at the player's feet.
				if (currentFoot == ("Left")) {
					if (dirtFX != null) {
						dirtFX.SetActive (false);
						dirtFX.SetActive (true);
						dirtFX.transform.position = leftFoot.transform.position;
						dirtRotation = dirtFX.transform.rotation;
						dirtPos = dirtFX.transform.position;
					}
				} else if (currentFoot == ("Right")) {
					if (dirtFX != null) {
						dirtFX.SetActive (false);
						dirtFX.SetActive (true);
						dirtFX.transform.position = rightFoot.transform.position;
						dirtRotation = dirtFX.transform.rotation;
						dirtPos = dirtFX.transform.position;
					}
				}
			}
		}
	}
	*/


}