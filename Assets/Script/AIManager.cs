using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIManager : MonoBehaviour {
	public int ready;//how many bots are ready, referenced by bots
	public List<int> randList;
	public int totalPlaceChoice;//input!!!!!!! how many place choices in total
	public int specialFlag;

	public int ManDown;
	void Start () {
		ready=0;
		randList = getRandomList (totalPlaceChoice);
		specialFlag = 233;
		ManDown = 0;
	}

	// Update is called once per frame
	void Update () {}

	private List<int> getRandomList(int count){
		List<int> res = new List<int> ();
		for (int i = 0; i < count; i++) {
			res.Add (i);
		}
		for (int i = 0; i < count; i++) {
			int rnd = Random.Range (0, count);
			int temp = res [0];
			res [0] = res[rnd];
			res [rnd] = temp;
		}

		return res;
	}
}
