/* Copyright (c) 2007 Technicat, LLC */

var fadeTime:float = 5.0;

function Update() {
	GetComponent.<GUIText>().material.color.a -= Time.deltaTime/fadeTime;
	if (GetComponent.<GUIText>().material.color.a<0) {
		GetComponent.<GUIText>().enabled = false;
		this.enabled = false;
  }
}

