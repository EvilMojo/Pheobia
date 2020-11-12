using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	public GameObject target;
	public GameObject player;

	public float startTime;
	public float journeyLength;
	public float cameraSpeed;

	public int duration;

	public bool returningToPlayer;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("playerCharacter");
		player = GameObject.Find ("playerCharacter");
		journeyLength = Vector3.Distance (this.transform.position, target.transform.position);
		//this.transform.parent = playerCharacter.transform;
		startTime = Time.time;
		cameraSpeed = 1.0f;
	
		returningToPlayer = true;
	}
	
	// Update is called once per frame
	void Update () {
		//float moveX = 0;
		//float moveY = 0;
		//float moveZ = 0;
		//print (player + " || " + target);
		if (returningToPlayer) {
			if (Vector3.Distance (this.transform.position, target.transform.position) <= 0.0f) {
				//this.transform.parent = playerCharacter.transform;
				returningToPlayer = false;
			}
		}

		if (duration > 0 && target!=player) {
			duration--;
			if (duration <= 0) {
				returnToPlayer ();
			}
		}

		float distCovered = (Time.time - startTime) * cameraSpeed;

		float fractionOfJourney = distCovered / journeyLength;

		this.transform.position = Vector3.Lerp (this.transform.position, target.transform.position, fractionOfJourney);

		if (target == null || player == null) {
			target = player = GameObject.Find ("playerCharacter");
		}

		if (returningToPlayer == false && target == player) {
			this.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, target.transform.position.z);
		}
	}

	public void sendCameraTo(GameObject target, int duration) {
		//this.transform.parent = null;
		this.target = target;
		startTime = Time.time;
		journeyLength = Vector3.Distance (this.transform.position, target.transform.position);
		this.duration = duration;
		player.GetComponent<PlayerMovement> ().lockCharacter (duration);
	}

	public void returnToPlayer() {
		returningToPlayer = true;
		target = player;
		startTime = Time.time;
		journeyLength = Vector3.Distance (this.transform.position, target.transform.position);
	}

	public void preserve() {
		DontDestroyOnLoad (this.gameObject);
	}
}
