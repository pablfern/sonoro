using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	
	public float width;
	public float height;
	public float rotationSpeed;

	private float xForce;
	private float yForce;

	void Start () {
		Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), true);
		resetStar ();
	}
	
	void Update () {
		checkBoundaries();
	}

	public void resetStar() {
		setPosition();
		setInitialMovement();
	}

	void setPosition() {
		float screenWidth = Screen.width;
		float screenHeight = Screen.height;
		transform.position = new Vector3(Random.Range(-6.0f, 6.0f), Random.Range(5.0f, 6.0f), 0);
	}

	void setInitialMovement() {
		GetComponent<Rigidbody2D>().AddTorque(0.5f, ForceMode2D.Impulse);
		yForce = Random.Range(-4, 0) * (Time.deltaTime + rotationSpeed);
		GetComponent<Rigidbody2D>().AddForce(new Vector2(0, yForce));
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

    void OnBecameInvisible () {
		Vector3 pos = transform.position;
		setPosition ();
		GameController.instance.returnStar(gameObject);
	}
}
