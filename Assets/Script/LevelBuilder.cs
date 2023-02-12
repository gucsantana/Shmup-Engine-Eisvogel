using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Component that creates the queue of enemies to be spawned into the level
public class LevelBuilder : MonoBehaviour
{
	private List<Queue<EnemyInstanceClass>> _levelDeclaration = new List<Queue<EnemyInstanceClass>>();
	private List<BossInstanceClass> _levelBosses = new List<BossInstanceClass>();
	private bool _finishedLoading = false;
	
	public Queue<EnemyInstanceClass> GetEnemyQueue (int level)
	{
		return _levelDeclaration[level];
	}
	
	public BossInstanceClass GetLevelBoss (int level)
	{
		return _levelBosses[level];
	}
	
	public bool IsFinishedLoading()
	{
		return _finishedLoading;
	}
	
	public void Start()
	{
		StartCoroutine(BuildDatabase());
	}
	
	/// Builds database of enemies that will be spawned for every level, including all relevant parameters per enemy
	// TODO: move enemy parameters to a XML or JSON and read from there, for code cleanliness
	IEnumerator BuildDatabase ()
	{
		// LEVEL 1 -----------------
		Queue<EnemyInstanceClass> queue1 = new Queue<EnemyInstanceClass>();
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 3f, enemyType: "EnemySphere", score: 10, health: 2, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(-2.5f, 4.5f), moveParam: new Vector2(0, -1), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0, shotDelay: 0, shotsMax: 0, shotSpeed: 0, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 3f, enemyType: "EnemySphere", score: 10, health: 2, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(-1.5f, 4.5f), moveParam: new Vector2(0, -1), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0, shotDelay: 0, shotsMax: 0, shotSpeed: 0, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 3f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(1.5f, 4.5f), moveParam: new Vector2(0, -1), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0, shotDelay: 0, shotsMax: 0, shotSpeed: 0, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 3f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(2.5f, 4.5f), moveParam: new Vector2(0, -1), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0, shotDelay: 0, shotsMax: 0, shotSpeed: 0, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 4.5f, enemyType: "EnemySoldier", score: 10, health: 4, isStatic: true, itemDrop: "",
			movePat: 2, moveSpeed: 1f, spawnPt: new Vector2(0f, 4.5f), moveParam: new Vector2(0, -1), moveMod: new Vector2(0f, 0f),
			movePoints: new List<Vector2>{ new Vector2(0f, 4.5f), new Vector2(2f, 3f), new Vector2(-2f, 2f), new Vector2(2f, 1f) },
			dampenFactor: 0.5f, shotPattern: 2, firstShotDelay: 1f, shotDelay: 1f, shotsMax: 3, shotSpeed: 0.5f, shotDirection: new Vector2(0f, -1f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 7.5f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "PickupWpn",
			movePat: 3, moveSpeed: 1.5f, spawnPt: new Vector2(3f, 4.5f), moveParam: new Vector2(-0.5f, -1f), moveMod: new Vector2(-0.5f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0, shotDelay: 0, shotsMax: 0, shotSpeed: 0, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 7.5f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 3, moveSpeed: 1.5f, spawnPt: new Vector2(-3f, 4.5f), moveParam: new Vector2(0.5f, -1f), moveMod: new Vector2(0.5f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0, shotDelay: 0, shotsMax: 0, shotSpeed: 0, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 7.7f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 3, moveSpeed: 1.5f, spawnPt: new Vector2(3f, 4.5f), moveParam: new Vector2(-0.5f, -1f), moveMod: new Vector2(-0.5f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0, shotDelay: 0, shotsMax: 0, shotSpeed: 0, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 7.7f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 3, moveSpeed: 1.5f, spawnPt: new Vector2(-3f, 4.5f), moveParam: new Vector2(0.5f, -1f), moveMod: new Vector2(0.5f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0, shotDelay: 0, shotsMax: 0, shotSpeed: 0, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 7.9f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 3, moveSpeed: 1.5f, spawnPt: new Vector2(3f, 4.5f), moveParam: new Vector2(-0.5f, -1f), moveMod: new Vector2(-0.5f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0, shotDelay: 0, shotsMax: 0, shotSpeed: 0, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 7.9f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 3, moveSpeed: 1.5f, spawnPt: new Vector2(-3f, 4.5f), moveParam: new Vector2(0.5f, -1f), moveMod: new Vector2(0.5f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0, shotDelay: 0, shotsMax: 0, shotSpeed: 0, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 8.1f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 3, moveSpeed: 1.5f, spawnPt: new Vector2(3f, 4.5f), moveParam: new Vector2(-0.5f, -1f), moveMod: new Vector2(-0.5f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0, shotDelay: 0, shotsMax: 0, shotSpeed: 0, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 8.1f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 3, moveSpeed: 1.5f, spawnPt: new Vector2(-3f, 4.5f), moveParam: new Vector2(0.5f, -1f), moveMod: new Vector2(0.5f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0, shotDelay: 0, shotsMax: 0, shotSpeed: 0, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 8.3f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 3, moveSpeed: 1.5f, spawnPt: new Vector2(3f, 4.5f), moveParam: new Vector2(-0.5f, -1f), moveMod: new Vector2(-0.5f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 1, firstShotDelay: 1.5f, shotDelay: 0, shotsMax: 1, shotSpeed: 2f, shotDirection: new Vector2(0f, -1f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 8.3f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 3, moveSpeed: 1.5f, spawnPt: new Vector2(-3f, 4.5f), moveParam: new Vector2(0.5f, -1f), moveMod: new Vector2(0.5f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 1, firstShotDelay: 1.5f, shotDelay: 0, shotsMax: 1, shotSpeed: 2f, shotDirection: new Vector2(0f, -1f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 10f, enemyType: "EnemyTurret", score: 20, health: 12, isStatic: false, itemDrop: "",
			movePat: 0, moveSpeed: 0f, spawnPt: new Vector2(-2.5f, 4.5f), moveParam: new Vector2(0f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 2, firstShotDelay: 1.5f, shotDelay: 1.2f, shotsMax: 3, shotSpeed: 0.3f, shotDirection: new Vector2(0f, -1f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 10f, enemyType: "EnemyTurret", score: 20, health: 12, isStatic: false, itemDrop: "",
			movePat: 0, moveSpeed: 0f, spawnPt: new Vector2(2.5f, 4.5f), moveParam: new Vector2(0f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 2, firstShotDelay: 1.5f, shotDelay: 1.2f, shotsMax: 3, shotSpeed: 0.3f, shotDirection: new Vector2(0f, -1f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 12f, enemyType: "EnemyTurret", score: 20, health: 12, isStatic: false, itemDrop: "PickupWpn",
			movePat: 0, moveSpeed: 0f, spawnPt: new Vector2(-1.5f, 4.5f), moveParam: new Vector2(0f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 2, firstShotDelay: 1.5f, shotDelay: 1.2f, shotsMax: 3, shotSpeed: 0.3f, shotDirection: new Vector2(0f, -1f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 12f, enemyType: "EnemyTurret", score: 20, health: 12, isStatic: false, itemDrop: "",
			movePat: 0, moveSpeed: 0f, spawnPt: new Vector2(1.5f, 4.5f), moveParam: new Vector2(0f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 2, firstShotDelay: 1.5f, shotDelay: 1.2f, shotsMax: 3, shotSpeed: 0.3f, shotDirection: new Vector2(0f, -1f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 13.5f, enemyType: "EnemySoldier", score: 30, health: 10, isStatic: true, itemDrop: "",
			movePat: 4, moveSpeed: 3f, spawnPt: new Vector2(-2.5f, 4.5f), moveParam: new Vector2(0, -1), moveMod: new Vector2(-2.5f, 4.5f),
			movePoints: new List<Vector2>{ new Vector2(-2.5f, 4.5f), new Vector2(-2.5f, 3f)},
			dampenFactor: 0.5f, shotPattern: 2, firstShotDelay: 1.2f, shotDelay: 0.1f, shotsMax: 10, shotSpeed: 0.15f, shotDirection: new Vector2(0f, -1f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 13.5f, enemyType: "EnemySoldier", score: 30, health: 10, isStatic: true, itemDrop: "",
			movePat: 4, moveSpeed: 3f, spawnPt: new Vector2(2.5f, 4.5f), moveParam: new Vector2(0, -1), moveMod: new Vector2(2.5f, 4.5f),
			movePoints: new List<Vector2>{ new Vector2(2.5f, 4.5f), new Vector2(2.5f, 3f)},
			dampenFactor: 0.5f, shotPattern: 2, firstShotDelay: 1.2f, shotDelay: 0.1f, shotsMax: 10, shotSpeed: 0.15f, shotDirection: new Vector2(0f, -1f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 18f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(-4f, 3f), moveParam: new Vector2(1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 18f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(4f, 3f), moveParam: new Vector2(-1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 18.3f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(-4f, 3f), moveParam: new Vector2(1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 18.3f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(4f, 3f), moveParam: new Vector2(-1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 18.6f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(-4f, 3f), moveParam: new Vector2(1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 18.6f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(4f, 3f), moveParam: new Vector2(-1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 18.9f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(-4f, 3f), moveParam: new Vector2(1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 18.9f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(4f, 3f), moveParam: new Vector2(-1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 19.2f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(-4f, 3f), moveParam: new Vector2(1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 19.2f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(4f, 3f), moveParam: new Vector2(-1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 19.5f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(-4f, 3f), moveParam: new Vector2(1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 19.5f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "PickupWpn",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(4f, 3f), moveParam: new Vector2(-1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 19.8f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(-4f, 3f), moveParam: new Vector2(1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
		queue1.Enqueue( new EnemyInstanceClass(
			timing: 19.8f, enemyType: "EnemySphere", score: 10, health: 1, isStatic: true, itemDrop: "",
			movePat: 1, moveSpeed: 1.5f, spawnPt: new Vector2(4f, 3f), moveParam: new Vector2(-1f, 0f), moveMod: new Vector2(0f, 0f),
			movePoints: null,
			dampenFactor: 0.5f, shotPattern: 0, firstShotDelay: 0f, shotDelay: 0, shotsMax: 0, shotSpeed: 0f, shotDirection: new Vector2(0f, 0f) ) );
			
		_levelDeclaration.Add(queue1);
		
		_levelBosses.Add( new BossInstanceClass( timing: 25f, enemyType: "kardinal", spawnPoint: new Vector2(0f, 5.5f) ) );
		
		Debug.Log("Finished loading the level builder.");
		_finishedLoading = true;
		yield return true;
	}
}
