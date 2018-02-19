using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour {

	public delegate void SomethingLost ();
	public static event SomethingLost SphereLost;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Sphere")) {
			SphereLost ();
		}
		Destroy (other.gameObject);
	}
}
