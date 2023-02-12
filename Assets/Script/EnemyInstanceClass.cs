using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

/// An instantiable object that describes an instance of an enemy in a level, along with all of its relevant parameters
public class EnemyInstanceClass
{
	public float _timing;					// timing for spawn in level; it's spawned when the camera's current distance is equal or greater than this timing value
	
	public string _enemyType;				// type of enemy (defines prefab to be called)
	public int _scoreValue;					// score given when defeated
	public int _health;						// enemy health
	public bool _isStatic;					// static: follows camera movement
	
	public int _movePattern = 1;				// the pattern in which the enemy moves on the screen
	public Vector2 _spawnPoint;				// (relative) screen point in which this enemy is spawned
	public Vector2 _moveParam;				// base movement direction
	public List<Vector2> _movePoints;		// movement points used in pattern 2
	public Vector2 _moveMod;				// movement direction modifier, used in pattern 3
	public float _moveSpeed = 1f;			// speed at which this enemy moves
	public float _pointDampenFactor = 0.5f;	// dampens the speed of movement by this value when close to the next point
	
	public int _shotPattern = 0;				// the pattern in which the enemy fires
	public float _firstShotDelay = 0f;			// delay before the first shot
	public float _shotDelay = 0f;				// delay between shots (irrelevant if it doesn't fire)
	public int _shotsMax = 0;				// how many shots it fires in total
	public float _shotSpeed = 3;				// the speed of the bullets fired by this enemy
	public Vector2 _shotDirection;			// the angle this enemy shoots at, if pattern 1
	
	public string _itemDrop = "";				// the name of the item this item drops, if any
	
	public EnemyInstanceClass (float timing, string enemyType, int score, int health, bool isStatic, int movePat, float moveSpeed, Vector2 spawnPt, Vector2 moveParam, List<Vector2> movePoints, Vector2 moveMod, float dampenFactor, 
		int shotPattern, float firstShotDelay, float shotDelay, int shotsMax, float shotSpeed, Vector2 shotDirection, string itemDrop)
	{
		this._timing = timing;
		
		this._enemyType = enemyType;
		this._scoreValue = score;
		this._health = health;
		this._isStatic = isStatic;
		
		this._movePattern = movePat;
		this._moveSpeed = moveSpeed;
		this._spawnPoint = spawnPt;
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
}
