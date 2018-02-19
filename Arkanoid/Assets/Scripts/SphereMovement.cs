using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : MonoBehaviour {

	[SerializeField] private float velocityMagnitude;

	private Rigidbody2D rigidbodyComponent;

	[SerializeField] private float minimumY;

	Vector2? normal;
	Collider2D blockHit;

	public float VelocityMagnitude {
		get {
			return velocityMagnitude; 
		} 
		set { 
			rigidbodyComponent.velocity *= (value / velocityMagnitude);
			velocityMagnitude = value;
		} 
	}

	#region MonoBehaviour

	void OnEnable()
	{
		GameManager.GameStartedEvent += FirstKick;
	}

	void OnDisable()
	{
		GameManager.GameStartedEvent -= FirstKick;
	}

	void Start () {
		rigidbodyComponent = GetComponent<Rigidbody2D> ();
		StartCoroutine (movement ());
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Wall")) {
			AudioManager.Instance.PlayOnWallHitEffect ();
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		var hittable = other.gameObject.GetComponent<IHittable> ();
		if (hittable != null) {
			if (hittable is Enemy) {
				hittable.Hit ();
			} 
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

	public void Stop()
	{
		rigidbodyComponent.velocity = Vector2.zero;
	}

	private IEnumerator movement()
	{
		while (true) {
			yield return new WaitForFixedUpdate ();
			float rayLength = rigidbodyComponent.velocity.magnitude * Time.fixedDeltaTime;
			var origin = (Vector2)transform.position + rigidbodyComponent.velocity.normalized * GetComponent<CircleCollider2D> ().radius;
			RaycastHit2D centerHit;
			centerHit = Physics2D.Raycast (origin, rigidbodyComponent.velocity.normalized, rayLength, LayerMask.GetMask ("Block"));
			if (centerHit.collider != null) {
				changeDirection (centerHit.normal);
				var vectorToNormal = centerHit.point - origin;
				var newPositionVector = vectorToNormal + Vector2.Reflect (vectorToNormal.normalized, -centerHit.normal) * (rayLength - vectorToNormal.magnitude);
				rigidbodyComponent.MovePosition (rigidbodyComponent.position + newPositionVector);
				centerHit.collider.GetComponent<Block> ().Hit ();
			}
		}
	}

	private Vector2 getStraightNormal(Vector2 normal)
	{
		if (Mathf.Abs (normal.x) > Mathf.Abs (normal.y)) {
			return new Vector2 (Mathf.Sign (normal.x) * 1.0f, 0);
		} else
			return new Vector2 (0, Mathf.Sign (normal.y) * 1.0f);
	}

	private void changeDirection(Vector2 outNormal)
	{
		rigidbodyComponent.velocity = Vector2.Reflect (rigidbodyComponent.velocity, getStraightNormal(-outNormal));
	}
}


