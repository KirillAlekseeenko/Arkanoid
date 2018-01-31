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
		foreach (var point in GetComponent<EdgeCollider2D>().points) {
			point.Set (point.x * multiplier, point.y);
		}
	}
}
