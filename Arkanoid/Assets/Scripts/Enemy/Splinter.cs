using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splinter : Enemy {

	private const float maxSplinterAngle = (60.0f * Mathf.Deg2Rad);

	void Start()
	{
		GetComponent<Rigidbody2D> ().velocity = getRandomSplinterVelocity () * speed;
	}
	
	private Vector2 getRandomSplinterVelocity()
	{
		var angle = Random.Range (-maxSplinterAngle, maxSplinterAngle);
		return MathUtils.RotateVector (Vector2.down, angle);
	}
}
