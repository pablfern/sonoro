using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	
	public float width;
	public float height;

	// Use this for initialization
	void Start () {
		setPosition();
	}
	
	// Update is called once per frame
	void Update () {
		checkBoundaries();
	}

	void setPosition() {
		float screenWidth = Screen.width;
		float screenHeight = Screen.height;
		transform.position = new Vector2( Random.Range(-screenWidth, screenWidth), Random.Range(-screenHeight, screenHeight));
	}

	void checkBoundaries()
	{

		Vector3 pos = transform.position;
		// es 6 en total, va de -3 a 3
		float verticalSeen = Camera.main.orthographicSize * 2.0f;
		// es 8 en total, va desde -4 a 4
		float horizontalSeen = verticalSeen * Screen.width / Screen.height;

		float maxX = horizontalSeen / 2;
		float minX = maxX * -1;
		float maxY = verticalSeen / 2;
		float minY = maxX * -1;

		if (pos.x < minX - width)
		{
			GameController.instance.returnStar(gameObject);
		}
		if (pos.x > maxX + width)
		{
			GameController.instance.returnStar(gameObject);
		}
		if (pos.y < minY - height)
		{
			GameController.instance.returnStar(gameObject);
		}
		if (pos.y > maxY + height)
		{
			GameController.instance.returnStar(gameObject);
		}
	}
}
