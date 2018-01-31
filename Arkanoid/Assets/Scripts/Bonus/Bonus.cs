﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bonus : MonoBehaviour {

	[SerializeField] protected float speed;

	protected void Update () {
		transform.Translate (Vector3.down * speed * Time.deltaTime);
	}

	public abstract void GetBonus (Platform platform);
}