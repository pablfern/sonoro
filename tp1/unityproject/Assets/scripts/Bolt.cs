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

    public void setBoltTTL(float ttl) {
        this.ttl = ttl;
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
        gameObject.SetActive(true);
        Destroy(gameObject);
//		GameController.instance.returnBolt (gameObject);
	}

    void checkBoundaries()
    {

        Vector3 pos = transform.position;
        // es 6 en total, va de -3 a 3
        float verticalSeen = Camera.main.orthographicSize * 2.0f;
        // es 8 en total, va desde -4 a 4
        float horizontalSeen = verticalSeen * Screen.width / Screen.height;

        float maxX = 6.7f;
        float minX = -6.7f;
        float minY = -3.4f;
        float maxY = 3.4f;

        if (pos.x < minX)
        {
            transform.position = new Vector3(maxX, pos.y, pos.z);
        }
        if (pos.x > maxX)
        {
            transform.position = new Vector3(minX, pos.y, pos.z);
        }
        if (pos.y < minY)
        {
            transform.position = new Vector3(pos.x, maxY, pos.z);
        }
        if (pos.y > maxY)
        {
            transform.position = new Vector3(pos.x, minY, pos.z);
        }
    }
}
