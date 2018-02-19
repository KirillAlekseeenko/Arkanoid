using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemy : SpecialEnemy {

	[SerializeField] private float forceSpeedMultiplier;
	[SerializeField] private float turnSpeed; // angle in degrees per second

	private const float coroutineFrameLength = 0.2f;

	private Vector3 desiredDirection;
	private Transform platformTransform;

	#region MonoBehaviour

	private new void Start()
	{
		platformTransform = GameObject.FindWithTag ("Platform").transform;
		base.Start ();
	}

	private new void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D (other);
		if (other.gameObject.layer == LayerMask.NameToLayer("Wall")) {
			Destroy (gameObject);
		}
	}

	#endregion

	#region implemented abstract members of SpecialEnemy

	protected override void specialAction ()
	{
		turnForceOn ();
	}

	#endregion

	#region force movement

	private void turnForceOn()
	{
		direction = (platformTransform.position - transform.position).normalized;
		speed *= forceSpeedMultiplier;
		StartCoroutine (forceMovement ());
	}

	private IEnumerator forceMovement()
	{
		while (true) {
			turn ();
			yield return new WaitForSeconds (coroutineFrameLength);
		}
	}

	private void turn()
	{
		desiredDirection = (platformTransform.position - transform.position).normalized;
		var angle = turnSpeed * Mathf.Deg2Rad * coroutineFrameLength;
		direction = Vector3.RotateTowards (direction, desiredDirection, angle, 1.0f);
	}

	#endregion


}
