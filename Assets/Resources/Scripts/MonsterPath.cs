using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPath : MonoBehaviour {

	public int index;
	public float[] waitFrames;
	public float[] waitAngles;
	public GameObject prev, next;
	public GameObject owningMonster;

	void OnTriggerEnter(Collider monster) {
		owningMonster.GetComponent<Monster> ().waitAtAngleFor (index, waitFrames, waitAngles);
	}

	public void fetchNextTarget() {
		owningMonster.GetComponent<Monster> ().setNextTarget (index, prev, next);
	}
}
