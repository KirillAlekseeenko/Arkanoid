using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleLength : PlatformBonusComponent {

	private const float scaleModifier = 1.5f;
	
	#region implemented abstract members of PlatformBonusComponent

	public override void RemoveBonus ()
	{
		scale (1 / scaleModifier);
		Destroy (this);
	}

	#endregion

	void Start () {
		scale (scaleModifier);
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
		var size = GetComponent<CapsuleCollider2D> ().size;
		GetComponent<CapsuleCollider2D> ().size = new Vector2 (size.x * multiplier, size.y);
	}
}
