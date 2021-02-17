using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightChecker : MonoBehaviour {

	GameObject top, centre;
	float climbableTop;
	// Use this for initialization
	void Start () {

		centre = GameObject.Find ("Sphere");
		top = GameObject.Find ("Sphere (2)");
	}
	
	// Update is called once per frame
	void Update () {
		
		climbableTop = this.gameObject.GetComponent<Collider>().bounds.center.y + (this.gameObject.GetComponent<Collider>().bounds.size.y/2);

		centre.transform.SetPositionAndRotation (this.gameObject.GetComponent<Collider> ().bounds.center, this.gameObject.transform.rotation);
		top.transform.SetPositionAndRotation (new Vector3 (this.gameObject.GetComponent<Collider> ().bounds.center.x, climbableTop, this.gameObject.GetComponent<Collider> ().bounds.center.z), this.gameObject.transform.rotation);

		//top.transform.SetPositionAndRotation (new Vector3 (gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z), top.transform.rotation);
		//top.transform.SetPositionAndRotation (new Vector3 (top.transform.position.x, climbableTop, this.gameObject.transform.position.z), top.transform.rotation);
	}
}
