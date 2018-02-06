﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	[SerializeField] private float speed;
	private float xPos;

	#region MonoBehaviour

	void Start()
	{
		xPos = transform.position.x;
	}

	void Update () {
		transform.Translate (Vector3.up * speed * Time.deltaTime);
		transform.position = new Vector3 (xPos, transform.position.y);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		var hittable = other.GetComponent<IHittable> ();
		if (hittable != null) {
			hittable.Hit ();
		}
		Destroy (gameObject);
	}

	#endregion
}
