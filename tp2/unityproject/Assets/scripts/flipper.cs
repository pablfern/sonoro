using UnityEngine;
using System.Collections;

public class flipper : MonoBehaviour {
	public float maxAngle;
	public float flipTime;
	public string key;
	
	private Quaternion initialOrientation;
	private Quaternion finalOrientation;
	private UnityEngine.KeyCode keyCode;
	private float t;

	// Use this for initialization
	void Start ()
	{
		initialOrientation = transform.rotation;
		flipTime = 0.20f;
		if(key=="left"){
			this.keyCode = KeyCode.LeftArrow;
			maxAngle = -90.0f;
			finalOrientation.eulerAngles = new Vector3(initialOrientation.eulerAngles.x, initialOrientation.eulerAngles.y + maxAngle, initialOrientation.eulerAngles.z);
		} else {
			this.keyCode = KeyCode.RightArrow;
			maxAngle = 90.0f;
			finalOrientation.eulerAngles = new Vector3(initialOrientation.eulerAngles.x, initialOrientation.eulerAngles.y + maxAngle, initialOrientation.eulerAngles.z);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(this.keyCode)){
			transform.rotation = Quaternion.Slerp(initialOrientation, finalOrientation, t/flipTime);
			t += Time.deltaTime;
			if(t > flipTime){
				t = flipTime;
			}
		}
		else
		{
			transform.rotation = Quaternion.Slerp(initialOrientation, finalOrientation, t/flipTime);
			t -= Time.deltaTime;
			if(t < 0)
			{
				t = 0;
			}
		}
	}
}
