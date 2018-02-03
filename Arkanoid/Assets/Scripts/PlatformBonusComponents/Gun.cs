using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : PlatformBonusComponent {

	private Bullet bulletPrefab;
	private Vector3 leftSpawn;
	private Vector3 rightSpawn;
	private float reloadTime;

	private Coroutine bulletSpawnerCoroutine;

	protected void Start () {
		bulletPrefab = GetComponent<Platform> ().BulletPrefab;
		reloadTime = GetComponent<Platform> ().ReloadTime;

		bulletSpawnerCoroutine = StartCoroutine (bulletSpawn ());
	}

	private void spawnBullet(Vector3 place)
	{
		Instantiate (bulletPrefab.gameObject, place, Quaternion.identity, transform);
	}

	private IEnumerator bulletSpawn()
	{
		while (true) {
			yield return new WaitForSeconds (reloadTime);
			spawnBullet (GetComponent<Platform> ().LeftSpawn);
			spawnBullet (GetComponent<Platform> ().RightSpawn);
		}
	}
		
	#region implemented abstract members of PlatformBonusComponent

	public override void RemoveBonus ()
	{
		StopCoroutine (bulletSpawnerCoroutine);
		Destroy (this);
	}

	#endregion
}
