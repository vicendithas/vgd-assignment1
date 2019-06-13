using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	
	private float orbitspeed;
	public float scaling;
	public GUIText rotText;
	private float angle;
	private float radius;
	private float rotscale;
	private bool exploded;
	public GameObject explosion;

	void Start(){
		exploded = false;
		rotscale = Mathf.PI;
		orbitspeed = Mathf.PI;
		radius = 3f;
		angle = 0f;
		rotText.text = "";
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKey (KeyCode.Alpha9)){
			radius = Mathf.Max (1.5f, radius - (scaling * Time.deltaTime));
		}

		if(Input.GetKey (KeyCode.Alpha0)){
			radius = Mathf.Min (7f, radius + (scaling * Time.deltaTime));
		}

		if(Input.GetKey (KeyCode.Alpha1)){
			orbitspeed = Mathf.Max (0f, orbitspeed - (rotscale * Time.deltaTime));
		}

		if(Input.GetKey (KeyCode.Alpha2)){
			orbitspeed = Mathf.Min (10*Mathf.PI, orbitspeed + (rotscale * Time.deltaTime));
		}

		angle += orbitspeed * Time.deltaTime;
		float xpos = radius * Mathf.Cos (angle);
		float zpos = radius * Mathf.Sin (angle);

		if(!exploded){
			gameObject.transform.localPosition = new Vector3(xpos, 0f, zpos);
		}

		SetRotationText ();

		Vector3 current = gameObject.transform.position;
		//print ("Child Pos - X: " + current.x + "   Y: " + current.y + "   Z: " + current.z + "   Exploded: " + exploded);
		bool crash = (current.x > -10.25) && (current.x < 10.25) && (current.z > -10.25) && (current.z < 10.25) && (current.y < 0.25) && (current.y > -0.25);

		if (crash && !exploded){
			gameObject.GetComponent<Renderer>().enabled = false;
			exploded = true;
            gameObject.GetComponent<AudioSource>().Play();
        }
	}

	void SetRotationText(){
		float RPM = orbitspeed * 60 / (2 * Mathf.PI);

		string msg1 = "Orbit Radius: " + radius.ToString ("#0.00");
		string msg2 = "\nOrbit Speed: " + RPM.ToString ("#0.00") + " RPM";
		rotText.text = msg1 + msg2;
	}
}
