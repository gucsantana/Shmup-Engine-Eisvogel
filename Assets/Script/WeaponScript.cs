using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{	
	private WeaponClass _curWeapon;
	private float _cooldown;
	private bool _canAttack;
	public List<GameObject> _shotSrc = new List<GameObject>();		// list of anchor objects (hidden in the player prefab) from which the shots will be spawned
	
	public WeaponClass GetWeapon()
	{
		return _curWeapon;
	}
	
	public bool CanAttack()
	{
		return _canAttack;
	}
	
	void Awake()
	{
		// setting up the standard Basic weapon
		if (_curWeapon == null)
		{
			_curWeapon = new WeaponClass(
				name: "Basic",
				level: 1,
				power: 1,
				speed: 9f,
				rate: new List<float> {0.15f, 0.12f, 0.09f},
				positions: new List<string[]> { new string[] {"BasicSrcA"}, new string[] {"BasicSrcB","BasicSrcC"}, new string[] {"BasicSrcA", "BasicSrcB", "BasicSrcC"} }
			);
			
			UpdateShotSources();
		}
	}
	
	void Update()
	{
		if(_cooldown > 0f)
			_cooldown -= Time.deltaTime;
		if(_cooldown <= 0f && !_canAttack)
			_canAttack = true;
	}
	
	// called to increase the level of the current weapon by 1 and update the shot sources as necessary
	public void LevelUpWeapon()
	{
		_curWeapon._curLevel += 1;
		UpdateShotSources();
	}
	
	// called to update the shot sources (the anchors from which the shots are spawned)
	public void UpdateShotSources()
	{
		string[] src = _curWeapon._firePos[_curWeapon._curLevel - 1];
		List<GameObject> newSrc = new List<GameObject>();
		foreach (string srcName in src)
		{
			GameObject obj = GameObject.Find(srcName);
			newSrc.Add(obj);
		}
		this._shotSrc = newSrc;
	}
	
	// called to fire one round of shots from each shot source (anchor)
	public void Attack ()
	{
		_canAttack = false;
		GameObject _shotPrefab = Resources.Load<GameObject>("Shot Prefabs/" + _curWeapon._wpnName);
		foreach (GameObject src in _shotSrc)
		{
			GameObject bullet = Instantiate( _shotPrefab, src.transform.position, src.transform.rotation);
			bullet.GetComponent<ShotScript>().SetParameters(spd: _curWeapon._shotSpd, dmg: _curWeapon._attackPwr, dir: new Vector2(0, 1));
		}
		_cooldown = _curWeapon._fireRate[_curWeapon._curLevel-1];
	}
}
