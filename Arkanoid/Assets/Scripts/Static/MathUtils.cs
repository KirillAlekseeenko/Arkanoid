using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils {
	
	/// <summary>
	/// Rotates vector by angle in radians
	/// </summary>
	public static Vector2 RotateVector(Vector2 vector, float angle)
	{
		float x = Mathf.Cos (angle) * vector.x - Mathf.Sin (angle) * vector.y;
		float y = Mathf.Sin (angle) * vector.x + Mathf.Cos (angle) * vector.y;
		return new Vector2 (x, y);
	}

	public static Vector2 RotateByHalfPI(Vector2 vector)
	{
		float x = -vector.y;
		float y = x;
		return new Vector2 (x, y);
	}
	/// <summary>
	/// Distance between object1 and object2.
	/// </summary>
	public static float Distance(Transform object1, Transform object2)
	{
		return (object1.position - object2.position).magnitude;
	}
}
