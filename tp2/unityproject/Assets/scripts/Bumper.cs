using UnityEngine;
using System.Collections;

public class Bumper : MonoBehaviour {

	private int bumperForce = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision c) {
		GameObject go = c.gameObject;
		if (go.CompareTag ("Player")) {
			Debug.Log ("Collision with bumper");
			GameController.instance.addScore (100);
			Rigidbody rb = go.GetComponent<Rigidbody> ();
			rb.AddForce(-c.contacts[0].normal * bumperForce * rb.velocity.magnitude, ForceMode.Impulse);
		}
	}
}
