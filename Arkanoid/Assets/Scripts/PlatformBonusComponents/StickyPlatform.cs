﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : PlatformBonusComponent {

	private const float pauseTime = 1.0f;

	private bool isKickPossible = true;

	LinkedList<StickySphereInfo> spheresOnThePlatform;
		
	#region MonoBehaviour

	void Awake()
	{
		spheresOnThePlatform = new LinkedList<StickySphereInfo> ();
	}

	void Update()
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
		
	#endregion

	#region implemented virtual members of PlatformBonusComponent

	public override void SpecialAction ()
	{
		if (spheresOnThePlatform.Count > 0 && isKickPossible) {
			kickOneSphere ();
			StartCoroutine (reload ());
		}
	}

	#endregion

	public void AddSphere(SphereMovement sphere, Vector2 direction)
	{
		var relativePosition = sphere.transform.position - transform.position;
		spheresOnThePlatform.AddFirst(new StickySphereInfo (sphere, direction, relativePosition));
	}

	private void kickOneSphere()
	{
		var sphereInfo = spheresOnThePlatform.First.Value;
		spheresOnThePlatform.RemoveFirst ();
		sphereInfo.sphere.Kick (sphereInfo.direction);
	}

	private IEnumerator reload()
	{
		isKickPossible = false;
		yield return new WaitForSeconds (pauseTime);
		isKickPossible = true;
	}

	private struct StickySphereInfo
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

