using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStartUpController : MonoBehaviour {

	GameObject player;
	GameObject playerSpawnPoint;
	GameObject levelData;
	// Use this for initialization
	void Start () {

		//Locate c
		levelData = GameObject.Find ("GameManager/LevelManager/" + SceneManager.GetActiveScene ().name + " Level Data");

		//Unfade the black screen
		print("Starting " + SceneManager.GetActiveScene().name);
		GameObject.Find ("CameraTarget").transform.Find ("Fader").gameObject.GetComponent<Fader> ().toggleFadeAfter (0.0f, false);

		//Load all objects in Level Data Script
		//levelData.GetComponent<LevelData>().loadData();

		//Move Spawn point to last saved spawn and place player at spawn point location
		player = GameObject.Find("playerCharacter");
		playerSpawnPoint = GameObject.Find ("PlayerSpawnPoint");
		playerSpawnPoint.transform.SetPositionAndRotation (levelData.GetComponent<LevelData> ().playerSpawnLocation);
		player.transform.SetPositionAndRotation(playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
	}
}
