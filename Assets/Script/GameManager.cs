using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	/// --- Constants ---
	private const float _WAITBEFORESPAWN = 1f;
	private const int _STARTINGLIVES = 3;
	private const float _TIMEDELAYAFTERLEVELEND = 3F;
	
	/// --- Singleton definitions ---
	private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }
	
	private PlayerController _pl;
	private int _lives = _STARTINGLIVES;
	public int _level = 0;
	private int _score = 0;
	
	private GameObject _respawnAnchorA;
	private LevelBuilder _lb;
	private CameraScript _cam;
	private HUDController _hud;
	
	public Queue<EnemyInstanceClass> _enemyQueue = new Queue<EnemyInstanceClass>();
	public BossInstanceClass _boss;
	public bool _bossSpawned = false;
	
	private bool _gameOver = false;
	
	public void AddScore(int points)
	{
		this._score += points;
		_hud.SetScore(_score);
	}
	
	public void AddLife()
	{
		_lives ++;
		_hud.SetLives(_lives);
	}
	
	public void RemoveLife()
	{
		_lives--;
		_hud.SetLives(_lives);
	}
	
	public void SetStage()
	{
		_hud.SetStageText(_level);
	}
	
	/* ************************** */
	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	
	private void Awake()
	{
		// singleton logic, to avoid duplicates
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}
	
	public IEnumerator Start()
	{
		DontDestroyOnLoad(this);
		
		_lb = GetComponent<LevelBuilder>();
			
		yield return new WaitUntil( ( ) => _lb.IsFinishedLoading());
		
		GetEnemyList(_level);
	}
	
	void Update()
	{
		if(!_cam)
			_cam = GameObject.FindWithTag("MainCamera")?.GetComponent<CameraScript>();
		if(!_hud)
			_hud = GameObject.FindWithTag("HUD")?.GetComponent<HUDController>();
		
		// we check the current distance along the level, and check the queue to see if there are any enemies we should release into the level
		float _curDistance = _cam ? _cam.GetCurDistance() : -1f;
		while(_level >= 1 && _enemyQueue != null && _enemyQueue.Count > 0 && _enemyQueue.Peek()._timing <= _curDistance)
		{
			// dequeue the next enemy on the queue, search for the prefab, spawn it, and fill out its parameters
			EnemyInstanceClass _eC = _enemyQueue.Dequeue();
			GameObject _enemyObj = Resources.Load<GameObject>("Character Prefabs/"+_eC._enemyType);
			GameObject _enemyInstance = Instantiate( _enemyObj, new Vector2(_eC._spawnPoint.x, _eC._spawnPoint.y + _curDistance), new Quaternion());
			_enemyInstance.GetComponent<EnemyController>().SetParameters(
				score: _eC._scoreValue, health: _eC._health, movePat: _eC._movePattern, moveSpeed: _eC._moveSpeed, moveParam: _eC._moveParam, movePoints: _eC._movePoints, dampenFactor: _eC._pointDampenFactor, itemDrop: _eC._itemDrop,
				moveMod: _eC._moveMod, shotPattern: _eC._shotPattern, firstShotDelay: _eC._firstShotDelay, shotDelay: _eC._shotDelay, shotsMax: _eC._shotsMax, shotSpeed: _eC._shotSpeed, shotDirection: _eC._shotDirection);
				
			if(_eC._isStatic)
				_cam.AddToStaticList(_enemyInstance);
		}
		if(_level >= 1 && !_bossSpawned &&_boss != null && _curDistance >= _boss._timing)
		{
			_bossSpawned = true;
			GameObject _bossObj = Resources.Load<GameObject>("Character Prefabs/"+_boss._enemyType);
			GameObject _enemyInstance = Instantiate( _bossObj, new Vector2(_boss._spawnPoint.x, _boss._spawnPoint.y + _curDistance), new Quaternion());
		}
	}
	
	// on scene loaded, we respawn the player and get the appropriate enemy queue
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		switch(scene.name)
		{
			case "Stage_A":
				Debug.Log("Scene A loaded");
				_level = 1;
				_bossSpawned = false;
				GetEnemyList(_level);
				StartCoroutine(RespawnPlayer());
				break;
			default:
				break;
		}
	}
	
	/* ************************** */
	// called to go to level 1 from the main menu
	public void StartGame()
	{
		Debug.Log("Starting a new game from level 1.");
		_level = 1;
		SceneManager.LoadScene("Stage_A");
	}
	
	// called when the player 'dies', by colliding with an enemy or bullet
	public void Death()
	{
		Debug.Log("Player crashed the ship!");
		Destroy(_pl.gameObject);
		if(_lives >= 1)
		{
			RemoveLife();
			StartCoroutine( RespawnPlayer() );
		}
		else
		{
			GameOver();
		}
	}
	
	/// get the list of enemies to populate the level, as well as the level's boss
	public void GetEnemyList(int level)
	{
		if(level >= 1 && _lb && _lb.IsFinishedLoading())
		{
			_enemyQueue = new Queue<EnemyInstanceClass>(_lb.GetEnemyQueue(level - 1));
			_boss = _lb.GetLevelBoss(level - 1);
		}
	}
	
	/// respawn the player offscreen and order the new ship to move into position
	IEnumerator RespawnPlayer()
	{
		// get the respawn anchor to plop the player down at
		_respawnAnchorA = GameObject.Find("Respawn Anchor A");
		if (_respawnAnchorA == null)		// debug statement
			Debug.Log("Operation error: make sure you have both Respawn Anchors A/B attached to the camera.");
			
		// wait for respawn timer, then find the player prefab and spawn a new player at the anchor, ordering it to move in place
		yield return new WaitForSeconds(_WAITBEFORESPAWN);
		GameObject _playerObj = Resources.Load<GameObject>("Character Prefabs/PlayerPrefab");
		GameObject _playerInstance = Instantiate( _playerObj, _respawnAnchorA.transform.position, _respawnAnchorA.transform.rotation);
		_pl = _playerInstance.GetComponent<PlayerController>();
		_pl.MoveToStartPos();
	}
	
	/// trigger the end of the level after a boss is defeated, moving to the next stage (or showing the victory screen, in the case of this demo)
	public void TriggerLevelEnd()
	{
		StartCoroutine(LevelEnd());
	}
	public IEnumerator LevelEnd()
	{
		yield return new WaitForSeconds(_TIMEDELAYAFTERLEVELEND);
		_gameOver = true;
		Time.timeScale = 0;
		_hud.DisplayVictoryScreen(true);
	}
	
	/// restart the stage after a game over, zeroing the score, refreshing the lives, and resetting the enemy list
	public void RestartStage()
	{
		Debug.Log("A");
		if(_gameOver)
		{
			Debug.Log("Stage restarted.");
			Time.timeScale = 1f;
			_lives = _STARTINGLIVES;
			_score = 0;
			_hud.DisplayGameOver(false);
			_hud.DisplayVictoryScreen(false);
			GetEnemyList(_level);
			_gameOver = false;			
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
	
	/// display the game over message when the player runs out of lives
	public void GameOver()
	{
		_gameOver = true;
		Time.timeScale = 0;
		_hud.DisplayGameOver(true);
	}
}
