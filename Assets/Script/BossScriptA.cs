using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Script for Boss A - Kardinal
public class BossScriptA : BossController
{
	// patterns and stats
	public int _pattern = 1;							// the pattern in which this boss is currently acting under
			
	public Vector2 _offscreenPos;					// position where the boss is spawned at
	public Vector2 _startingPos;						// starting position for the boss fight
	public List<Vector2> _movePoints;				// movement points used in normal novement
	public float _moveSpeed = 0.25f;					// speed at which this enemy moves
			
	private int _curMovePoint = 0;					// last move point reached (only matters for patterns that rely on points)
	private float _timeAlongCurPoint = 0f;				// how long since we reached the last move point (pattern 2)
	public float _pointDampenFactor = 0.4f;			// dampens the speed of movement by this value when close to the next point
			
	public float _shotDelay = 2.2f;						// delay between normal shots
	public float _mainGunDelay = 0.2f;				// delay between main gun shots
	public float _shotSpeed = 0.50f;					// the speed of the bullets fired by this enemy
	public float _mainGunShotSpeed = 0.25f;			// the speed of the bullets fired by the main gun
	public float _mainGunBurstDuration = 4f;			// time the main gun fires burst shots for
	public float _mainGunCooldown = 2f;				// time the main gun shuts down after firing
	public Vector2 _shotDirection = new Vector2(0,-1);	// the angle of the main gun shot
	
	public float _waitBeforeFight = 2f;				// time the boss stands still after reaching the fight start point but before starting the fight logic
	private float _curWaitBeforeFight = 0f;			// current waiting time before fight
		
	private float[] _curShotDelay = {0f,0.25f,0.5f,0.75f};	// current shot timing; once it equals the delay, it fires
	private float _curMainGunDelay = 0f;				// current timing for the main gun shots
	private float _curGunBurst = 0f;					// burst timing for current cluster of main gun shots
	private float _curMainGunCooldown = 0f;			// current cooldown wait for the main gun
	
	private float _hpThresholdAPercent = 0.7f;
	private int _hpThresholdAValue;
	
	private bool _invincible = true;					// can the boss currently take damage?
			
	public List<GameObject> _weapons;				// list of the weapons (shot spawn points) this boss holds
	
	public void SetParameters()
	{
		this._hpThresholdAValue = (int)Mathf.Ceil(_health * _hpThresholdAPercent);
		this._movePoints = new List<Vector2> { new Vector2(0.5f, 2.5f), new Vector2(-0.5f, 2.5f)};
	}
	
	/* *************************** */
	
	/* --- BOSS A - KARDINAL ---
		---- PATTERNS ----
	Pattern 1:
		Move from offscreen into starting position.
	Pattern 2:
		Wait for "_waitBeforeFight" seconds before engaging in combat; is invincible before this happens
	Pattern 3: 
		Slowly move left to right, firing staggered shots from all four side cannons at the player.
	Pattern 4:
		Triggered at low health, the main cannon will fire intermittent rounds of many slow bullets towards the center.
	*/
	new void Start()
	{
		base.Start();
		SetParameters();
	}
	
	new void Update ()
	{
		base.Update();
		Vector2 playerDir;
		switch (_pattern)
		{
			case 1:
				// pattern 1 (moving to position), boss doesn't fire yet
				break;
			case 2:
				// pattern 2: wait before fight
				_curWaitBeforeFight += Time.deltaTime;
				if (_curWaitBeforeFight >= _waitBeforeFight)
				{
					_invincible = false;
					_pattern = 3;
				}
				break;
			case 3:
				// pattern 3: fire from all four cannons at player
				// check if the player is currently alive
				if(!_playerTransform)
					_playerTransform = GameObject.FindWithTag("Player")?.transform;
				// if the player was succesfully found, we generate a shot aimed at the player's current position
				if(_playerTransform)
				{
					for (int x = 0; x < _curShotDelay.Length; x++)
					{
						_curShotDelay[x] += Time.deltaTime;
						if(_curShotDelay[x] >= _shotDelay)
						{
							playerDir = new Vector2(_playerTransform.position.x - _weapons[x].transform.position.x, _playerTransform.position.y - _weapons[x].transform.position.y);
							_curShotDelay[x] = 0;
							GameObject shotObj = Resources.Load<GameObject>("Shot Prefabs/EnemyShotA");
							GameObject shotInst = Instantiate(shotObj, _weapons[x].transform.position, this.transform.rotation);
							shotInst.GetComponent<ShotScript>().SetParameters( spd: _shotSpeed, dir: playerDir);
						}
					}
				}
				break;
			case 4:
				// pattern 4: in addition to the four cannons, also fire the center cannon ('main gun') intermittently straight down
				// check if the player is currently alive
				if(!_playerTransform)
					_playerTransform = GameObject.FindWithTag("Player")?.transform;
				// if the player was succesfully found, we generate a shot aimed at the player's current position
				if(_playerTransform)
				{
					for (int x = 0; x < _curShotDelay.Length; x++)
					{
						_curShotDelay[x] += Time.deltaTime;
						if(_curShotDelay[x] >= _shotDelay)
						{
							playerDir = new Vector2(_playerTransform.position.x - _weapons[x].transform.position.x, _playerTransform.position.y - _weapons[x].transform.position.y);
							_curShotDelay[x] = 0;
							GameObject shotObj = Resources.Load<GameObject>("Shot Prefabs/EnemyShotA");
							GameObject shotInst = Instantiate(shotObj, _weapons[x].transform.position, this.transform.rotation);
							shotInst.GetComponent<ShotScript>().SetParameters( spd: _shotSpeed, dir: playerDir);
						}
					}
				}
				// the main gun doesn't care if the player is alive or not
				// check if the current gun burst is within its max duration
				if(_curGunBurst < _mainGunBurstDuration)
				{
					// if yes, we add to the current shot timer, and fire when appropriate
					_curGunBurst += Time.deltaTime;
					_curMainGunDelay += Time.deltaTime;
					if(_curMainGunDelay >= _mainGunDelay)
					{
						_curMainGunDelay = 0;
						GameObject shotObj = Resources.Load<GameObject>("Shot Prefabs/EnemyShotB");
						GameObject shotInst = Instantiate(shotObj, _weapons[4].transform.position, this.transform.rotation);
						shotInst.GetComponent<ShotScript>().SetParameters( spd: _mainGunShotSpeed, dir: _shotDirection);
					}
				} else {
					// if it's over duration, we run the cooldown timer
					_curMainGunCooldown += Time.deltaTime;
					if(_curMainGunCooldown >= _mainGunCooldown)
					{
						_curMainGunCooldown = 0;
						_curGunBurst = 0;
					}
				}
				break;
			default:
				Debug.Log("Operational error: boss A (Kardinal) tried to move into an undefined pattern.");
				break;
		}
	}
	
	void FixedUpdate ()
	{
		// we get the camera's curdistance (a.k.a how far along the level we are) to shift the points up appropriately, so that we only need to pass relative coordinates rather than absolute
		float curDistance = _cam.GetCurDistance();
		switch (_pattern)
		{
			case 1:
				// pattern 1, moving to start position
				// we move it along based on its speed (speed 1 = 1 second between points)
				_timeAlongCurPoint += Time.deltaTime * _moveSpeed;
				// lerp along the previous and next points by the factor we've been adding to
				Vector3 oldPos1 = new Vector3(_offscreenPos.x, _offscreenPos.y + curDistance, _ZHEIGHT);
				Vector3 target1 = new Vector3(_startingPos.x, _startingPos.y + curDistance, _ZHEIGHT);
				this.transform.position = Vector3.Lerp(oldPos1, target1, _timeAlongCurPoint);
				
				if(_timeAlongCurPoint >= 1f)
				{
					// once we reach the starting position, we shift to pattern 2
					_timeAlongCurPoint = 0.5f;
					_pattern = 2;
				}
				break;
			case 2:
				// pattern 2: wait before fight; no movement logic here
				break;
			case 3:
			case 4:
				// patterns 3 and 4, the boss moves side to side while firing at the player
				// we move it along based on its speed (speed 1 = 1 second between points)
				_timeAlongCurPoint += Time.deltaTime * (_timeAlongCurPoint >= 0.8f ? _moveSpeed * _pointDampenFactor : _moveSpeed);
				// lerp along the previous and next points by the factor we've been adding to
				int targetPoint = _curMovePoint == 0 ? 1 : 0;		// the boss always moves between points 0 and 1
				Vector3 oldPos2 = new Vector3(_movePoints[_curMovePoint].x, _movePoints[_curMovePoint].y + curDistance, _ZHEIGHT);
				Vector3 target2 = new Vector3(_movePoints[targetPoint].x, _movePoints[targetPoint].y + curDistance, _ZHEIGHT);
				this.transform.position = Vector3.Lerp(oldPos2, target2, _timeAlongCurPoint);
				
				if(_timeAlongCurPoint >= 1f)
				{
					_timeAlongCurPoint = 0;
					_curMovePoint = _curMovePoint == 0 ? 1 : 0;
				}
				break;
			default:
				Debug.Log("Operational error: boss A (Kardinal) tried to move into an undefined pattern.");
				break;
		}
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		if(col.gameObject.tag == "PlayerProj")
		{
			Destroy(col.gameObject);
			if(!_invincible)
			{
				this._health -= col.gameObject.GetComponent<ShotScript>().GetDamage();
				this.SetHealthBar((float)_health/(float)_maxHealth);
			
				if(_pattern <= 3 && _health <= _hpThresholdAValue)
					_pattern = 4;
				
				// boss defeated!
				if (_health <= 0)
				{
					_gm.AddScore(_scoreValue);
					// clean up all of the boss shots left over
					GameObject[] bossShots = GameObject.FindGameObjectsWithTag("EnemyProj");
					for(int y = 0; y < bossShots.Length; y++)
						Destroy(bossShots[y]);
					Debug.Log("Boss Kardinal defeated!");
					base.SignalLevelEnd();
					_hud.TurnOffBossBar();
					Destroy(this.gameObject);
				}
			}
		}
	}
}
