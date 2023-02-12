using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Script attached to flying bullets, describing their movement and parameters
// TODO: Add different varieties of movement per shot, rather than just straight line shots
public class ShotScript : MonoBehaviour
{
	private Vector2 _movement;
	private float _speed = 6f;
	public Vector2 _direction = new Vector2(0,1);
	private int _damage = 1;

	private Rigidbody2D _rb;
	private SpriteRenderer _rend;
	private Collider2D _collider;
	
	/* ******************************** */
	
	public int GetDamage()
	{
		return _damage;
	}
	
	public void SetParameters(float spd, Vector2 dir, int dmg = 0)
	{
		this._speed = spd;
		this._damage = dmg;
		this._direction = dir != null ? dir : new Vector2(0,1);
	}

	/* ******************************** */
	
	void Awake()
	{
		// initialize the rigidbody reference
		_rb = GetComponent<Rigidbody2D>();
		_rend = GetComponent<SpriteRenderer>();
		_collider = GetComponent<Collider2D>();
	}

	void Update()
	{
		_movement = new Vector2(_speed * _direction.x, _speed * _direction.y);
		if(!_rend.isVisible)
			Destroy(this.gameObject);
	}
	
	void FixedUpdate()
	{
		_rb.velocity = _movement;
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		// if a shot hits the edge of the screen, its collider is turned off so that it's rendered harmless against offscreen enemies (or in case a mismanaged enemy shoots at the player from offscreen)
		if(col.gameObject.tag == "CameraBarrier")
			_collider.enabled = false;
	}
}
