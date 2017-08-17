
function Update () {
	if (!Options.IsGamePaused() && Input.GetButtonDown("Fire1")) {
		GetComponent.<Light>().enabled = !GetComponent.<Light>().enabled;
	}
}