using UnityEngine;
using System.Collections;

public class DownWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision c) {
		GameObject go = c.gameObject;
		if (go.CompareTag ("Player")) {
			GameController.instance.resetPlayer ();
		}
	}
}
