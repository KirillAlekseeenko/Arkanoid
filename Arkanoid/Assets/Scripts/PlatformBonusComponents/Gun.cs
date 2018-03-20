using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : PlatformBonusComponent {

	private Bullet bulletPrefab;
	private float reloadTime;
	private float timeToRemove;

	private Coroutine bulletSpawnerCoroutine;

	void Start () {
		bulletPrefab = GetComponent<Platform> ().BulletPrefab;
		reloadTime = GetComponent<Platform> ().ReloadTime;
		timeToRemove = GetComponent<Platform> ().TimeToRemove;
		bulletSpawnerCoroutine = StartCoroutine (bulletSpawn ());
	}

	private void spawnBullet(Vector3 place)
	{
		Instantiate (bulletPrefab.gameObject, place, Quaternion.identity);
	}

	private IEnumerator bulletSpawn()
	{
		float time = 0;
		while (time < timeToRemove) {
			yield return new WaitForSeconds (reloadTime);
			spawnBullet (GetComponent<Platform> ().LeftSpawn);
			spawnBullet (GetComponent<Platform> ().RightSpawn);
			time += reloadTime;
		}
		Destroy (this);
	}
		
	#region implemented abstract members of PlatformBonusComponent

	public override void RemoveBonus ()
	{
		StopCoroutine (bulletSpawnerCoroutine);
		Destroy (this);
	}

	#endregion
}
