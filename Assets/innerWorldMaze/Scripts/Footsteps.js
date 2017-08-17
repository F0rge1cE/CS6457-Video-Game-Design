var audioStepLength = 0.3;
var walkSounds:AudioClip[];

function Start ()
{
	var controller : CharacterController = GetComponent(CharacterController);

	while (true)
	{
		if (controller.isGrounded && controller.velocity.magnitude > 0.3)
		{
			GetComponent.<AudioSource>().clip = walkSounds[Random.Range(0, walkSounds.length)];
			GetComponent.<AudioSource>().Play();
			yield WaitForSeconds(audioStepLength);
		}
		else
		{
			yield;
		}
	}
}
