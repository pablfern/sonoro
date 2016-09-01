using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public static GameController instance;

    public int lives;
    public int score;
    public int level;

    public GameObject asteroidPrefab;
	public GameObject starPrefab;
	public GameObject boltPrefab;
    public GameObject spaceShip;
    public GameObject startText;
    public GameObject scoreText;
    public GameObject livesText;
    public GameObject gameOverText;

    public int asteroidPoolSize;
	public int starPoolSize;
	public int boltPoolSize;
    
    private float nextActionTime;
	private float nextStarActionTime;
    public float period;
	public float starPeriod;
    private bool gameStarted = false;
    private LinkedList<GameObject> inactiveAsteroidList;
    private LinkedList<GameObject> activeAsteroidList;
	private LinkedList<GameObject> inactiveStarList;
	private LinkedList<GameObject> activeStarList;

	private LinkedList<GameObject> boltList;

    // pool de tiros
    // pool de Asteroides


    // Use this for initialization
    void Start () {
        this.lives = 3;
        this.score = 0;
        this.level = 1;
        this.nextActionTime = 0.0f;
		this.nextStarActionTime = 0.0f;
        this.period = 3.0f;
		this.starPeriod = 1.0f;
        createAsteroidPool();
		createStarPool();
		createBoltPool();
	}

	private void createBoltPool() {
		boltList = new LinkedList<GameObject> ();
		for (int i = 0 ; i < boltPoolSize ; i++) {
			GameObject obj = (GameObject)Instantiate(boltPrefab);
			obj.SetActive(false);
			boltList.AddLast(obj);
		}
	}

    public void createAsteroidPool() {
        inactiveAsteroidList = new LinkedList<GameObject>();
        activeAsteroidList = new LinkedList<GameObject>();
        for (int i = 0; i < this.asteroidPoolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(asteroidPrefab);
            obj.SetActive(false);
            inactiveAsteroidList.AddLast(obj);
        }
    }
		
	public void createStarPool() {
		inactiveStarList = new LinkedList<GameObject>();
		activeStarList = new LinkedList<GameObject>();
		for (int i = 0; i < this.starPoolSize; i++) 
		{
			GameObject obj = (GameObject)Instantiate(starPrefab);
			obj.SetActive (false);
			inactiveStarList.AddLast(obj);
		}
	}

	public GameObject getBolt() {
		if (boltList.Count > 0) {
			GameObject obj = boltList.First.Value;
			obj.SetActive (true);
			boltList.RemoveFirst ();
			return obj;
		}
		return null;
	}

	public void returnBolt(GameObject bolt) {
		bolt.SetActive (false);
		boltList.AddLast (bolt);
	}

    public GameObject getAsteroid() {
        if (inactiveAsteroidList.Count > 0 ) {
            GameObject obj = inactiveAsteroidList.First.Value;
            inactiveAsteroidList.RemoveFirst();
            activeAsteroidList.AddLast(obj);
			obj.GetComponent<Asteroid> ().resetAsteroid();
            return obj;
        }
        return null;
    }

	public GameObject getStar() {
		if (inactiveStarList.Count > 0) {
			GameObject obj = inactiveStarList.First.Value;
			inactiveStarList.RemoveFirst();
			activeStarList.AddLast(obj);
			return obj;
		}
		return null;
	}

    public void returnAsteroid(GameObject asteroid) {
        asteroid.SetActive(false);
        activeAsteroidList.Remove(asteroid);
        inactiveAsteroidList.AddLast(asteroid);
    }

	public void returnStar(GameObject star) {
		star.SetActive(false);
		activeStarList.Remove(star);
		inactiveStarList.AddLast(star);
		Debug.Log ("en return");
		Debug.Log(string.Format("inactive count: {0}",inactiveStarList.Count));
		Debug.Log(string.Format("active count: {0}",activeStarList.Count));
	}

    void checkInput() {
        if (Input.GetKey(KeyCode.Return)) {
            Debug.Log("Start!");
            startGame();
        }
    }

    void startGame() {
        gameStarted = true;
        startText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
        spaceShip.gameObject.SetActive(true);
    }

    void refreshScoreAndLives() {
        scoreText.GetComponent<UnityEngine.UI.Text>().text = "Score: " + score.ToString();
        livesText.GetComponent<UnityEngine.UI.Text>().text = "Lives: " + lives.ToString();
    }

    void spawnAsteroid() {
        if (Time.time > nextActionTime) {
            nextActionTime += period;
            GameObject obj = getAsteroid();
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }

	void spawnStars() {
		if (Time.time > nextStarActionTime) {
			Debug.Log("en spawn if");
			nextStarActionTime += starPeriod;
			Debug.Log(string.Format("inactive count: {0}",inactiveStarList.Count));
			GameObject obj = getStar();
			if (obj != null) {
				obj.SetActive (true);
				Debug.Log("activating");
			}
		}
	}

    void removeAsteroids() {
        foreach(GameObject obj in activeAsteroidList) {
            obj.SetActive(false);
            inactiveAsteroidList.AddLast(obj);
        }
        activeAsteroidList = new LinkedList<GameObject>();
    }

   public void playerKilled() {
        lives -= 1;
        if(lives < 0) {
            gameOver();
        } else {
            spaceShip.gameObject.GetComponent<SpaceShip>().restartPosition();
            spaceShip.gameObject.SetActive(true);
            removeAsteroids();
        }
    }

    void gameOver() {
        spaceShip.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
        if (!gameStarted) { 
            checkInput();
        } else {
            refreshScoreAndLives();
            spawnAsteroid();
			spawnStars();
        }
    }

	void Awake () {
		if (instance == null) {
			instance = this;
		}

		else if (instance != this) {
			Destroy(gameObject); 
		}
	}
}
