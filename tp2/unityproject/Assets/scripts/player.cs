using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int initialThrust;
	private bool isPlaying = false;
	private Vector3 initialPosition = new Vector3 (22.5f, 1.0f, -38.5f);

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

	public void resetPosition () {
		isPlaying = false;
		gameObject.transform.position = initialPosition;
	}

	void launch () {
		isPlaying = true;
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		rb.AddForce(new Vector3 (0, 0, 1) * initialThrust, ForceMode.Impulse);
	}
}
