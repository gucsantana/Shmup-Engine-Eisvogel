using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private const int _MAXLVL = 3;
	private const float _RESPAWNINVMAX = 2.5f;
	private const float _TIMETORESPAWNANCHOR = 0.8f;
	private const float _WAITATANCHORMAX = 0.25f;
	private const float _PLAYERZHEIGHT = -1f;
	
	/* ******************************** */
	/// --- Singleton definitions ---
	private static PlayerController _instance;
	public static PlayerController Instance { get { return _instance; } }
	
	private Vector2 _movement;
	private Vector2 _speed = new Vector2(5,5);
	
	private GameManager _gm;
	private Rigidbody2D _rb;
	private BoxCollider2D _col;
	private WeaponScript _wp;
	private Transform _rAnchorA;
	private Transform _rAnchorB;
	
	private bool _inControl = true;	// the player loses control during respawns and level transitions
	public bool _invincible = false;	// the player is briefly invincible after respawning
	
	public float  _invincibleCd = 0f;
	private bool _isMovingToAnchor = false;
	private bool _isWaitingAtAnchor = false;
	private float _timeMovingToAnchor = 0f;
	private float _waitAtAnchor = 0f;
	
	/* ******************************** */
	
	public bool IsInControl()
	{
		return _inControl;
	}
	public void SetInControl(bool con)
	{
		_inControl = con;
	}
	
	/* ******************************** */

	/// set up dependencies and components
	public void Awake()
	{
		// singleton logic, to avoid duplicates
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
		
		_gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
		_rAnchorA = GameObject.Find("Respawn Anchor A").transform;
		_rAnchorB = GameObject.Find("Respawn Anchor B").transform;
		_rb = GetComponent<Rigidbody2D>();
		_col = GetComponent<BoxCollider2D>();
		_wp = GetComponent<WeaponScript>();
		
		if (!_wp || !_rb || !_col)
			Debug.Log("Operational error: the player prefab needs a Rigidbody2D, a BoxCollider2D, and the WeaponScript.");
	}

	void Update()
	{
		// the player must be in control for any movement and attack triggers to actually proc
		if(_inControl)
		{
			float inputX = Input.GetAxis("Horizontal");
			float inputY = Input.GetAxis("Vertical");
			
			_movement = new Vector2(inputX * _speed.x, inputY * _speed.y);
			
			if(Input.GetButton("Fire") && _wp.CanAttack())
			{
				_wp.Attack();
			}
		}
		
		// if we're currently invincible, wind down the invincibility timer
		if(_invincible)
		{
			_invincibleCd += Time.deltaTime;
			if (_invincibleCd >= _RESPAWNINVMAX)
			{
				_invincible = false;
				_invincibleCd = 0f;
			}
		}
		
		// if the player has just respawned, we used these 'to anchor' variables and values to play out a little animation of the new ship moving back onscreen
		if(_isMovingToAnchor)
		{
			_timeMovingToAnchor += Time.deltaTime;
			// interpolate between the two anchors to get the current position of the ship - the Z height fix makes sure we can ignore the camera's height and relative position
			Vector3 _posA = new Vector3(_rAnchorA.position.x, _rAnchorA.position.y, _PLAYERZHEIGHT);
			Vector3 _posB = new Vector3(_rAnchorB.position.x, _rAnchorB.position.y, _PLAYERZHEIGHT);
			this.transform.position = Vector3.Lerp(_posA, _posB, (_timeMovingToAnchor/_TIMETORESPAWNANCHOR));
			
			if (_timeMovingToAnchor >= _TIMETORESPAWNANCHOR)
			{
				_isMovingToAnchor = false;
				_isWaitingAtAnchor = true;
				_col.enabled = true;
				_timeMovingToAnchor = 0f;
			}
		}
		if(_isWaitingAtAnchor)
		{
			_waitAtAnchor += Time.deltaTime;
			if (_waitAtAnchor >= _WAITATANCHORMAX)
			{
				_isWaitingAtAnchor = false;
				_inControl = true;
				_waitAtAnchor = 0;
			}
		}
	}
	
	void FixedUpdate()
	{
		_rb.velocity = _movement;
	}
	
	void OnTriggerEnter2D (Collider2D col)
	{
		// trigger check: collided with an enemy or enemy projectile (while not invincible)
		if(!_invincible && (col.gameObject.tag == "EnemyProj" || col.gameObject.tag == "Enemy") )
		{
			Destroy(col.gameObject);
			_invincible = true;
			_gm.Death();
		}
		
		// trigger check: collided with a boss
		else if(!_invincible && col.gameObject.tag == "BossEnemy" )
		{
			_invincible = true;
			_gm.Death();
		}
		
		// trigger check: item pickup
		else if(col.gameObject.tag == "Pickup")
		{
			string type = col.gameObject.GetComponent<PickupScript>()?._type;
			Destroy(col.gameObject);
			switch(type)
			{
				case "weapon":
					if(_wp?.GetWeapon()?._curLevel < _MAXLVL)
					{
						_wp.LevelUpWeapon();
						Debug.Log("Weapon level up!");
					}
					else
					{
						_gm?.AddScore(100);
						Debug.Log("Picked up a weapon powerup at max power; added score instead.");
					}
					break;
				default:
					break;
			}
		}
	}
	
	/* ******************************** */
	
	/// moves the player automatically to the start position; this assumes that the player has been spawned at the Anchor A, which should be offscreen
	public void MoveToStartPos()
	{
		_inControl = false;
		_col.enabled = false;
		_invincible = true;
		_isMovingToAnchor = true;
	}
}
