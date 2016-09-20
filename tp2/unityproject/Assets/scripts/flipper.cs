using UnityEngine;
using System.Collections;

public class flipper : MonoBehaviour {

	public int bumperForce;
//	private UnityEngine.KeyCode keyCode;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

//	void OnCollisionEnter (Collision c) {
//		// bumper effect to speed up ball
//		GameObject go = c.gameObject;
//		if (go.CompareTag ("Player") && Input.GetKey(this.keyCode)) {
//			Rigidbody rb = go.GetComponent<Rigidbody> ();
//			Debug.Log ("Normal = " + c.contacts[0].normal + " | Velocity = " + rb.velocity.magnitude);
//			rb.AddForce(-c.contacts[0].normal * bumperForce * rb.velocity.magnitude, ForceMode.Impulse);
//		}
////		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
////		rb.AddForce(c.contacts[0].normal * bumperForce * rb.velocity.magnitude, ForceMode.Impulse);
//	}
}
