using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Script attached to the main Camera object, that regulates general camera features and movement
public class CameraScript : MonoBehaviour
{
	private Vector2 _camDirection = new Vector2(0,1);
	private Vector2 _camSpeed = new Vector2(0,1);
	public bool _isMoving = true;
	
	// references to the player and to every 'static' enemy (a.k.a enemies that follow the camera movement)
	private GameObject _player;
	private HashSet<GameObject> _staticEnemyList = new HashSet<GameObject>();
	
	private float _levelLength = 100;
	public float _curDistance = 0;
	
	public float GetCurDistance ()
	{
		return _curDistance;
	}
	
	public void SetLevelLength(int len)
	{
		_levelLength = len;
	}

	/* ********************************** */
	void FixedUpdate()
	{
		if(_player == null)
			_player = GameObject.FindWithTag("Player");
		
		// apply movement to the camera upwards as long as we haven't reached the end of the level
		if(_isMoving && _curDistance < _levelLength)
		{
			Vector3 movement = new Vector3(
				_camDirection.x * _camSpeed.x,
				_camDirection.y * _camSpeed.y,
				0);

			movement *= Time.deltaTime;
			transform.Translate(movement);
			
			// apply camera movement to the player (provided he's currently alive)
			if(_player)
				_player.transform.Translate(movement);
			
			// remove destroyed enemies from the static enemy list, and then apply movement to the remainder
			_staticEnemyList.RemoveWhere(x => x == null);
			foreach(GameObject obj in _staticEnemyList)
				obj.transform.Translate(movement);
			
			_curDistance += Time.deltaTime * _camSpeed.y;
		}
	}
	
	public void AddToStaticList(GameObject enemy)
	{
		_staticEnemyList.Add(enemy);
	}
}
