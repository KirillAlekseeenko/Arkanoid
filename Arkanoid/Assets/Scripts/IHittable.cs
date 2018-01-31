using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable {
	int RewardPoints{ get; }
	Vector3 Position{ get; }
	void Hit();
}
