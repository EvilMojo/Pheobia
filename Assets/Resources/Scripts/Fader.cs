using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour {

	public bool faded, respawnOk;
	public float transparencyControl;
	public const float MAX_FADE = 1.0f;
	public const float fadeRate = 0.01f;
	public float timer;
	public GameObject targetPlayer;

	// Use this for initialization
	void Start () {
		faded = false;
		respawnOk = true;
		this.gameObject.GetComponent<MeshRenderer> ().material.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		transparencyControl = 1.0f;
		this.gameObject.GetComponent<MeshRenderer> ().material.shader = Shader.Find ("Sprites/Diffuse");
		targetPlayer = GameObject.Find ("playerCharacter");
	}
	
	// Update is called once per frame
	void Update () {

        this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0.0f, 0.0f, 0.0f, transparencyControl);

        if (timer != -1) {
			if (timer <= 0 && timer > -1) {
				if (!faded && transparencyControl > 0) {
					transparencyControl -= fadeRate;
				} else if (faded && transparencyControl < MAX_FADE) {
					transparencyControl += fadeRate;
				}
			} else if (timer > 0) {
				timer -= Time.deltaTime;
			}
		}

		if (transparencyControl >= 1.0f) {
			targetPlayer.GetComponent<PlayerManager> ().revive ();
		}
        
		if (respawnOk) {
			faded = false;
			transparencyControl = 0.0f;
			respawnOk = false;
			targetPlayer = GameObject.Find ("playerCharacter");
		}
	}

	public void toggleFadeAfter(float timer, bool faded) {
		this.timer = timer;
		this.faded = faded;
		respawnOk = false;
	}
}
