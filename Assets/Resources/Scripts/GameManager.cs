using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject player;
	public GameObject cameraTarget;

	void Awake() {

		DontDestroyOnLoad (this.gameObject);
		player = GameObject.Find ("playerCharacter").gameObject;
		player.GetComponent<PlayerManager>().preserve ();
		cameraTarget = GameObject.Find ("CameraTarget").gameObject;
		cameraTarget.GetComponent<CameraControl>().preserve ();

		transform.Find ("LevelManager").gameObject.GetComponent<LevelManager> ().generateLevel ("Tutorial");
	}

}
