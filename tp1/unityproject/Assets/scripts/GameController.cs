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
    public GameObject bossPrefab;

	public GameObject starPrefab;
	public GameObject boltPrefab;
    public GameObject spaceShip;
    public GameObject startText;
    public GameObject scoreText;
    public GameObject highScoreText;
    public GameObject startGameText;
    public GameObject startGameArrowText;
    public GameObject quitGameText;
    public GameObject quitGameArrowText;
    public GameObject livesText;
    public GameObject levelText;
    public GameObject gameOverText;
    public GameObject messageText;

    public int asteroidPoolSize;
	public int mediumAsteroidPoolSize;
	public int starPoolSize;
	public int boltPoolSize;
    
    public AudioSource startMusic;
	public AudioSource gameOverMusic;
    public AudioSource winMusic;
    public AudioSource level1Music;
    public AudioSource level2Music;
    public AudioSource level3Music;
    public AudioSource computerVoice1;
    public AudioSource computerVoice2;
    public AudioSource computerVoice3;
    public AudioSource selectSound;

    private float nextActionTime;
	private float nextStarActionTime;
    public float period;
	public float starPeriod;
    private bool gameStarted = false;
    private bool gameIsOver = false;
    private int level;
    private int asteroidsDestroyed;
    private int blueEnemiesDestroyed;
    private int levelState;
    private bool bossIsSpawn;

    private List<GameObject> largeAsteroidList;
	private List<GameObject> mediumAsteroidList;
	private List<GameObject> starList;
	private List<GameObject> boltList;
    private List<GameObject> blueEnemyList;
    private int menuOption;

    // Use this for initialization
    void Start () {
        menuOption = 1;
        startMusic.Play();
        startText.GetComponent<UnityEngine.UI.Text>().text = "ASTEROIDS";
        this.nextActionTime = 0.0f;
		this.nextStarActionTime = 0.0f;
        this.period = 3.0f;
		this.starPeriod = 1.0f;
        this.level = 1;
	}

    private void createBlueEnemyPool() {
        blueEnemyList = new List<GameObject>();
        for(int i = 0; i < 3; i++) {
            GameObject obj = (GameObject)Instantiate(blueEnemyPrefab);
            obj.SetActive(false);
            blueEnemyList.Add(obj);
        }
    }

    public void getAsteroidExplosion(Vector3 position) {
        GameObject obj = (GameObject)Instantiate(asteroidExplosionPrefab);
        obj.transform.position = position;
        obj.gameObject.GetComponent<ParticleSystem>().time = 0;
        obj.gameObject.GetComponent<ParticleSystem>().Play();
        obj.SetActive(true);
        StartCoroutine(DelayedDestroyObject(obj, 5));
    }

    IEnumerator DelayedDestroyObject(GameObject obj, float seconds) {
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
        Destroy(obj);
        
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
        GameObject obj = (GameObject)Instantiate(boltPrefab);
        obj.SetActive(true);
        return obj;
	}

	public void returnBolt(GameObject bolt) {
		bolt.SetActive (true);
        Destroy(bolt);
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
            return obj;
        }
        return null;
    }

	public void returnStar(GameObject star) {
		star.SetActive(false);
		starList.Add (star);
	}

    public void destroyAsteroid() {
        this.asteroidsDestroyed = 1 + this.asteroidsDestroyed;
    }

    void checkInput() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            menuOption = menuOption + 1 == 2 ? 2 : 1;
            changeMenuSelected();
            selectSound.Play();
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            menuOption = menuOption - 1 == 0 ? 2 : 1;
            changeMenuSelected();
            selectSound.Play();
        } else if (Input.GetKeyDown(KeyCode.Return)) {
            if (gameIsOver) {
                gameOverMusic.Stop();
                winMusic.Stop();
                startMusic.volume = 1.0f;
                startMusic.Play();
                gameOverText.gameObject.SetActive(false);
                startText.gameObject.SetActive(true);
                startGameText.gameObject.SetActive(true);
                quitGameText.gameObject.SetActive(true);
                startGameArrowText.gameObject.SetActive(true);
                quitGameArrowText.gameObject.SetActive(true);
                menuOption = 1;
                changeMenuSelected();
                gameIsOver = false;
                removeAsteroids();
                removeEnemies();
                removeBolts();
            } else {
                if (menuOption == 1) {
                    startGame();
                } else {
                    quitGame();
                }
            }
            
        }
    }

    private void changeMenuSelected() {
        if(menuOption == 1) {
            startGameArrowText.GetComponent<UnityEngine.UI.Text>().text = "->";
            quitGameArrowText.GetComponent<UnityEngine.UI.Text>().text = "";
        } else if (menuOption == 2) {
            startGameArrowText.GetComponent<UnityEngine.UI.Text>().text = "";
            quitGameArrowText.GetComponent<UnityEngine.UI.Text>().text = "->";
        }
    }
    private void quitGame() {
        Application.Quit();
    }

    void destroyBlueEnemies() {
        for(int i = 0; i < 4; i++) {
            GameObject obj = GameObject.FindWithTag("BlueEnemy") as GameObject;
            if(obj == null) {
                return;
            }
            obj.SetActive(false);
            Destroy(obj);
        }
    }

    void startGame() {
        gameIsOver = false;
        winMusic.Stop();
        gameOverMusic.Stop();
        gameStarted = true;
        this.lives = 2;
        this.score = 0;
		this.nextActionTime = Time.time;
		this.nextStarActionTime = Time.time;
        this.period = 3.0f;
        this.starPeriod = 1.0f;
        this.level = 1;
        this.levelState = 0;
        this.asteroidsDestroyed = 0;
        this.blueEnemiesDestroyed = 0;

        removeAsteroids();
        removeEnemies();
        removeBolts();

        createAsteroidPool();
        createBlueEnemyPool();
        createStarPool();
        createBoltPool();
        createMediumAsteroidPool();
        destroyBlueEnemies();
        spaceShip.gameObject.GetComponent<SpaceShip>().restartPosition();
        gameOverText.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);
        startGameText.gameObject.SetActive(false);
        quitGameText.gameObject.SetActive(false);
        startGameArrowText.gameObject.SetActive(false);
        quitGameArrowText.gameObject.SetActive(false);
        //scoreText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        spaceShip.gameObject.SetActive(true);
        if(highScore>0) {
            //highScoreText.gameObject.SetActive(true);
        } else {
            //highScoreText.gameObject.SetActive(false);
        }
    }

    void refreshUITexts() {
        //scoreText.GetComponent<UnityEngine.UI.Text>().text = "Score: " + score.ToString();
        livesText.GetComponent<UnityEngine.UI.Text>().text = "Lives: " + lives.ToString();
        //highScoreText.GetComponent<UnityEngine.UI.Text>().text = "High Score: " + highScore.ToString();
        levelText.GetComponent<UnityEngine.UI.Text>().text = "Level: " + level;
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
            obj.SetActive(true);
            obj.GetComponent<BlueEnemy>().resetBlueEnemy();
        }
    }

    void spawnBoss() {
        GameObject obj = (GameObject)Instantiate(bossPrefab);
        obj.SetActive(true);
    }

    void removeEnemies() { 
        if (GameObject.FindGameObjectsWithTag("BlueEnemy").Length > 0) {
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("BlueEnemy")) {
                obj.SetActive(true);
                Destroy(obj);
            }
        }
        if (GameObject.FindGameObjectsWithTag("Boss").Length > 0) {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Boss")) {
                obj.SetActive(true);
                Destroy(obj);
            }
        }
    }

    void removeAsteroids() {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("largeAsteroid")) {
            obj.SetActive(true);
            Destroy(obj);
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("mediumAsteroid")) {
            obj.SetActive(true);
            Destroy(obj);
        }

//		createAsteroidPool ();
//		createMediumAsteroidPool ();
//		this.nextActionTime = Time.time;
//		this.nextStarActionTime = Time.time;
    }

    void removeBolts() {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("bolt")) {
            obj.SetActive(true);
            Destroy(obj);
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("EnemyBolt")) {
            obj.SetActive(true);
            Destroy(obj);
        }

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

    void winGame() {
        stopLevelMusic();
        winMusic.Play();
        gameStarted = false;
        gameIsOver = true;
        removeAsteroids();
        removeEnemies();
        removeBolts();
        if (score > highScore && score > 0) {
            highScore = score;
            //highScoreText.GetComponent<UnityEngine.UI.Text>().text = "High score: " + highScore.ToString();
            gameOverText.GetComponent<UnityEngine.UI.Text>().text = "You win!!\n Press ENTER to return to main menu";
        } else {
            gameOverText.GetComponent<UnityEngine.UI.Text>().text = "You win!!\n Press ENTER to return to main menu";
        }

        //gameOverMusic.Play();
        spaceShip.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        //scoreText.gameObject.SetActive(false);
        levelText.gameObject.SetActive(false);
        //highScoreText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
    }

    private void stopLevelMusic() {
        level1Music.Stop();
        level2Music.Stop();
        level3Music.Stop();

        level1Music.volume = 0.0f;
        level2Music.volume = 0.0f;
        level3Music.volume = 0.0f;
    }

    void gameOver() {
        gameIsOver = true;
        stopLevelMusic();
        gameOverMusic.Play();
        gameStarted = false;
        if (score > highScore && score > 0) {
            highScore = score;
            //highScoreText.GetComponent<UnityEngine.UI.Text>().text = "High score: " + highScore.ToString();
            gameOverText.GetComponent<UnityEngine.UI.Text>().text = "Game Over!\n Press ENTER to return to main menu";
        } else {
            gameOverText.GetComponent<UnityEngine.UI.Text>().text = "Game Over!\n Press ENTER to return to main menu";
        }
        spaceShip.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        //scoreText.gameObject.SetActive(false);
        levelText.gameObject.SetActive(false);
        //highScoreText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
    }

    void Update () {
        if (!gameStarted) { 
            checkInput();
			//removeAsteroids();
        } else {
            refreshUITexts();
            //spawnStars();
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
        fadeOutMusic(startMusic, 0.0f, 0.75f);
        fadeInMusic(level1Music, 0.6f, 0.03f);
        switch (this.levelState) {
            case 0:
                StartCoroutine(ShowMessage("Level " + this.level, 3));
                computerVoice1.Play();
                level1Music.Play();
                StartCoroutine(changeLevelState(2, 8));
                this.levelState = 1;
                break;
            case 1:
                break;
            case 2:
                spawnAsteroid();
                if (asteroidsDestroyed == 30) {
                    levelState = 3;
                }
                break;
            case 3:
                level = 2;
                levelState = 0;
                break;
        }        
    }

    void level2Update() {
        fadeOutMusic(level1Music, 0.0f, 0.75f);
        fadeInMusic(level2Music, 0.6f, 0.03f);
        switch (this.levelState) {
            case 0:
                StartCoroutine(ShowMessage("Level " + this.level, 3));
                computerVoice2.Play();
                level2Music.Play();
                StartCoroutine(changeLevelState(2, 6));
                this.levelState = 1;
                break;
            case 1:
                break;
            case 2:
                spawnBlueEnemy();
                if (GameObject.FindGameObjectsWithTag("BlueEnemy").Length == 0) {
                    levelState = 3;
                }
                break;
            case 3:
                level = 3;
                levelState = 0;
                break;
        }
    }

    void level3Update() {
        fadeOutMusic(level2Music, 0.0f, 0.75f);
        fadeInMusic(level3Music, 0.6f, 0.03f);
        switch (this.levelState)
        {
            case 0:
                StartCoroutine(ShowMessage("Level " + this.level, 3));
                computerVoice3.Play();
                level3Music.Play();
                StartCoroutine(changeLevelState(2, 4));
                this.levelState = 1;
                this.bossIsSpawn = false;
                break;
            case 1:
                break;
            case 2:
                if (!bossIsSpawn) {
                    spawnBoss();
                    this.bossIsSpawn = true;
                }

                if (GameObject.FindGameObjectsWithTag("Boss").Length == 0) {
                    winGame();
                }
                break;
        }
    }

    IEnumerator changeLevelState(int state,  float delay) {
        yield return new WaitForSeconds(delay);
        this.levelState = state;
    }

    IEnumerator ShowMessage(string message, float delay) {
        messageText.GetComponent<UnityEngine.UI.Text>().text = message;
        messageText.GetComponent<UnityEngine.UI.Text>().enabled = true;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        messageText.GetComponent<UnityEngine.UI.Text>().enabled = false;
        messageText.gameObject.SetActive(false);
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

    private void fadeInMusic(AudioSource audio, float maxVolume, float step) {
        if (audio.volume < maxVolume) {
            audio.volume += step * Time.deltaTime;
        }
    }

    private void fadeOutMusic(AudioSource audio, float minVolume, float step) {
        if (audio.volume > minVolume) {
            audio.volume -= step * Time.deltaTime;
        }
    }


	IEnumerator DestructionWait(float duration) {
		spaceShip.gameObject.GetComponent<Collider2D> ().enabled = false;
		spaceShip.gameObject.GetComponent<SpaceShip> ().enableFlash (true);
		yield return new WaitForSeconds(duration);
		spaceShip.gameObject.GetComponent<Collider2D> ().enabled = true;
		spaceShip.gameObject.GetComponent<SpaceShip> ().enableFlash (false);
	}
}
