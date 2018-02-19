using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

	public delegate void TeleportEvent ();
	public static event TeleportEvent OnPortalEnter;

	void OnEnable()
	{
		GameManager.LevelCleanedEvent += deactivate;
		PurpleBonus.PurpleBonusAcquired += activate;
	}

	void OnDisable()
	{
		GameManager.LevelCleanedEvent -= deactivate;
		PurpleBonus.PurpleBonusAcquired -= activate;
	}

	void Start()
	{
		deactivate ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Platform")) {
			OnPortalEnter ();
		}
	}

	private void deactivate()
	{
		GetComponent<Renderer> ().enabled = false;
		GetComponent<Collider2D> ().enabled = false;
	}

	private void activate()
	{
		GetComponent<Renderer> ().enabled = true;
		GetComponent<Collider2D> ().enabled = true;
	}
}
