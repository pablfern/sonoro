using UnityEngine;
using System.Collections;

public class Bolt : MonoBehaviour {

	public int velocity = 7;
	public float ttl = 1.0f;
	private float creationTime;
	public float width;
	public float height;

	void Start () {
		width = GetComponent<Renderer>().bounds.size.x;
		height = GetComponent<Renderer>().bounds.size.y;
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
		checkBoundaries();
	}

	public void setCreationTime () {
		this.creationTime = Time.time;
	}

	public void returnBolt() {
		GameController.instance.returnBolt (gameObject);
	}

	void checkBoundaries() {

		Vector3 pos = transform.position;
		// es 6 en total, va de -3 a 3
		float verticalSeen    = Camera.main.orthographicSize * 2.0f;
		// es 8 en total, va desde -4 a 4
		float horizontalSeen = verticalSeen * Screen.width / Screen.height;

		float maxX = horizontalSeen / 2;
		float minX = maxX * -1;
		float maxY = verticalSeen / 2;
		float minY = maxX * -1;

		if (pos.x < minX - width) {
			transform.position = new Vector3(maxX, pos.y, pos.z);
		}
		if (pos.x > maxX + width) {
			transform.position = new Vector3(minX, pos.y, pos.z);
		}
		if (pos.y < minY - height) {
			transform.position = new Vector3(pos.x, maxY, pos.z);
		}
		if (pos.y > maxY + height) {
			transform.position = new Vector3(pos.x, minY, pos.z);
		}
	}
}
