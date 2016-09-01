using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	
	public float width;
	public float height;
	public float rotationSpeed;

	private float xForce;
	private float yForce;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), true);
		setPosition();
//		Debug.Log ("en start");
		setInitialMovement();
	}
	
	// Update is called once per frame
	void Update () {
		checkBoundaries();
	}

	void setPosition() {
		float screenWidth = Screen.width;
		float screenHeight = Screen.height;
//		Debug.Log ("en set position");
		//transform.position = new Vector2( Random.Range(-screenWidth, screenWidth), Random.Range(-screenHeight, screenHeight));
		transform.position = new Vector3(Random.Range(-6.0f, 6.0f), Random.Range(5.0f, 6.0f), 0);
	}

	void setInitialMovement() {
		rb.AddTorque(0.5f, ForceMode2D.Impulse);
		//xForce = Random.Range(-4, 4) * (Time.deltaTime + rotationSpeed);
		yForce = Random.Range(-4, 0) * (Time.deltaTime + rotationSpeed);
		rb.AddForce(new Vector2(0, yForce));
	}

	void checkBoundaries()
	{

		Vector3 pos = transform.position;
		//Debug.Log (string.Format ("pos x y : {0}, {1}", pos.x, pos.y));
		// es 6 en total, va de -3 a 3
		float verticalSeen = Camera.main.orthographicSize * 2.0f;
		// es 8 en total, va desde -4 a 4
		float horizontalSeen = verticalSeen * Screen.width / Screen.height;

		float maxX = horizontalSeen / 2;
		float minX = maxX * -1;
		float maxY = verticalSeen / 2;
		float minY = maxX * -1;

	
		//if (pos.y < minY - height)
		//if (pos.y < -5.0f)
		//{
		//	Debug.Log (string.Format ("position y min: {0}", pos.y));
		//	GameController.instance.returnStar(gameObject);
		//}
	}

	void OnBecameInvisible () {
//		Debug.Log ("en invisible");
		Vector3 pos = transform.position;
//		Debug.Log (string.Format ("pos x y : {0}, {1}", pos.x, pos.y));
		setPosition ();
		GameController.instance.returnStar(gameObject);
	}
}
