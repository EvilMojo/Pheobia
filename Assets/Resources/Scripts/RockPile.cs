using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPile : Activatable {

	int tick, quantity;
	float scalemin, scalemax;

	// Use this for initialization
	void Start () {
		quantity = 0;
		scalemin = scalemax = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public override void activate() {
		this.gameObject.SetActive (true);
		print (this.gameObject + " Activate rocks");
		generateRockPile (0, 0, 0);
	}

	public override void deactivate() {

	}

	public void generateRockPile(int quantity, float scalemin, float scalemax) {

		if (quantity == 0)
			quantity = Random.Range (0, 10);
		if (scalemin == 0) 
			scalemin = Random.Range (.2f, 4.0f);
		if (scalemax == 0) {
			scalemax = Random.Range (scalemin, 8);
		}
		else if (scalemax < scalemin) {
			scalemax = scalemin;
		}

		for (int i = 0; i < quantity; i++) {
			GameObject rock = new GameObject();
			int index = Random.Range(0,5);
			string type = "A";

			switch (index) {
			case 0:
				type = "A";
				break;
			case 1:
				type = "B";
				break;
			case 2:
				type = "C";
				break;
			case 3:
				type = "D";
				break;
			case 4:
				type = "E";
				break;
			case 5:
				type = "F";
				break;
			}


			rock = Instantiate ((Resources.Load ("Prefabs/rock" + type, typeof(GameObject)) as GameObject), this.transform.position, Quaternion.identity);
			float scale = Random.Range (scalemin, scalemax);

			rock.transform.localScale = new Vector3 (scale, scale, scale);


			float buffer = rock.gameObject.GetComponent<MeshFilter> ().mesh.bounds.size.y;
			rock.transform.Translate(new Vector3(0, (i*scale+buffer)*2.5f, 0));
			//rock.transform.position = new Vector3 (Random.Range (-3.0f, 3.0f), Random.Range (-3.0f, 3.0f), Random.Range (-3.0f, 3.0f));

			rock.AddComponent<Rigidbody> ();
			rock.GetComponent<Rigidbody> ().useGravity = true;
			rock.GetComponent<Rigidbody> ().mass = scale * 100;
			rock.GetComponent<Rigidbody> ().drag = .2f*scale;
		}


		Destroy (this.gameObject); //Destroy this here so we don't accidentally make duplicates. We don't destroy this in start as start can execute before this can

	}
}
