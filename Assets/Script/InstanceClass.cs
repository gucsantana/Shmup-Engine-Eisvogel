using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Base class for enemy instances; all enemies and bosses should be derived from this
public class InstanceClass : MonoBehaviour
{
	public float _timing;					// timing for spawn in level; it's spawned when the camera's current distance is equal or greater than this timing value
}
