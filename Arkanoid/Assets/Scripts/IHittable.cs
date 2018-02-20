using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All objects that sphere can hit
/// </summary>
public interface IHittable {
	int RewardPoints{ get; }
	Vector3 Position{ get; }
	void Hit();
}
