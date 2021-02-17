using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LazyCoder : MonoBehaviour {

	public string level;
	// Use this for initialization
	void Start () {
		if (GameObject.Find("GameManager") == null) {
			DontDestroyOnLoad (this.gameObject);
			print ("Game Manager Not Found");
			SceneManager.LoadSceneAsync ("DataHold");
		}
	}
}
