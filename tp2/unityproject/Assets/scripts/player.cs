using UnityEngine;
using System.Collections;

public class player : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space)){
			Debug.Log ("Reseting player position to (3, 1, 0)");
			gameObject.transform.position = new Vector3 (3, 1, 0);
		}
	}
}
