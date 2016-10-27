using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public static GameController instance;

    public int lives;
    public int score;
    public int highScore;

    public GameObject largeAsteroidPrefab;
	public GameObject mediumAsteroidPrefab;
    public GameObject asteroidExplosionPrefab;
    public GameObject blueEnemyPrefab;

	public GameObject starPrefab;
	public GameObject boltPrefab;
    public GameObject spaceShip;
    public GameObject startText;
    public GameObject scoreText;
    public GameObject highScoreText;
    public GameObject livesText;
    public GameObject gameOverText;

    public int asteroidPoolSize;
	public int mediumAsteroidPoolSize;
	public int starPoolSize;
	public int boltPoolSize;

	public AudioSource backgroundMusic;
	public AudioSource gameOverMusic;
    
    private float nextActionTime;
	private float nextStarActionTime;
    public float period;
	public float starPeriod;
    private bool gameStarted = false;
    private int level;

    private List<GameObject> largeAsteroidList;
	private List<GameObject> mediumAsteroidList;
	private List<GameObject> starList;
	private List<GameObject> boltList;
    private List<GameObject> inactiveExplosions;
    private List<GameObject> activeExplosions;
    private List<GameObject> blueEnemyList;

    // Use this for initialization
    void Start () {
        this.nextActionTime = 0.0f;
		this.nextStarActionTime = 0.0f;
        this.period = 3.0f;
		this.starPeriod = 1.0f;
        this.level = 1;
	}

    private void createBlueEnemyPool() {
        blueEnemyList = new List<GameObject>();
        for(int i = 0; i < 1; i++) {
            GameObject obj = (GameObject)Instantiate(blueEnemyPrefab);
            obj.SetActive(false);
            blueEnemyList.Add(obj);
        }
    }

    private void createExplosionPool() {
        inactiveExplosions = new List<GameObject>();
        activeExplosions = new List<GameObject>();

        for (int i = 0; i < mediumAsteroidPoolSize; i++) {
            GameObject obj = (GameObject)Instantiate(asteroidExplosionPrefab);
            obj.SetActive(false);
            inactiveExplosions.Add(obj);
        }
    }

    public void getAsteroidExplosion(Vector3 position) {
        if (inactiveExplosions.Count > 0) {
            GameObject obj = inactiveExplosions[0];
            obj.transform.position = position;
            obj.gameObject.GetComponent<ParticleSystem>().time = 0;
            obj.gameObject.GetComponent<ParticleSystem>().Play();
            obj.SetActive(true);
            inactiveExplosions.RemoveAt(0);
            activeExplosions.Add(obj);
            //return obj;
        } else {
            List<GameObject> aux = inactiveExplosions;
            inactiveExplosions = activeExplosions;
            activeExplosions = inactiveExplosions;
            //return getAsteroidExplosion(position);
        }
    }

    private void createMediumAsteroidPool() {
		mediumAsteroidList = new List<GameObject> ();
		for (int i = 0 ; i < mediumAsteroidPoolSize ; i++) {
			GameObject obj = (GameObject)Instantiate(mediumAsteroidPrefab);
			obj.SetActive(false);
			mediumAsteroidList.Add(obj);
		}
	}

	private void createBoltPool() {
		boltList = new List<GameObject> ();
		for (int i = 0 ; i < boltPoolSize ; i++) {
			GameObject obj = (GameObject)Instantiate(boltPrefab);
			obj.SetActive(false);
			boltList.Add(obj);
		}
	}

    public void createAsteroidPool() {
		largeAsteroidList = new List<GameObject> ();
        for (int i = 0 ; i < this.asteroidPoolSize ; i++) {
			GameObject obj = (GameObject)Instantiate(largeAsteroidPrefab);
			obj.SetActive(false);
			largeAsteroidList.Add(obj);
		}
    }
		
	public void createStarPool() {
		starList = new List<GameObject>();
        for (int i = 0 ; i < this.starPoolSize ; i++) {
			GameObject obj = (GameObject)Instantiate(starPrefab);
			obj.SetActive (false);
			starList.Add(obj);
		}
	}

	public GameObject getMediumAsteroid(float x, float y) {
		if (mediumAsteroidList.Count > 0) {
			GameObject obj = mediumAsteroidList[0];
			obj.SetActive (true);
			mediumAsteroidList.RemoveAt (0);
			obj.GetComponent<Asteroid> ().setPosition(x, y);
			obj.GetComponent<MediumAsteroid> ().resetAsteroid ();
			return obj;
		}
		return null;
	}

	public void returnMediumAsteroid(GameObject mediumAsteroid) {
		mediumAsteroid.SetActive (false);
		mediumAsteroidList.Add (mediumAsteroid);
	}

	public GameObject getBolt() {
		if (boltList.Count > 0) {
			GameObject obj = boltList[0];
			obj.SetActive (true);
			boltList.RemoveAt (0);
			return obj;
		}
		return null;
	}

	public void returnBolt(GameObject bolt) {
		bolt.SetActive (false);
		boltList.Add (bolt);
	}

    public GameObject getAsteroid() {
        if (largeAsteroidList.Count > 0 ) {
			GameObject obj = largeAsteroidList[0];
			largeAsteroidList.RemoveAt(0);
            return obj;
        }
        return null;
    }
		
	public void returnAsteroid(GameObject asteroid) {
		largeAsteroidList.Add(asteroid);
		asteroid.SetActive(false);
	}

	public GameObject getStar() {
		if (starList.Count > 0) {
			GameObject obj = starList[0];
			starList.RemoveAt(0);
			return obj;
		}
		return null;
	}

    public GameObject getBlueEnemy() {
        if(blueEnemyList.Count > 0) {
            GameObject obj = blueEnemyList[0];
            blueEnemyList.RemoveAt(0);
            print(obj);
            return obj;
        }
        return null;
    }

	public void returnStar(GameObject star) {
		star.SetActive(false);
		starList.Add (star);
	}

    void checkInput() {
        if (Input.GetKey(KeyCode.Return)) {
            startGame();
        }
    }

    void startGame() {
        gameStarted = true;
        this.lives = 3;
        this.score = 0;
		this.nextActionTime = Time.time;
		this.nextStarActionTime = Time.time;
        this.period = 3.0f;
        this.starPeriod = 1.0f;
        this.level = 1;
        createAsteroidPool();
        createStarPool();
        createBoltPool();
        createMediumAsteroidPool();
        createExplosionPool();
        backgroundMusic.Play();
        spaceShip.gameObject.GetComponent<SpaceShip>().restartPosition();
        gameOverText.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
        spaceShip.gameObject.SetActive(true);
        if(highScore>0) {
            highScoreText.gameObject.SetActive(true);
        } else {
            highScoreText.gameObject.SetActive(false);
        }
    }

    void refreshScoreAndLives() {
        scoreText.GetComponent<UnityEngine.UI.Text>().text = "Score: " + score.ToString();
        livesText.GetComponent<UnityEngine.UI.Text>().text = "Lives: " + lives.ToString();
        highScoreText.GetComponent<UnityEngine.UI.Text>().text = "High Score: " + highScore.ToString();
    }

    void spawnAsteroid() {
        if (Time.time > nextActionTime) {
            nextActionTime += period;
            GameObject obj = getAsteroid();
            if (obj != null) {
                obj.SetActive(true);
  				obj.GetComponent<LargeAsteroid> ().resetAsteroid();
            }
        }
    }

	void spawnStars() {
		if (Time.time > nextStarActionTime) {
			nextStarActionTime += starPeriod;
			GameObject obj = getStar();
			if (obj != null) {
				obj.SetActive (true);
				obj.GetComponent<Star> ().resetStar ();
			}
		}
	}

    void spawnBlueEnemy() {
        GameObject obj = getBlueEnemy();
        if(obj != null) {
            print(obj);
            obj.SetActive(true);
            obj.GetComponent<BlueEnemy>().resetBlueEnemy();
        }
    }

    void removeAsteroids() {
		Destroy (GameObject.FindWithTag("largeAsteroid"));
		Destroy (GameObject.FindWithTag("mediumAsteroid"));
		createAsteroidPool ();
		createMediumAsteroidPool ();
		this.nextActionTime = Time.time;
		this.nextStarActionTime = Time.time;
    }

   public void playerKilled() {
        lives -= 1;
        if(lives < 0) {
            gameOver();
        } else {
            spaceShip.gameObject.GetComponent<SpaceShip>().restartPosition();
            spaceShip.gameObject.SetActive(true);
            //removeAsteroids ();
			StartCoroutine(DestructionWait(5.0f));
        }
    }

    void gameOver() {
		gameStarted = false;
		removeAsteroids();
        if(score > highScore && score > 0) {
            highScore = score;
            highScoreText.GetComponent<UnityEngine.UI.Text>().text = "High score: " + highScore.ToString();
            gameOverText.GetComponent<UnityEngine.UI.Text>().text = "Game Over!\n New high Score!\n" + score.ToString() + "\n Press ENTER to try again";
        } else {
            gameOverText.GetComponent<UnityEngine.UI.Text>().text = "Game Over!\n Press ENTER to try again";
        }
		gameOverMusic.Play ();
        spaceShip.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
    }

    void Update () {
        if (!gameStarted) { 
            checkInput();
			//removeAsteroids();
        } else {
            refreshScoreAndLives();
            spawnStars();
            switch (level) {
                case 1:
                    level1Update();
                    break;
                case 2:
                    level2Update();
                    break;
                case 3:
                    level3Update();
                    break;
            }
        }
    }

    void level1Update() {
        spawnAsteroid();
    }

    void level2Update() {

    }

    void level3Update() {

    }

    void Awake () {
		if (instance == null) {
			instance = this;
		}

		else if (instance != this) {
			Destroy(gameObject); 
		}
	}

    public void addScore(int score) {
        this.score += score;
    }

	IEnumerator DestructionWait(float duration) {
		spaceShip.gameObject.GetComponent<Collider2D> ().enabled = false;
		spaceShip.gameObject.GetComponent<SpaceShip> ().enableFlash (true);
		yield return new WaitForSeconds(duration);
		spaceShip.gameObject.GetComponent<Collider2D> ().enabled = true;
		spaceShip.gameObject.GetComponent<SpaceShip> ().enableFlash (false);
	}
}
