/* Copyright (c) 2008 Technicat, LLC.
   All Rights Reserved */
   
var start:Vector3; // start position

var fallHeight:float = 0;

var fallSound:AudioClip;

var isGetOut: GameObject;
var isReady:GameObject;

function Start() {
	isGetOut.active = false;
		var maze:Maze = FindObjectOfType(Maze);
		maze.MakeMaze();
		var tileSize:int = maze.tileSize;
		start.x = Mathf.Floor(tileSize*Random.Range(0,maze.width-1));
		start.z = Mathf.Floor(tileSize*Random.Range(0,maze.height-1));
		transform.position = start;
		transform.eulerAngles = Vector3(0,Random.Range(0,360),0);
		GetComponent.<AudioSource>().clip=fallSound;
		GetComponent.<AudioSource>().Play();

	isReady.active = true; 
}


function Update () {

	if (transform.localPosition.y<fallHeight) {

		isGetOut.active = true;

//		var maze:Maze = FindObjectOfType(Maze);
//		maze.MakeMaze();
//		var tileSize:int = maze.tileSize;
//		start.x = Mathf.Floor(tileSize*Random.Range(0,maze.width-1));
//		start.z = Mathf.Floor(tileSize*Random.Range(0,maze.height-1));
//		transform.position = start;
//		transform.eulerAngles = Vector3(0,Random.Range(0,360),0);
//		GetComponent.<AudioSource>().clip=fallSound;
//		GetComponent.<AudioSource>().Play();


	}

}
