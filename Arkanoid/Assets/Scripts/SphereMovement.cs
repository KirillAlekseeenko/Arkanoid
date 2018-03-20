using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : MonoBehaviour {

	[SerializeField] private float _velocityMagnitude;

	private Rigidbody2D rigidbodyComponent;

	Vector2? normal;
	Collider2D blockHit;

	private bool stopped = false;

	private bool afterWallCollision = false;
	private ContactPoint2D[] wallContactPoint;

	private bool canInteractWithPlatform = true;

	public bool Stopped
	{
		get {
			return stopped; 
		}
		private set {
			if (value == stopped)
				return;
			if (value) {
				velocityMagnitude *= 0.01f;
			} else {
				velocityMagnitude *= 100f;
			}
			stopped = value;
		}
	}

	private float velocityMagnitude
	{
		get {
			return _velocityMagnitude;
		}
		set{
			rigidbodyComponent.velocity *= (value / _velocityMagnitude);
			_velocityMagnitude = value;
		}
	}

	public float VelocityMagnitude {
		get {
			return velocityMagnitude; 
		} 
		set {
			if (Stopped)
				return;
			velocityMagnitude = value;
		} 
	}

	public bool CanInteractWithPlatform {
		get {
			return canInteractWithPlatform;
		}
		set {
			canInteractWithPlatform = value;
			if (!value) {
				StartCoroutine(TurnOnInteractionWithPlatform (0.2f));
			}
		}
	}

	#region MonoBehaviour

	void Awake()
	{
		CanInteractWithPlatform = true;
		wallContactPoint = new ContactPoint2D[1];
		rigidbodyComponent = GetComponent<Rigidbody2D> ();
	}

	void OnEnable()
	{
		GameManager.GameStartedEvent += FirstKick;
	}

	void OnDisable()
	{
		GameManager.GameStartedEvent -= FirstKick;
	}

	void Start () 
	{
		StartCoroutine (movement ());
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Wall")) {
			AudioManager.Instance.PlayOnWallHitEffect ();
			afterWallCollision = true;
			other.GetContacts (wallContactPoint);
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
		var randomAngle = Random.Range (-45, 45);
		var velocity = MathUtils.RotateVector (Vector2.up, randomAngle * Mathf.Deg2Rad);

		rigidbodyComponent.velocity = velocity * velocityMagnitude;
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
		float stoppedRayLength = velocityMagnitude;
		
		while (true) {
			yield return new WaitForFixedUpdate ();
			float rayLength;
			if (Stopped)
				rayLength = stoppedRayLength;
			else
				rayLength = velocityMagnitude * Time.fixedDeltaTime;
			
			if (afterWallCollision) {
				afterWallCollision = false;
				var hit = Physics2D.Raycast (wallContactPoint[0].point, rigidbodyComponent.velocity.normalized, rayLength, LayerMask.GetMask ("Block"));
				if (hit.collider != null && hit.collider.OverlapPoint (rigidbodyComponent.position)) {
					changeDirection (hit.normal);
					hit.collider.GetComponent<Block> ().Hit ();
					continue;
				}
			}

			var origin = rigidbodyComponent.position;
			RaycastHit2D centerHit;
			centerHit = Physics2D.Raycast (origin, rigidbodyComponent.velocity.normalized, rayLength, LayerMask.GetMask ("Block"));
			if (centerHit.collider != null) {
				stoppedRayLength = rayLength;
				Stopped = true;
				changeDirection (centerHit.normal);
				rigidbodyComponent.MovePosition (rigidbodyComponent.position + (centerHit.point - rigidbodyComponent.position) * 0.7f);
				centerHit.collider.GetComponent<Block> ().Hit ();
			} else
				Stopped = false;
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

	private IEnumerator TurnOnInteractionWithPlatform(float period)
	{
		yield return new WaitForSeconds (period);
		canInteractWithPlatform = true;
	}
}


