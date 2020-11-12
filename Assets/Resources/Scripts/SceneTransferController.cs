using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This class handles the fade-out-fade-in scene transfer
public class SceneTransferController : MonoBehaviour {

	public float transitionTimer;
	public float transitionTimerMax;
	public GameObject fader;
	public string levelTo;

	// Use this for initialization
	void Start () {
		transitionTimer = -1.0f;
		transitionTimerMax = 3.0f;
		fader = GameObject.Find("CameraTarget").transform.Find("Fader").gameObject;
	}

	void Update() {
		if (transitionTimer > 0) {
			transitionTimer -= Time.deltaTime;
		}

		if (transitionTimer <= 0 && transitionTimer > -1.0f) {
			saveScene ();
			loadScene (levelTo);
			transitionTimer = -1.0f;
		}
	}

	public void loadScene(string level) {
		SceneManager.LoadSceneAsync (level);
	}

	void OnCollisionEnter(Collision col) {
		//Start timer to give time to fadeout before transitioning to new level
		transitionTimer = transitionTimerMax;	

		//Start camera fadeout
		fader.GetComponent<Fader>().faded = true;
	}

	public void saveScene() {

		GameObject levelData = GameObject.Find ("GameManager");


		if (levelData != null) {
			levelData.GetComponent<LevelData> ().saveData ();
		} else {
			print ("No level data found");
		}
	}
}
