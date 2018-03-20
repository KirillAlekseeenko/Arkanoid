using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : PlatformBonusComponent {

	private const float pauseTime = 0.2f;

	private bool isKickPossible = true;

	LinkedList<StickySphereInfo> spheresOnThePlatform;
		
	#region MonoBehaviour

	void Awake()
	{
		spheresOnThePlatform = new LinkedList<StickySphereInfo> ();
	}

	void OnEnable()
	{
		SpheresManager.SphereSpawned += onSphereSpawn;
	}

	void OnDisable()
	{
		SpheresManager.SphereSpawned -= onSphereSpawn;
	}

	void LateUpdate()
	{
		foreach (var sphereInfo in spheresOnThePlatform) {
			sphereInfo.sphere.transform.position = transform.position + sphereInfo.positionRelativeToThePlatform;
		}
	}

	#endregion
	
	#region implemented abstract members of PlatformBonusComponent

	public override void RemoveBonus ()
	{
		foreach (var sphereInfo in spheresOnThePlatform) {
			sphereInfo.sphere.Kick (sphereInfo.direction);
		}
		Destroy (this);
	}

	public override void SpecialAction ()
	{
		if (spheresOnThePlatform.Count > 0 && isKickPossible) {
			AudioManager.Instance.PlayOnPlatformHitEffect ();
			kickOneSphere ();
			StartCoroutine (reload ());
		}
	}

	#endregion

	public void AddSphere(SphereMovement sphere, Vector2 direction)
	{
		stabilizeSphere (sphere);
		var relativePosition = sphere.transform.position - transform.position;
		spheresOnThePlatform.AddFirst(new StickySphereInfo (sphere, direction, relativePosition));
	}

	private void stabilizeSphere(SphereMovement sphere)
	{
		var halfLength = GetComponent<Platform> ().HalfLength;
		var xRelativePos = Mathf.Clamp(sphere.transform.position.x - transform.position.x, -halfLength, halfLength);
		var yRelativePos = GetComponent<CapsuleCollider2D> ().size.y / 2 + sphere.GetComponent<CircleCollider2D>().radius;
		sphere.transform.position = transform.position + new Vector3 (xRelativePos, yRelativePos);
	}

	private void kickOneSphere()
	{
		var sphereInfo = spheresOnThePlatform.First.Value;
		spheresOnThePlatform.RemoveFirst ();
		sphereInfo.sphere.CanInteractWithPlatform = false;
		sphereInfo.sphere.Kick (sphereInfo.direction);
	}

	private IEnumerator reload()
	{
		isKickPossible = false;
		yield return new WaitForSeconds (pauseTime);
		isKickPossible = true;
	}

	private void onSphereSpawn (SphereMovement sphere)
	{
		if (spheresOnThePlatform.Count > 0) {
			sphere.CanInteractWithPlatform = false;
			AddSphere (sphere, spheresOnThePlatform.First.Value.direction);
		}
	}


	private class StickySphereInfo
	{
		public SphereMovement sphere;
		public Vector2 direction;
		public Vector3 positionRelativeToThePlatform;
		public StickySphereInfo(SphereMovement _sphere, Vector2 _direction, Vector3 _positionRelativeToThePlatform)
		{
			sphere = _sphere;
			direction = _direction;
			positionRelativeToThePlatform = _positionRelativeToThePlatform;
		}
	}
}


