using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	[SerializeField] private float speed;

	#region MonoBehaviour

	void OnEnable()
	{
		GameManager.LevelCleanedEvent += onCleanLevel;
	}

	void OnDisable()
	{
		GameManager.LevelCleanedEvent -= onCleanLevel;
	}

	void Update () {
		transform.Translate (Vector3.up * speed * Time.deltaTime);
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

	private void onCleanLevel()
	{
		Destroy (gameObject);
	}
}
