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
    public GameObject levelManager;

	// Use this for initialization
	void Start () {
		transitionTimer = -1.0f;
		transitionTimerMax = 3.0f;
        fader = GameObject.Find("CameraTarget").transform.Find("Fader").gameObject;
        levelManager = GameObject.Find("LevelManager");

    }

	void Update() {
		if (transitionTimer > 0) {
			transitionTimer -= Time.deltaTime;
		}

		if (transitionTimer <= 0 && transitionTimer > -1.0f) {
			saveScene ();
			loadScene (levelTo);
			transitionTimer = -1.0f;
            //fader.GetComponent<Fader>().toggleFadeAfter(0.0f, false);
        }
	}

	public void loadScene(string level) {
		SceneManager.LoadSceneAsync (level);
	}

	void OnCollisionEnter(Collision col) {
        
		//Start timer to give time to fadeout before transitioning to new level
		transitionTimer = transitionTimerMax;

        //Start camera fadeout
        fader.GetComponent<Fader>().toggleFadeAfter(0.0f, true);
	}

	public void saveScene() {

        GameObject levelData;
        
        levelData = GameObject.Find("GameManager/LevelManager/" + levelTo + " Level Data");

        //Need GameManager/LevelManager/<num> Level Data
		print (SceneManager.GetActiveScene().name);

		if (levelData == null) {
            /*levelData = new GameObject();
            levelData.name = levelTo + " Level Data";
            levelData.AddComponent<LevelData>();
            levelData.transform.parent = GameObject.Find("GameManager/LevelManager/").transform;
            levelData.GetComponent<LevelData>().createData();
            */
            levelManager.GetComponent<LevelManager>().createLevel(levelTo);
        }
        levelData.GetComponent<LevelData>().saveData();
    }

	public void capturePlayerPosition() {
		GameObject.Find ("GameManager/LevelManager/" + SceneManager.GetActiveScene ().name).GetComponent<LevelData>().playerSpawnLocation = GameObject.Find("playerCharacter").transform.position;
	}
}
