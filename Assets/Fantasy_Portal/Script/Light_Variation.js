var minIntensity : float = 1.0;
var maxIntensity : float = 2.0;
var variationSpeed : float = 1.0;
var enableVariation : boolean = true;
var light2 : Light;
private var increase : boolean = true;
private var decrease : boolean;

function Start(){
	light2 = GetComponent("Light");
}
 
function Update () {
	if (increase == true)
		decrease = false;
	else
		decrease = true;

	if (enableVariation == true){
		if (increase == true)
			light2.intensity += variationSpeed*Time.deltaTime;
		if (decrease == true)
			light2.intensity -= variationSpeed*Time.deltaTime;
	}
	
	if (light2.intensity >= maxIntensity)
		increase = false;
	if (light2.intensity <= minIntensity)
		increase = true;
}