using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructTimer: MonoBehaviour {

	private float timer = -1.0f;

	void Start() {
		//timer = 1.0f;
	}

	// Update is called once per frame
	void Update () {

		if (timer != -1) {
			if (timer <= 0) {
				Destroy (this.gameObject);
			} else if (timer > 0) {
				timer -= Time.deltaTime;
			}
		}
	}

	public void setTimer(float timer) {
		this.timer = timer;
	}
}