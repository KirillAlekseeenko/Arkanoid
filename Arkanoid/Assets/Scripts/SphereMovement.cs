using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : MonoBehaviour {

	[SerializeField] private float velocityMagnitude;

	private Rigidbody2D rigidbodyComponent;

	[SerializeField] private float minimumY;
	private float xToMinimumY;

	private bool isColliding = false;
	private Vector2 firstCollidingBlockPosition;

	ContactFilter2D contactFilter;
	ContactPoint2D[] contactPoints;

	private List<Block> blocksInContact;

	Vector2? normal;
	Collider2D blockHit;

	private bool isOnWall = false;


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

	void Start () {
		contactFilter.SetLayerMask(LayerMask.GetMask("Block"));
		contactPoints = new ContactPoint2D[2];
		rigidbodyComponent = GetComponent<Rigidbody2D> ();
		xToMinimumY = Mathf.Sqrt (1 - minimumY * minimumY);
	}

	// debug
	private	IEnumerator smooth()
	{
		isSmooth = true;
		Time.timeScale = 0.1f;
		yield return new WaitForSecondsRealtime (2.0f);
		Time.timeScale = 1.0f;
		isSmooth = false;
	}

	bool isSmooth = false;


	void FixedUpdate()
	{
		if (normal.HasValue && blockHit != null) {
			changeDirection (normal.Value);
			blockHit.GetComponent<Block> ().Hit ();
			normal = null;
			blockHit = null;
		}
		RaycastHit2D hit;
		float rayLength = GetComponent<Rigidbody2D> ().velocity.magnitude * Time.fixedDeltaTime * 3;
		hit = Physics2D.Raycast (transform.position, GetComponent<Rigidbody2D> ().velocity.normalized, rayLength, LayerMask.GetMask ("Block"));
		if (hit != null) {
			normal = hit.normal;
			blockHit = hit.collider;
		}
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

	private void bounceSimulation(ContactPoint2D point)
	{
		var other = point.collider;
		var block = other.gameObject.GetComponent<Block> ();
		if (block != null) {
			if (isColliding) {
				Debug.Log ("second");
				if (block.isBlockNextTo (firstCollidingBlockPosition)) {
					Debug.Log ("nextTo");
					return;
				}
			} else {
				Debug.Log ("first");
				block.Hit ();
				firstCollidingBlockPosition = block.transform.position;
				isColliding = true;
			}
		}
		changeDirection (point.normal);
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


