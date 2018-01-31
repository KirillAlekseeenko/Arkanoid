using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialEnemy : MovingEnemy {

	private const float gameFieldWidth = 6.0f;
	private const float yFloorPosition = -5.0f;

	#region MonoBehaviour

	protected new void Start()
	{
		base.Start ();
		StartCoroutine (checkBlocksPosition ());
	}

	#endregion

	#region special movement

	private IEnumerator checkBlocksPosition()
	{
		while (canSee) {
			if (!isThereBlocksLower()) {
				canSee = false;
				specialAction ();
			}
			yield return new WaitForSeconds (1.0f);
		}
	}

	private bool isThereBlocksLower()
	{
		var yMiddlePoint = (transform.position.y + yFloorPosition) / 2;
		var center = new Vector2 (0, yMiddlePoint);
		var size = new Vector2 (gameFieldWidth, Mathf.Abs(transform.position.y - yFloorPosition));
		return Physics2D.OverlapBox (center, size, 0, LayerMask.GetMask ("Block"));
	}

	protected abstract void specialAction ();

	#endregion
}
