using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleLength : PlatformBonusComponent {
	
	#region implemented abstract members of PlatformBonusComponent

	public override void RemoveBonus ()
	{
		scale (0.5f);
		Destroy (this);
	}

	#endregion

	void Start () {
		scale (2.0f);
	}

	private void scale(float multiplier)
	{
		scaleSprite (multiplier);
		scaleCollider (multiplier);
	}

	private void scaleSprite(float multiplier)
	{
		var sz = GetComponent<SpriteRenderer> ().size;
		GetComponent<SpriteRenderer> ().size = new Vector2 (sz.x * multiplier, sz.y);
	}
	private void scaleCollider(float multiplier)
	{
		var points = GetComponent<EdgeCollider2D> ().points;
		var newPoints = new Vector2[points.Length];
		for (int i = 0; i < points.Length; i++) {
			newPoints [i] = new Vector2 (points [i].x * multiplier, points [i].y);
		}
		GetComponent<EdgeCollider2D> ().points = newPoints;
	}
}
