using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public int initialThrust;
	private bool isPlaying = false;
//	private Vector3 initialPosition = new Vector3 (3.5f, 1.0f, -32.5f);
	private Vector3 initialPosition = new Vector3 (22.5f, 1.0f, -38.5f);

	private float launchPower = 0.0f;
	private float maxLaunchPower = 2.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// TODO: quitar esto cuando finalice el desarrollo
		if (Input.GetKey(KeyCode.Return)){
			resetPosition ();
		}
		if (!isPlaying) {
			if (Input.GetKey(KeyCode.Space)) {
				launchPower += Time.deltaTime;
			}
			if (Input.GetKeyUp(KeyCode.Space)){
				launch ();
			}
		}
	}

	public void resetPosition () {
		isPlaying = false;
		gameObject.transform.position = initialPosition;
	}

	void launch () {
		// TODO: implementar velocidad de lanzamiento variable de acuerdo al tiempo de key down
		isPlaying = true;
		Rigidbody rb = gameObject.GetComponent<Rigidbody> ();
		if (launchPower > maxLaunchPower) {
			launchPower = maxLaunchPower;
		}
		rb.AddForce(new Vector3 (0, 0, 1) * initialThrust * launchPower, ForceMode.Impulse);
		launchPower = 0.0f;
	}
}
