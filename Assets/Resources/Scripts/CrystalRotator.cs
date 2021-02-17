using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalRotator : MonoBehaviour {

	public float speed;
	public float[] axis;
	public int[] updown, MAX; 

	// Use this for initialization
	void Start () {
		speed = 1.0f;
		axis = new float[4] { .5f, .5f, .5f, .5f };
		updown = new int[4] {1,1,1,1};
		MAX = new int[4];
		MAX [0] = 50;
	}
	
	// Update is called once per frame
	void Update () {


		for (int i = 1; i < 4; i++) {
			axis [i] += updown [i] * Random.Range (0, 2);
			if (Mathf.Abs(axis [i]) >= Mathf.Abs(MAX [i])) {
				MAX [i] = Random.Range (1, 150);
				updown [i] = -updown [i];
			}
		}

		axis [0] += updown [0];
		if(Mathf.Abs(axis[0]) >= Mathf.Abs(MAX[0])) {
			MAX [0] = -MAX [0];
			updown [0] = -updown [0];
		}

		//print (axis [1] + " " + axis [2] + " " + axis [3]);
		this.gameObject.transform.Rotate (new Vector3 (axis[1]*updown[1], axis[2]*updown[2], axis[3]*updown[3]) * speed * Time.deltaTime);

		if (this.axis[0]>0)
			this.gameObject.transform.Translate(new Vector3 (0, .2f, 0) * Time.deltaTime, Space.World);
		else 
			this.gameObject.transform.Translate(new Vector3 (0, -.2f, 0) * Time.deltaTime, Space.World);
		
	}
}
