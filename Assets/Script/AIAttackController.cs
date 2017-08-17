using System;
using UnityEngine;

public class AIAttackController : MonoBehaviour
{
	private GameObject player;
	private Animator anim;

	private bool canDamage;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		anim = GetComponent<Animator> ();
		canDamage = true;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "portal") {
			GameObject.Find ("EnemyCountManager").GetComponent<EnemyCountManager> ().enemiesKilled++;
			Destroy (this.gameObject);
		}
	}

	private void Update()
	{
		AnimatorStateInfo currentAIstate = anim.GetCurrentAnimatorStateInfo (0);
		int kickState = Animator.StringToHash("Base.Kick");
		if (!currentAIstate.IsName ("Kick")) {
			canDamage = true;
			if (Vector3.Distance (player.transform.position, this.transform.position) < 1) {
				anim.SetBool ("Kick", true);
			} else {
				anim.SetBool ("Kick", false);
			}
		} else {
			if (Vector3.Distance (player.transform.position, this.transform.position) < 1) {
				// Kick on player, player lose HP
				if (canDamage) {
					HealthManager hm = player.GetComponent<HealthManager> ();
					hm.totalTries += 1;

					// Play the player's hurted audio.
					player.GetComponent<AudioManager> ().hurted();
					canDamage = false;
				}
			}
			anim.SetBool("Kick", false);
		}
	}

}
