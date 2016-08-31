using UnityEngine;
using System.Collections;

public class Bolt : MonoBehaviour {

	public int velocity;
	public float ttl;
	private float creationTime;

	void Start () {

	}

	void Update () {
		float angle = transform.rotation.eulerAngles.z + 90;
		float x = Mathf.Cos(angle * Mathf.Deg2Rad);
		float y = Mathf.Sin(angle * Mathf.Deg2Rad);
		transform.position = new Vector2(transform.position.x + (x * Time.deltaTime * velocity), transform.position.y + (y * Time.deltaTime * velocity));
		float now = Time.time;
		if (now > creationTime + ttl) {
			returnBolt ();
		}
	}

	public void setCreationTime () {
		this.creationTime = Time.time;
	}

	public void returnBolt() {
		GameController.instance.returnBolt (gameObject);
	}
}
