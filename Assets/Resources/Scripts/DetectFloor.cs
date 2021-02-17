using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectFloor : MonoBehaviour {	


	public GameObject player;
	private float x, z; //bounds size, to measure offset for ray locations
	private int distance;

	void Start() {

		player = GameObject.Find ("playerCharacter");
		x = GetComponent<BoxCollider> ().size.x / 2;
		z = GetComponent<BoxCollider> ().size.z / 2;
		distance = 1;
	}

	void Update() {

		if (!detectFloor()) {
			release ();
			//print ("Release");
		}
		detectRoof ();

		//drawDetectionRayDown (transform.position, distance);
		//drawDetectionRayDown (transform.position, distance);

	}

	public GameObject drawDetectionRay(Vector3 position, Vector3 direction, int distance) {

		RaycastHit hit;
		if (Physics.Raycast (position, transform.TransformDirection (direction), out hit, distance)) {
			//print (hit.collider.gameObject.name);
			Debug.DrawRay(position, direction, Color.green, distance);
			return hit.collider.gameObject;
		} else {
			Debug.DrawRay(position, direction, Color.red, distance);
			return null;
		}
	}

	public bool detectFloor() {

		float tx = this.transform.position.x;
		float ty = this.transform.position.y;
		float tz = this.transform.position.z;

		GameObject check = drawDetectionRay (new Vector3(tx, ty, tz), Vector3.down, distance);
		if (check == null)
			check = drawDetectionRay (new Vector3(tx, ty, tz+z), Vector3.down, distance);
		if (check == null)
			check = drawDetectionRay (new Vector3(tx, ty, tz-z), Vector3.down, distance);
		if (check == null)
			check = drawDetectionRay (new Vector3(tx+x, ty, tz), Vector3.down, distance);
		if (check == null)
			check = drawDetectionRay (new Vector3(tx-x, ty, tz), Vector3.down, distance);

		if (check == null) {
			return false;
		} else {
			return true;
		}
	}

	public bool detectRoof() {

		float tx = this.transform.position.x;
		float ty = this.transform.position.y + this.gameObject.GetComponent<BoxCollider>().bounds.size.y;
		float tz = this.transform.position.z;

		/*GameObject location = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Destroy (location.GetComponent<Collider> ());
		location.transform.position = new Vector3 (tx, ty, tz);
		location.AddComponent<SelfDestructTimer> ();*/

		List<GameObject> list = new List<GameObject> ();

		GameObject check = drawDetectionRay (new Vector3(tx, ty, tz), Vector3.up, distance);
		if (check != null)
			list.Add (check);
		check = drawDetectionRay (new Vector3(tx, ty, tz+z), Vector3.up, distance);
		if (check != null)
			list.Add (check);
		check = drawDetectionRay (new Vector3(tx, ty, tz-z), Vector3.up, distance);
		if (check != null)
			list.Add (check);
		check = drawDetectionRay (new Vector3(tx+x, ty, tz), Vector3.up, distance);
		if (check != null)
			list.Add (check);
		check = drawDetectionRay (new Vector3(tx-x, ty, tz), Vector3.up, distance);
		if (check != null)
			list.Add (check);

		foreach (GameObject o in list) {
			//o.transform.Translate (this.gameObject.GetComponent<Rigidbody> ().velocity);
			//print (o.transform.parent);
			if (o.name != "playerCharacter" && o.transform.parent == null) {
				o.transform.parent = this.transform;

				//print (o.transform.parent);
				//if (o.GetComponent<GravityDeParenter> () == null) {
				//	o.AddComponent<GravityDeParenter> ();
				//} else {
					//
				//}
			}
		}

		if (check == null) {
			return false;
		} else {
			return true;
		}
	}

	public void release() {

		if (player != null) {
			player.GetComponent<PlayerMovement> ().freeMovement ();
			player.GetComponent<PlayerMovement> ().facingLock = "None";
			this.transform.SetParent (null);
		}

		if (this.transform.gameObject.GetComponent<Rigidbody> () == null) {
			Rigidbody rbody = this.gameObject.AddComponent<Rigidbody> ();

			rbody.freezeRotation = true;
			rbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;

			rbody.mass = 20;
		}

		//print (this.gameObject.name);
		SceneManager.MoveGameObjectToScene (this.gameObject, SceneManager.GetActiveScene ());

		transform.Find("AreaTriggerX").gameObject.GetComponent<Moveable>().gripped = false;
		transform.Find("AreaTriggerZ").gameObject.GetComponent<Moveable>().gripped = false;
	}
}
