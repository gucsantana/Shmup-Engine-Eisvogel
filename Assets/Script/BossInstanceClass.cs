using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInstanceClass
{
	public float _timing;
	public string _enemyType;
	public Vector2 _spawnPoint;	
	
	public BossInstanceClass (float timing, string enemyType, Vector2 spawnPoint)
	{
		this._timing = timing;
		this._enemyType = enemyType;
		this._spawnPoint = spawnPoint;
	}
}
