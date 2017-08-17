

var startColor = Color.black;
var endColor = Color.white;
var timeScale = 1.0;

function Update () {
	GetComponent.<GUITexture>().color = Color.Lerp(startColor,endColor,timeScale*Mathf.PingPong(Time.time,1.0/timeScale));
}
