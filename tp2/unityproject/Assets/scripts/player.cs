using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

	public int initialThrust;
	private bool isPlaying = false;
	private Vector3 initialPosition = new Vector3 (21, 1, -48);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Return)){
			resetPosition ();
		}
		if (Input.GetKey(KeyCode.Space) && !isPlaying){
			launch ();
		}

	}

	void resetPosition () {
		isPlaying = false;
		gameObject.transform.position = initialPosition;
	}

	void launch () {
		isPlaying = true;
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		rb.AddForce(new Vector3 (0, 0, 1) * initialThrust, ForceMode.Impulse);
	}
}
