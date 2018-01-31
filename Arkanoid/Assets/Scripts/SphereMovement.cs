using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : MonoBehaviour {

	[SerializeField] private float velocityMagnitude;

	private Rigidbody2D rigidbodyComponent;

	[SerializeField] private float minimumY;
	private float xToMinimumY;

	public float VelocityMagnitude {
		get {
			return velocityMagnitude;
		}
		set{
			velocityMagnitude = value;
		}
	}

	#region MonoBehaviour

	void Start () {
		rigidbodyComponent = GetComponent<Rigidbody2D> ();
		xToMinimumY = Mathf.Sqrt (1 - minimumY * minimumY);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		var hittable = other.gameObject.GetComponent<IHittable> ();
		if (hittable != null) {
			hittable.Hit();
			correctCourse ();
		}
	}

	#endregion

	public void FirstKick()
	{
		var randVariable = Random.Range (0, 2);
		if (randVariable == 0) {
			rigidbodyComponent.velocity = new Vector2 (1, 1).normalized * velocityMagnitude; 
		} else {
			rigidbodyComponent.velocity = new Vector2 (-1, 1).normalized * velocityMagnitude; 
		}
	}

	public void Kick(Vector2 direction)
	{
		rigidbodyComponent.velocity = direction.normalized * velocityMagnitude;
	}

	private void correctCourse()
	{
		var velocity = rigidbodyComponent.velocity.normalized;

		if (Mathf.Abs(velocity.y) < minimumY) {
			rigidbodyComponent.velocity = new Vector2 (Mathf.Sign(velocity.x) * xToMinimumY, Mathf.Sign(velocity.y) * minimumY) * velocityMagnitude;
		}
	}

	public void Stop()
	{
		rigidbodyComponent.velocity = Vector2.zero;
	}

}


