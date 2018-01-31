using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
	
	/// <summary>
	/// Rotates vector by angle in radians
	/// </summary>
	public static Vector2 RotateVector(Vector2 vector, float angle)
	{
		float x = Mathf.Cos (angle) * vector.x - Mathf.Sin (angle) * vector.y;
		float y = Mathf.Sin (angle) * vector.x + Mathf.Cos (angle) * vector.y;
		return new Vector2 (x, y);
	}
}
