using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

	public delegate void PlatformInteraction();
	public static event PlatformInteraction PlatformDestroyed;

	[SerializeField] protected float speed;

	protected void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Platform")) {
			PlatformDestroyed ();
		}
	}

	protected void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Platform")) {
			PlatformDestroyed ();
		}
	}
}
