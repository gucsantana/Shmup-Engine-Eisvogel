using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponClass
{
	public string _wpnName;		// name of the weapon - doubles as the name of the shot prefab
	public int _curLevel = 1;			// current weapon level, decides fire rate and where the shots come from
	public int _attackPwr;			// attack power of each shot
	public float _shotSpd;			// how fast the shot travels on the screen
	public List<float> _fireRate;		// list of fire rate (cooldown) speeds, one for each level
	public List<string[]> _firePos;	// list of strings denoting the anchor objects (hidden on the player prefab) from which the shots will be generated
	
	public WeaponClass(string name, int level, int power, float speed, List<float> rate, List<string[]> positions)
	{
		this._wpnName = name;
		this._curLevel = level;
		this._attackPwr = power;
		this._shotSpd = speed;
		this._fireRate = rate;
		this._firePos = positions;
	}
}