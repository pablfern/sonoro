using UnityEngine;
using System.Collections;

public class flipperRotatorScript : MonoBehaviour {

	public float maxAngle;
	public float flipTime;
	public string key;

	private Quaternion initialOrientation;
	private Quaternion finalOrientation;
	private UnityEngine.KeyCode keyCode;
	private float t = 0.0f;
	private float prev_t = 0.0f;

	void Start () {
		initialOrientation = transform.rotation;
		if(key=="left") {
			this.keyCode = KeyCode.LeftArrow;
			maxAngle = -90.0f;
			finalOrientation.eulerAngles = new Vector3(initialOrientation.eulerAngles.x, initialOrientation.eulerAngles.y + maxAngle, initialOrientation.eulerAngles.z);
		} else {
			this.keyCode = KeyCode.RightArrow;
			maxAngle = 90.0f;
			finalOrientation.eulerAngles = new Vector3(initialOrientation.eulerAngles.x, initialOrientation.eulerAngles.y + maxAngle, initialOrientation.eulerAngles.z);
		}
	}

    void Update() {
        if (GameController.instance.inGame()) {
            if (Input.GetKeyDown(this.keyCode)) {
                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();
            }

			prev_t = t;

            if (Input.GetKey(this.keyCode)) {
                transform.rotation = Quaternion.Slerp(initialOrientation, finalOrientation, t / flipTime);
				t += Time.deltaTime;
                if (t > flipTime) {
                    t = flipTime;
                }
            } else {
                transform.rotation = Quaternion.Slerp(initialOrientation, finalOrientation, t / flipTime);
                t -= Time.deltaTime;
                if (t < 0) {
                    t = 0;
                }
            }
				
//			Debug.Log ("t = " + t + " prev_t = " + prev_t);
        }
    }

	void OnCollisionEnter (Collision c) {
		GameObject go = c.gameObject;
		if (go.CompareTag ("Player")) {
			Rigidbody rb = go.GetComponent<Rigidbody> ();
			float bumperForce = (t - prev_t) * 1000;
//			Debug.Log ("Collision with flipper force = " + bumperForce);
			if (bumperForce > 0) {
//				rb.AddForce(-c.contacts[0].normal * bumperForce * rb.velocity.magnitude, ForceMode.Impulse);
				rb.AddForce(-c.contacts[0].normal * bumperForce, ForceMode.Impulse);
			}
		}
	}
}
