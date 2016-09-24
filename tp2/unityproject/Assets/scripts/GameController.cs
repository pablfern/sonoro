using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController instance;
	public GameObject player;
	public TextMesh scoreText;
    public TextMesh ballsText;
    public TextMesh generalText;
    private bool isPlaying = false;
    private int balls = 0;
    private int score = 0;

    // Use this for initialization
    void Start () {
        player.SetActive(false);
        generalText.text = "PRESS SPACE\nTO BEGIN";
	}
	
	// Update is called once per frame
	void Update () {
        if (isPlaying) {
            scoreText.text = "SCORE\n" + score.ToString("D8");
            ballsText.text = "BALLS\n" + balls.ToString();
        } else {
            if (Input.GetKeyDown(KeyCode.Space)) {
                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();
                this.startGame();
            }
        }
	}

    private void startGame() {
        player.SetActive(true);
        generalText.gameObject.SetActive(false);
        this.isPlaying = true;
        this.score = 0;
        this.balls = 3;
        ballsText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        
        resetPlayer();
    }

    public bool inGame() {
        return this.isPlaying;
    }

    public void takeLive() {
        this.balls -= 1;
        if (this.balls < 0) {
            this.gameOver();
        }
    }

    private void gameOver() {
        player.SetActive(false);
        ballsText.gameObject.SetActive(false);
        generalText.text = "GAME OVER!";
        generalText.gameObject.SetActive(true);
        isPlaying = false;
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
		//score += value;
	}
		
}
