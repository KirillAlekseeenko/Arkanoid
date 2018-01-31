using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	[SerializeField] protected float speed;

	protected void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Platform")) {
			GameManager.Instance.Lost ();
		}
	}
}
