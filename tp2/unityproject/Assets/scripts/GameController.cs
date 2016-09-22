using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController instance;
	public static int score = 0;
	public GameObject player;
	public TextMesh scoreText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (scoreText) {
			scoreText.text = score.ToString ("D8");
		}
	}

	public void resetPlayer () {
		player.gameObject.GetComponent<Player>().resetPosition();
	}

	void Awake () {
		if (instance == null) {
			instance = this;
		}

		else if (instance != this) {
			Destroy(gameObject); 
		}
	}

	public static void addScore(int value) {
		score += value;
	}
		
}
