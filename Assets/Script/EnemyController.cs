using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	private const int _POWERUPODDS = 10; 		// percentage of item drop after destruction
	private const float _MINTIMEALIVE = 3f;		// minimum time alive before this enemy is wiped from memory
	private const float _ZHEIGHT = -1f;			// fixed Z height of this enemy
	
	private Vector2 _movement;
	
	private GameManager _gm;
	private Rigidbody2D _rb;
	private SpriteRenderer _rend;
	private CameraScript _cam;
	private Transform _playerTransform;
	
	public int _scoreValue = 10;
	public int _health = 3;
	
	// patterns and stats
	public int _movePattern = 1;				// the pattern in which the enemy moves on the screen
	public Vector2 _moveParam;				// base movement direction
	public List<Vector2> _movePoints;		// movement points used in pattern 2
	public Vector2 _moveMod;				// movement direction modifier, used in pattern 3
	public float _moveSpeed = 1f;			// speed at which this enemy moves
	private float _timeBeforeLeaving = 4f;		// for pattern 4, identifies how long the enemy stays before leaving to point moveMod
	private float _curTimeBfrLeaving = 0f;		// current time before leaving (see above)
	private int _curMovePoint = 0;			// last move point reached (only matters for patterns that rely on points)
	private float _timeAlongCurPoint = 0f;		// how long since we reached the last move point (pattern 2)
	public float _pointDampenFactor = 0.5f;	// dampens the speed of movement by this value when close to the next point
	
	public int _shotPattern = 0;				// the pattern in which the enemy fires
	public float _firstShotDelay = 0f;			// delay before the first shot
	public float _shotDelay = 0f;				// delay between shots (irrelevant if it doesn't fire)
	public int _shotsMax = 0;				// how many shots it fires in total
	public float _shotSpeed = 3;				// the speed of the bullets fired by this enemy
	public Vector2 _shotDirection;			// the angle this enemy shoots at, if pattern 1

	private float _lifetime = 0f;				// how long this enemy has been alive for		
	private float _curShotDelay = 0f;			// current shot timing; once it equals the delay, it fires
	private int _shotsShot = 0;				// how many shots it has currently fired
	
	private string _itemDrop = "";			// whether this enemy drops an item on death
	
	/* --- MOVE PATTERNS ---
	0 - doesn't move at all
	1 - straight line in a given direction
	2 - follows a number of set points on the screen, and hangs until destroyed
	3 - curves out in a given direction (uses moveMod as a modifier)
	4 - follow a number of set points, and move offscreen to point moveMod after a certain timing
	*/
	
	/* --- SHOT PATTERNS ---
	0 - doesn't shoot at all
	1 - shoots in a passed direction (usually straight down)
	2 - shoots directly at the player
	*/
	
	public void SetParameters (int score, int health, int movePat, float moveSpeed, Vector2 moveParam, List<Vector2> movePoints, Vector2 moveMod, float dampenFactor, 
		int shotPattern, float firstShotDelay, float shotDelay, int shotsMax, float shotSpeed, Vector2 shotDirection, string itemDrop)
	{
		this._scoreValue = score;
		this._health = health;
		
		this._movePattern = movePat;
		this._moveSpeed = moveSpeed;
		this._moveParam = moveParam;
		this._movePoints = movePoints;
		this._moveMod = moveMod;
		this._pointDampenFactor = dampenFactor;
		
		this._shotPattern = shotPattern;
		this._firstShotDelay = firstShotDelay;
		this._shotDelay = shotDelay;
		this._shotsMax = shotsMax;
		this._shotSpeed = shotSpeed;
		this._shotDirection = shotDirection;
		
		this._itemDrop = itemDrop;
	}
	
	/* ******************************** */

	void Awake()
	{
		_gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
		_cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraScript>();
		_rb = GetComponent<Rigidbody2D>();
		_rend = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		// check how long this has been alive, and remove it if it's offscreen and old enough
		_lifetime += Time.deltaTime;
		if(!_rend.isVisible && _lifetime >= _MINTIMEALIVE)
			Destroy(this.gameObject);
		
		if(_shotPattern >= 1)
		{
			switch(_shotPattern)
			{
				case 1:
					_curShotDelay += Time.deltaTime;
					if((_shotsShot == 0 && _curShotDelay >= _firstShotDelay) || (_shotsShot >= 1 && _shotsShot < _shotsMax && _curShotDelay >= _shotDelay))
					{
						_shotsShot ++;
						_curShotDelay = 0;
						GameObject shotObj = Resources.Load<GameObject>("Shot Prefabs/EnemyShotA");
						GameObject shotInst = Instantiate(shotObj, this.transform.position, this.transform.rotation);
						shotInst.GetComponent<ShotScript>().SetParameters( spd: _shotSpeed, dir: _shotDirection);
					}
					break;
				case 2:
					_curShotDelay += Time.deltaTime;
					if((_shotsShot == 0 && _curShotDelay >= _firstShotDelay) || (_shotsShot >= 1 && _shotsShot < _shotsMax && _curShotDelay >= _shotDelay))
					{
						_shotsShot ++;
						_curShotDelay = 0;
						
						// check if the player is currently alive
						if(!_playerTransform)
						{
							_playerTransform = GameObject.FindWithTag("Player")?.transform;
						}
						// if the player was succesfully found, we generate a shot aimed at the player's current position
						if(_playerTransform)
						{
							Vector2 playerDir = new Vector2(_playerTransform.position.x - this.transform.position.x, _playerTransform.position.y - this.transform.position.y);
							GameObject shotObj = Resources.Load<GameObject>("Shot Prefabs/EnemyShotA");
							GameObject shotInst = Instantiate(shotObj, this.transform.position, this.transform.rotation);
							shotInst.GetComponent<ShotScript>().SetParameters( spd: _shotSpeed, dir: playerDir);
						}
					}
					break;
				default:
					Debug.Log("Operational error: shot pattern passed doesn't actually exist (enemy "+this.gameObject+")");
					break;
			}
		}
	}
	
	void FixedUpdate()
	{
		// check move pattern and act accordingly; move pattern 0 means it never moves
		if(_movePattern >= 1)
		{
			switch(_movePattern)
			{
				case 1:
					_movement = new Vector2(_moveParam.x * _moveSpeed, _moveParam.y * _moveSpeed);
					break;
				case 2:
					// pattern 2: enemy moves to a number of set points in order
					if(_curMovePoint < _movePoints.Count-1)
					{
						// we move it along based on its speed (speed 1 = 1 second between points); if time is over 0.8, we dampen movement by _pointDampenFactor (if factor is 1, dampening has no effect)
						_timeAlongCurPoint += Time.deltaTime * (_timeAlongCurPoint >= 0.8f ? _moveSpeed * _pointDampenFactor : _moveSpeed);
						// we get the camera's curdistance (a.k.a how far along the level we are) to shift the points up appropriately, so that we only need to pass relative coordinates rather than absolute
						float curDistance = _cam.GetCurDistance();
						// lerp along the previous and next points by the factor we've been adding to
						Vector3 oldPos = new Vector3(_movePoints[_curMovePoint].x, _movePoints[_curMovePoint].y + curDistance, _ZHEIGHT);
						Vector3 target = new Vector3(_movePoints[_curMovePoint+1].x, _movePoints[_curMovePoint+1].y + curDistance, _ZHEIGHT);
						this.transform.position = Vector3.Lerp(oldPos, target, _timeAlongCurPoint);
						
						if(_timeAlongCurPoint >= 1f)
						{
							_timeAlongCurPoint = 0;
							_curMovePoint++;
						}
					}
					break;
				case 3:
					// pattern 3: same as pattern 1, but we modify the move parameter by the move mod parameter every second, creating curves
					_moveParam = new Vector2(_moveParam.x + (_moveMod.x * Time.deltaTime), _moveParam.y + (_moveMod.y * Time.deltaTime));
					_movement = new Vector2(_moveParam.x * _moveSpeed, _moveParam.y * _moveSpeed);
					break;
				case 4:
					// pattern 4: same as pattern 2, but the enemy moves away after _timeBeforeLeaving seconds
					// if there are still normal move points to visit:
					if(_curMovePoint < _movePoints.Count-1)
					{
						// we move it along based on its speed (speed 1 = 1 second between points); if time is over 0.8, we dampen movement by _pointDampenFactor (if factor is 1, dampening has no effect)
						_timeAlongCurPoint += Time.deltaTime * (_timeAlongCurPoint >= 0.8f ? _moveSpeed * _pointDampenFactor : _moveSpeed);
						// we get the camera's curdistance (a.k.a how far along the level we are) to shift the points up appropriately, so that we only need to pass relative coordinates rather than absolute
						float curDistance = _cam.GetCurDistance();
						// lerp along the previous and next points by the factor we've been adding to
						Vector3 oldPos = new Vector3(_movePoints[_curMovePoint].x, _movePoints[_curMovePoint].y + curDistance, _ZHEIGHT);
						Vector3 target = new Vector3(_movePoints[_curMovePoint+1].x, _movePoints[_curMovePoint+1].y + curDistance, _ZHEIGHT);
						this.transform.position = Vector3.Lerp(oldPos, target, _timeAlongCurPoint);
						
						if(_timeAlongCurPoint >= 1f)
						{
							_timeAlongCurPoint = 0;
							_curMovePoint++;
						}
					}
					// visited all movePoints, now we hang around for _timeBeforeLeaving seconds
					else
					{
						if(_curTimeBfrLeaving <= _timeBeforeLeaving)
						{
							_curTimeBfrLeaving += Time.deltaTime;
						}
						// after waiting _timeBeforeLeaving seconds, we move this enemy to the last point (which SHOULD always be offscreen), and destroy it
						else
						{
							// we move it along based on its speed (speed 1 = 1 second between points)
							_timeAlongCurPoint += Time.deltaTime * _moveSpeed;
							// we get the camera's curdistance (a.k.a how far along the level we are) to shift the points up appropriately, so that we only need to pass relative coordinates rather than absolute
							float curDistance = _cam.GetCurDistance();
							// lerp along the previous and next points by the factor we've been adding to
							Vector3 oldPos = new Vector3(_movePoints[_curMovePoint].x, _movePoints[_curMovePoint].y + curDistance, _ZHEIGHT);
							Vector3 target = new Vector3(_moveMod.x, _moveMod.y + curDistance, _ZHEIGHT);
							this.transform.position = Vector3.Lerp(oldPos, target, _timeAlongCurPoint);
							
							if(_timeAlongCurPoint >= 1f)
							{
								Destroy(this.gameObject);
							}
						}
					}
					break;
				default:
					Debug.Log("Operational error: move pattern passed doesn't actually exist (enemy "+this.gameObject+")");
					break;
			}
		}
		
		_rb.velocity = _movement;
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.gameObject.tag == "PlayerProj")
		{
			this._health -= col.gameObject.GetComponent<ShotScript>().GetDamage();
			Destroy(col.gameObject);
			
			if (_health <= 0)
			{
				_gm.AddScore(_scoreValue);
				if(!string.IsNullOrWhiteSpace(_itemDrop))
					DropItem();
				Destroy(this.gameObject);
			}
		}
	}
	
	/* ******************************** */
	// drops an item upon death
	private void DropItem ()
	{
		GameObject powerup = Resources.Load<GameObject>("Pickup Prefabs/"+_itemDrop);
		Instantiate(powerup, this.transform.position, this.transform.rotation);
	}
	
	// add this enemy to the camera's "static enemy list"; 'static' enemies keep up with the camera movement
	public void AddToStaticList ()
	{
		_cam.AddToStaticList(this.gameObject);
	}
}
