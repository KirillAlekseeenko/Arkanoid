using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Sphere")) {
			GameManager.Instance.CheckLostConditions ();
		}
		Destroy (other.gameObject);
	}
}
