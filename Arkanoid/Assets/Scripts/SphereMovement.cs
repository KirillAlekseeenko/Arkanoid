using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : MonoBehaviour {

	[SerializeField] private float _velocityMagnitude;

	private Rigidbody2D rigidbodyComponent;

	[SerializeField] private float minimumY;

	Vector2? normal;
	Collider2D blockHit;

	private bool _stopped = false;

	private bool stopped
	{
		get {
			return _stopped; 
		}
		set {
			if (value == _stopped)
				return;
			if (value) {
				velocityMagnitude *= 0.01f;
			} else {
				velocityMagnitude *= 100f;
			}
			_stopped = value;
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
			if (stopped)
				return;
			velocityMagnitude = value;
		} 
	}

	#region MonoBehaviour

	void Awake()
	{
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
		float stoppedRayLength = velocityMagnitude;
		
		while (true) {
			yield return new WaitForFixedUpdate ();
			float rayLength;
			if (stopped)
				rayLength = stoppedRayLength;
			else
				rayLength = velocityMagnitude * Time.fixedDeltaTime;
			var origin = rigidbodyComponent.position;
			RaycastHit2D centerHit;
			centerHit = Physics2D.Raycast (origin, rigidbodyComponent.velocity.normalized, rayLength, LayerMask.GetMask ("Block"));
			if (centerHit.collider != null) {
				stoppedRayLength = rayLength;
				stopped = true;
				changeDirection (centerHit.normal);
				rigidbodyComponent.MovePosition (rigidbodyComponent.position + (centerHit.point - rigidbodyComponent.position) * 0.7f);
				centerHit.collider.GetComponent<Block> ().Hit ();
			} else
				stopped = false;
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


