using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEnemy : SpecialEnemy {

	[SerializeField] private Rigidbody2D splinterPrefab;
	[SerializeField] private int splinterCount;

	#region implemented abstract members of SpecialEnemy

	protected override void specialAction ()
	{
		spawnSplinters ();
	}

	#endregion 

	#region splinters

	private void spawnSplinters()
	{
		for (int i = 0; i < splinterCount; i++) {
			Instantiate (splinterPrefab.gameObject, transform);
		}
	}

	#endregion

}
