using UnityEngine;
using System.Collections;
using System;

public class SpaceShip : MonoBehaviour {

    //public SpriteRenderer fireSprite;
    public float rotationSpeed = 2.0f;
	public float width;
	public float height;
    public GameObject gameController;
	private Camera cam;

    private Rigidbody2D rb;
	public Rigidbody2D bolt;
	public float boltSpeed = 10f;

	void Start () {
        //fireSprite.enabled = false;
        this.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
		width = GetComponent<Renderer>().bounds.size.x;
		height = GetComponent<Renderer>().bounds.size.y;
		cam = GetComponent<Camera>();
	}
	
	void Update () {
        checkInput();
		checkBoundaries();
	}

    void checkInput() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, transform.rotation.eulerAngles.z + Time.deltaTime + rotationSpeed));
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, transform.rotation.eulerAngles.z - (Time.deltaTime+ rotationSpeed)));
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            float angle = transform.rotation.eulerAngles.z + 90;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);
            rb.AddForce(new Vector2(x * (Time.deltaTime + rotationSpeed), y * (Time.deltaTime + rotationSpeed)));
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            float angle = transform.rotation.eulerAngles.z - 90;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);
            rb.AddForce(new Vector2(x * (Time.deltaTime + rotationSpeed), y * (Time.deltaTime + rotationSpeed)));
        }
		if (Input.GetKeyDown(KeyCode.Space)) {
			GameObject bolt = GameController.instance.getBolt ();
			bolt.transform.position = transform.position;
			bolt.transform.rotation = transform.rotation;
			bolt.GetComponent<Bolt> ().setCreationTime ();
		}
    }


    public void restartPosition() {
        // TODO: Reset rotation
        rb.velocity = new Vector3(0, 0, 0);
        transform.position = new Vector3(0, 0, 0);
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

//    void OnCollisionEnter2D(Collision2D collision) {
//        Debug.Log("SpaceShipCollide");
//        gameController.gameObject.GetComponent<GameController>().playerKilled();
//       
//
//    }

	void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log("SpaceShipCollide");
		gameController.gameObject.GetComponent<GameController>().playerKilled();


	}
}
