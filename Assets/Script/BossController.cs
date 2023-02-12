using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Base class for Boss-type enemies; all bosses should have customized scripts derived from this
public class BossController : MonoBehaviour
{
	protected const float _ZHEIGHT = -1f;			// fixed Z height of this enemy
	
	protected Vector2 _movement;
	
	protected GameManager _gm;
	protected Rigidbody2D _rb;
	protected SpriteRenderer _rend;
	protected CameraScript _cam;
	protected Transform _playerTransform;
	protected HUDController _hud;
	
	public int _scoreValue;
	public int _health;
	public int _maxHealth;
	
	private bool _fillingHpBar = false;
	private float _timeToFillBar = 3f;
	private float _curTimeToFillBar = 0;
	
	/* *********************** */
	
	void Awake ()
	{
		_gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
		_cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraScript>();
		_hud = GameObject.FindWithTag("HUD").GetComponent<HUDController>();
		_rb = GetComponent<Rigidbody2D>();
		_rend = GetComponent<SpriteRenderer>();
		
		_cam.AddToStaticList(this.gameObject);
		_hud.TurnOnBossBar("Kardinal");
		_fillingHpBar = true;
	}
	
	protected void Start()
	{
		_health = _maxHealth;
	}
	
	protected void Update()
	{
		if(_fillingHpBar)
		{
			_curTimeToFillBar += Time.deltaTime;
			SetHealthBar(_curTimeToFillBar / _timeToFillBar);
			if(_curTimeToFillBar >= _timeToFillBar)
				_fillingHpBar = false;
		}
	}
	
	/* *********************** */
	
	protected void SetHealthBar (float health)
	{
		_hud.SetHealthBar(health);
	}
	
	protected void SignalLevelEnd ()
	{
		_gm.TriggerLevelEnd();
	}
}
